using Godot;

namespace SDTesting.Assets.Script.UI
{
    public partial class HUD : Control
    {
        public static HUD Instance { get; private set; }

        [Export] SpeedInfo speedInfo;
        [Export] HeatInfo heatInfo;

        public override void _Ready()
        {
            Instance = this;
        }

        public void UpdateHeat(float heat, float penalty)
        {
            heatInfo.Update((int)heat, penalty);
        }

        public void UpdateSpeed(float speed, int gear, float rpm)
        {
            int speedInt, gearInt, rpmInt;
            speedInt = (int)(speed * 3.6); // Convert to kph from m/s
            gearInt = gear + 1; // Convert from index to normie readable number.
            rpmInt = (int)(rpm * 15 + 500);

            speedInfo.Update(speedInt, gearInt, rpmInt);
        }
    }
}
