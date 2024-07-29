using Godot;

namespace SDTesting.Assets.Script.UI
{
    public partial class Timer : Control
    {
        double time = 0;

        public override void _Process(double delta)
        {
            base._Process(delta);

            time += delta;
        }

        public double End()
        {
            return time;
        }
    }
}
