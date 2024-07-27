using Godot;

namespace SDTesting.Assets.Script.UI
{
    public partial class HUD : Control
    {
        public static HUD Instance { get; private set; }

        [Export] SpeedInfo speedInfo;

        public override void _Ready()
        {
            Instance = this;
        }

        public void UpdateHeat(float heat, float penalty)
        {

        }

        public void UpdateSpeed(float speed, int gear, float rpm)
        {
            int speedInt, gearInt, rpmInt;
            speedInt = (int)(speed * 3.6); // Convert to kph from m/s
            gearInt = gear + 1; // Convert from index to normie readable number.
            rpmInt = (int)rpm;

            speedInfo.Update(speedInt, gearInt, rpmInt);
        }
    }
}