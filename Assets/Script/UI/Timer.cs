using Godot;
using SDTesting.Assets.Script.Managers;

namespace SDTesting.Assets.Script.UI
{
    public partial class Timer : Control
    {
        [Export]Label timerLabel;

        double time = 0;

        public override void _Process(double delta)
        {
            base._Process(delta);

            time += delta;

            timerLabel.Text = time.ToString();
        }

        public double End()
        {
            return time;
        }

        public override void _ExitTree()
        {
            LeaderboardManager.SetTime(time);
        }
    }
}
