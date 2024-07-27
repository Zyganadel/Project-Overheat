using Godot;
using SDTesting.Assets.Script;

public partial class Car : VehicleBody3D
{
	[Export] float baseSteering { get; set; } = 0.5f;
	float steering;
	[Export] float baseEnginePower { get; set; } = 150; // For gear 1.
	float enginePower;
	float finalEnginePower;
	[Export] float baseBrakePower { get; set; } = 15;
	float brakePower;

	[Export] Gear[] gears { get; set; } = { Gear.Gear1, Gear.Gear2, Gear.Gear3, Gear.Gear4, Gear.Gear5 };
	Gear currentGear;

    int gear;
	[Export] VehicleWheel3D[] wheels;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gear = 0;
		steering = baseSteering;
		enginePower = baseEnginePower;
		finalEnginePower = baseEnginePower;
		brakePower = baseBrakePower;
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

				// Air control things here.
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print(LinearVelocity.Length());
		GearCheck();
	}

	void GearCheck()
	{
		currentGear = gears[gear];

		float speed = LinearVelocity.Length();

		bool changed = false;

		// Should we change gears?
		if (speed > currentGear.upperTransitionSpeed) { gear++; currentGear = gears[gear]; changed = true; }
		if (speed < currentGear.lowerTransitionSpeed) { gear--; currentGear = gears[gear]; changed = true; }

		if (changed)
		{
			enginePower = baseEnginePower * currentGear.powerMultiplier;
		}
	}

	float GetRPM()
	{
		float avgRpm = 0;
		foreach(VehicleWheel3D wheel in wheels)
		{
			avgRpm += wheel.GetRpm();
		}
		avgRpm /= 4;

		// Get average rpm relative to gear.
		return avgRpm /= currentGear.powerMultiplier;	
	}
}
