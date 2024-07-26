using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDTesting.Assets.Script
{
    internal partial class CarMain : RigidBody3D
    {
        float steer = 0.5f;
        int gear = 1;
        int engineRPM;
        int enginePower { get { return GetEnginePower(); } }
        [Export] float steerRange;
        [Export] Wheel fr, fl, rr, rl;
        Wheel[] wheels;


        public override void _Ready()
        {
            wheels = new Wheel[4]{ fr,fl,rr, rl };
        }

        public override void _PhysicsProcess(double delta)
        {
            PowerWheels();
        }

        // Power the wheels, let them accelerate!
        void PowerWheels()
        {
            foreach (Wheel wheel in wheels)
            {
                
            }
        }

        int GetEnginePower()
        {
            switch (gear)
            {
                case 1: return 1;
                default: throw new NotImplementedException();
            }
        }
    }
}
