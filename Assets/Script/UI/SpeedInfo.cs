using Godot;
using System;

namespace SDTesting.Assets.Script.UI
{
    internal partial class SpeedInfo : Control
    {
        [Export] Label speedLabel, gearLabel, rpmLabel;

        public void Update(int speed, int gear, int rpm)
        {
            try
            {
                speedLabel.Text = $"Speed: {speed}km/h";
                gearLabel.Text = $"Gear: {gear}";
                rpmLabel.Text = $"RPM: {rpm}";
            }
            catch (Exception e) { GD.Print($"{e.GetType()}, {e.Message} {e.StackTrace}"); }
        }
    }
}
