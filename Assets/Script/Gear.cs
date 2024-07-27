using Godot;

namespace SDTesting.Assets.Script
{

    internal partial class Gear : Resource
    {
        [Export] public float powerMultiplier, upperTransitionSpeed, lowerTransitionSpeed = float.MinValue;

        public static Gear Gear1 = new Gear() { powerMultiplier = 1, upperTransitionSpeed = 100 };
        public static Gear Gear2 = new Gear() { powerMultiplier = 2f, upperTransitionSpeed = 100, lowerTransitionSpeed = 30 };
        public static Gear Gear3 = new Gear() { powerMultiplier = 3f, upperTransitionSpeed = 100, lowerTransitionSpeed = 30 };
        public static Gear Gear4 = new Gear() { powerMultiplier = 4.5f, upperTransitionSpeed = 100, lowerTransitionSpeed = 30 };
        public static Gear Gear5 = new Gear() { powerMultiplier = 6.5f, upperTransitionSpeed = float.MaxValue, lowerTransitionSpeed = 30 };
    }
}
