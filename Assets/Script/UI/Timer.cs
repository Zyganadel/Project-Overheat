using Godot;
using SDTesting.Assets.Script.Managers;
using System;

namespace SDTesting.Assets.Script.UI
{
    public partial class Timer : Control
    {
        [Export] Label timerLabel;

        double time = 0;

        public override void _Process(double delta)
        {
            base._Process(delta);

            time += delta;

            timerLabel.Text = MathF.Round((float)time, 2).ToString();
        }

        public double End()
        {
            return time;
        }

        public override void _ExitTree()
        {
            LeaderboardManager.SetTime(MathF.Round((float)time, 2));
        }
    }
}
