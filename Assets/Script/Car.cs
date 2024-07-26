using Godot;

public partial class Car : VehicleBody3D
{
	float maxSteer { get; } = 0.5f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _UnhandledInput(InputEvent ie)
	{
		if(ie is InputEventKey)
		{
			OnKeyboardEvent();
		}

		void OnKeyboardEvent()
		{
			float steer = Input.GetAxis("right", "left");
			float accel = Input.GetActionStrength("forward");
			float brake = Input.GetActionStrength("brake");

			UpdateSteer();
			UpdateEnginePower();
			UpdateBrakes();

			// Anything that happens with steering, do it here.
			void UpdateSteer()
			{
				Steering = steer * maxSteer;

				// Also need aircontrol things here too.
			}

			// This will need to have gear management too.
			void UpdateEnginePower()
			{
				// For now, just set the power to a value.
				float maxpower = 100;
				EngineForce = accel * maxpower;
			}

			// Handle braking things here.
			void UpdateBrakes()
			{
				float maxbrakes = 15;
				Brake = brake * maxbrakes;

				// Air control things here.
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
