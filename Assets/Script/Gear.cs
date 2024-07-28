using Godot;

namespace SDTesting.Assets.Script
{

    internal partial class Gear : Resource
    {
        [Export] public float powerMultiplier, upperTransitionSpeed, lowerTransitionSpeed = float.MinValue;

        public static Gear Gear1 = new Gear() { powerMultiplier = 1.5f, upperTransitionSpeed = 80 };
        public static Gear Gear2 = new Gear() { powerMultiplier = 2f, upperTransitionSpeed = 100, lowerTransitionSpeed = 55 };
        public static Gear Gear3 = new Gear() { powerMultiplier = 2.5f, upperTransitionSpeed = 125, lowerTransitionSpeed = 75 };
        public static Gear Gear4 = new Gear() { powerMultiplier = 3.25f, upperTransitionSpeed = 175, lowerTransitionSpeed = 95 };
        public static Gear Gear5 = new Gear() { powerMultiplier = 4.5f, upperTransitionSpeed = float.MaxValue, lowerTransitionSpeed = 125 };
    }
}
