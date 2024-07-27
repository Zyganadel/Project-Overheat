using Godot;
using SDTesting.Assets.Script;
using System.Collections.Generic;

public partial class Car : VehicleBody3D
{
	[Export] float baseSteering { get; set; } = 0.75f;
	float steering;
	[Export] float baseWheelFriction = 0.9f;
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
				foreach (VehicleWheel3D wheel in wheels)
				{
					wheel.WheelFrictionSlip = baseWheelFriction - brakeInput * brakeFrictionReduc;
				}

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
	}

	void GearCheck()
	{
		currentGear = gears[gearIndex];

		float speed = LinearVelocity.Length();

		bool changed = false;

		// Should we change gears?
		if (speed > currentGear.upperTransitionSpeed) { gearIndex++; currentGear = gears[gearIndex]; changed = true; }
		if (speed < currentGear.lowerTransitionSpeed) { gearIndex--; currentGear = gears[gearIndex]; changed = true; }

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
}
