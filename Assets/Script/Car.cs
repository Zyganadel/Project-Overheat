using Godot;
using SDTesting.Assets.Script;
using SDTesting.Assets.Script.UI;
using System;
using System.Collections.Generic;

public partial class Car : VehicleBody3D
{
    // Engine values.
    [ExportCategory("Engine")]
    [Export] float baseSteering = 0.5f;
    [Export] float baseWheelFriction = 0.9f;
    [Export] float rearWheelFrictionReduc = 0.2f;
    [Export] float brakeFrictionReduc = 0.3f;
    [Export] float heatBrakePenalty = 0.3f;
    [Export] float baseEnginePower = 450; // For gear 1.
    [Export] float heatEnginePenalty = 0.3f;
    [Export] float baseBrakePower = 25;
    [Export] float sdModifier = 0.25f;
    float steering;
    float enginePower;
    float finalEnginePower;

    [Export] Gear[] gears = { Gear.Gear1, Gear.Gear2, Gear.Gear3, Gear.Gear4, Gear.Gear5 };
    Gear currentGear;
    int gearIndex;
    [Export] VehicleWheel3D[] wheels;
    VehicleWheel3D rr, lr, rf, lf;

    // Heat Mechanics
    [ExportCategory("Heat")]
    [Export] float brakeHeat = 0.2f;
    [Export] float engineHeat = 0.4f;
    [Export] float heatPerRPMSecond = 75;
    [Export] float maxHeat = 50; float excessHeat = 0;
    [Export] float cooling = 0.5f; // Cooling per second.
    float currentHeat;

    float speedLastFrame = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gearIndex = 0;
        steering = baseSteering;
        finalEnginePower = baseEnginePower;

        GetWheels();

        void GetWheels()
        {
            List<VehicleWheel3D> wheelList = new List<VehicleWheel3D>();
            foreach (Node3D node in GetChildren())
            {
                if (node is VehicleWheel3D) { wheelList.Add((VehicleWheel3D)node); }
            }
            wheels = wheelList.ToArray();

            lf = (VehicleWheel3D)GetNode("Wheel_LF");
            rf = (VehicleWheel3D)GetNode("Wheel_RF");
            rr = (VehicleWheel3D)GetNode("Wheel_RR");
            lr = (VehicleWheel3D)GetNode("Wheel_LR");
        }
    }

    public override void _UnhandledInput(InputEvent ie)
    {
        if (ie is InputEventKey)
        {
            OnKeyboardEvent();
        }

        void OnKeyboardEvent()
        {
            float steerInput = Input.GetAxis("right", "left");
            float accelInput = Input.GetActionStrength("forward");
            float brakeInput = Input.GetActionStrength("brake");

            UpdateSteer();
            UpdateEnginePower();
            UpdateBrakes();

            // Anything that happens with steering, do it here.
            void UpdateSteer()
            {
                Steering = steerInput * steering;

                // Also need aircontrol things here too.
            }

            // This will need to have gear management too.
            void UpdateEnginePower()
            {
                GD.Print($"BP = {baseEnginePower}, EP = {enginePower}, FP = {finalEnginePower}");

                float finalHeatPenalty = Mathf.Clamp(heatEnginePenalty * excessHeat, 0, 1);
                finalEnginePower = enginePower * (1 - finalHeatPenalty);

                EngineForce = accelInput * finalEnginePower;
            }

            // Handle braking things here.
            void UpdateBrakes()
            {
                float brakePower;
                float brakeFrictionReduc = this.brakeFrictionReduc;

                // Handle overheating.
                brakeFrictionReduc += heatBrakePenalty * excessHeat;
                brakePower = baseBrakePower * (1 - heatBrakePenalty * excessHeat);

                Brake = brakeInput * brakePower;

                // Make drifts work a bit nicer?
                float rearFriction = baseWheelFriction - rearWheelFrictionReduc - brakeInput * brakeFrictionReduc;
                float forwardFriction = baseWheelFriction - brakeInput * brakeFrictionReduc;
                lf.WheelFrictionSlip = forwardFriction;
                rf.WheelFrictionSlip = forwardFriction;
                lr.WheelFrictionSlip = rearFriction;
                rr.WheelFrictionSlip = rearFriction;

                // Air control things here.
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        GearCheck();
        GD.Print($"RPM is {GetRPM()}, speed is {LinearVelocity.Length()}");
        HeatTick(delta);

        // Hud updates, may need to be moved eventually.
        HUD.Instance.UpdateSpeed(LinearVelocity.Length(), gearIndex, GetRPM());
        HUD.Instance.UpdateHeat(currentHeat, excessHeat);
    }

    void GearCheck()
    {
        try { currentGear = gears[gearIndex]; } catch (IndexOutOfRangeException e) { currentGear = gears[0]; GD.PrintErr($"{e.GetType()} at gearcheck."); }

        float speed = LinearVelocity.Length();
        float RPM = GetRPM();

        // Should we change gears?
        if (RPM > currentGear.upperTransitionSpeed) { gearIndex++; currentGear = gears[gearIndex]; }
        if (RPM < currentGear.lowerTransitionSpeed) { gearIndex--; currentGear = gears[gearIndex]; }

        enginePower = baseEnginePower * currentGear.powerMultiplier;
    }

    float GetRPM()
    {
        float avgRpm = 0;
        foreach (VehicleWheel3D wheel in wheels)
        {
            avgRpm += wheel.GetRpm();
        }
        avgRpm = avgRpm / 4;

        // Get average rpm relative to gear.
        return avgRpm / currentGear.powerMultiplier;
    }

    float GetAvgGrip()
    {
        float totalGrip = 0;
        foreach (VehicleWheel3D wheel in wheels)
        {
            totalGrip += wheel.GetSkidinfo();
        }
        return totalGrip / 4;
    }

    void HeatTick(double delta)
    {
        // Actually apply the heat.
        if (Brake != 0) { TickBrake(); }
        TickEngine();
        currentHeat -= (float)(cooling * delta);

        // Now calculate whether or not any adverse effects should happen.
        float tempExcess = (currentHeat - 50) / 50;
        excessHeat = Mathf.Clamp(tempExcess, 0, float.MaxValue);

        void TickEngine()
        {
            float engineHeat = this.engineHeat;
            float accelInput = EngineForce / finalEnginePower;

            // If we're accelerating, gain extra heat based on our rpm.
            if (accelInput != 0)
            {
                float extraHeat = (GetRPM() / heatPerRPMSecond) * this.engineHeat;
                engineHeat += extraHeat;
            }

            // Apply heat.
            currentHeat += (float)(engineHeat * delta);
        }

        void TickBrake() { currentHeat += (float)(delta * brakeHeat); }
    }
}
