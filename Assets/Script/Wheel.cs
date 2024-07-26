using Godot;
using System;

public partial class Wheel : RigidBody3D
{
	bool isGrounded = false;

	public float grip = 1;
	public int rpm = 0;
	[Export] public float rotDist;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GroundCheck();
	}

	/// <summary>
	/// Checks if we're touching ground/wall/ceiling in any way, and then does things with that info.
	/// </summary>
	void GroundCheck()
	{
		PhysicsBody3D groundObj = null;

		// Check if anything we're colliding with is ground. If yes, we're grounded.
		foreach(Node3D obj in GetCollidingBodies())
		{
			// Determine if things are ground.
			bool isGround = true;

			if(isGround) { isGrounded = true; }

			try { groundObj = (PhysicsBody3D)obj; }catch(Exception e) { }
		}

		// Something to do with grip here?


	}
}
