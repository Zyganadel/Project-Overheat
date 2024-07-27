using Godot;

namespace SDTesting.Assets.Script
{

    internal partial class Gear : Resource
    {
        [Export] public float powerMultiplier, upperTransitionSpeed, lowerTransitionSpeed = -1;

        public static Gear Gear1 = new Gear() { powerMultiplier = 1, upperTransitionSpeed = 15 };
        public static Gear Gear2 = new Gear() { powerMultiplier = 1.5f, upperTransitionSpeed = 25, lowerTransitionSpeed = 13 };
        public static Gear Gear3 = new Gear() { powerMultiplier = 1.9f, upperTransitionSpeed = 35, lowerTransitionSpeed = 20 };
        public static Gear Gear4 = new Gear() { powerMultiplier = 2.25f, upperTransitionSpeed = 45, lowerTransitionSpeed = 30 };
        public static Gear Gear5 = new Gear() { powerMultiplier = 2.5f, upperTransitionSpeed = float.MaxValue, lowerTransitionSpeed = 40 };
    }
}
