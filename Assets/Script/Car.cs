using Godot;
using SDTesting.Assets.Script;
using System;
using System.Collections.Generic;

public partial class Car : VehicleBody3D
{
	[Export] float baseSteering { get; set; } = 0.5f;
	float steering;
	[Export] float baseWheelFriction = 0.9f;
	[Export] float rearWheelFrictionReduc = 0.2f;
	[Export] float brakeFrictionReduc = 0.3f;
	[Export] float baseEnginePower { get; set; } = 150; // For gear 1.
	float enginePower;
	float finalEnginePower;
	[Export] float baseBrakePower { get; set; } = 25;
	float brakePower;

	[Export] Gear[] gears { get; set; } = { Gear.Gear1, Gear.Gear2, Gear.Gear3, Gear.Gear4, Gear.Gear5 };
	Gear currentGear;

	int gearIndex;
	[Export] VehicleWheel3D[] wheels;
	VehicleWheel3D rr, lr, rf, lf;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gearIndex = 0;
		steering = baseSteering;
		finalEnginePower = baseEnginePower;
		brakePower = baseBrakePower;

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
				EngineForce = accelInput * finalEnginePower;
			}

			// Handle braking things here.
			void UpdateBrakes()
			{
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
		GD.Print(LinearVelocity.Length());
		GD.Print(GetRPM());
		GripDebug();
	}

	void GearCheck()
	{
		try { currentGear = gears[gearIndex]; } catch(IndexOutOfRangeException e) { currentGear = gears[0]; GD.PrintErr($"{e.GetType()} at gearcheck."); }

		float speed = LinearVelocity.Length();
		float RPM = GetRPM();

		bool changed = false;

		// Should we change gears?
		if (RPM > currentGear.upperTransitionSpeed) { gearIndex++; currentGear = gears[gearIndex]; changed = true; }
		if (RPM < currentGear.lowerTransitionSpeed) { gearIndex--; currentGear = gears[gearIndex]; changed = true; }

		if (changed)
		{
			enginePower = baseEnginePower * currentGear.powerMultiplier;
		}
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

	// Speed Drifts!!!
	void SDCheck(double delta)
	{

	}

	void GripDebug()
	{
		foreach (VehicleWheel3D wheel in wheels)
		{
			GD.Print(wheel.GetSkidinfo());
		}
	}
}
