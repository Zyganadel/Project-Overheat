using Godot;
using System;

namespace SDTesting.Assets.Script.UI
{
    internal partial class HeatInfo : Control
    {
        [Export] Label heatLabel, penaltyLabel;

        public void Update(int heat, float penalty)
        {
            heatLabel.Text = $"Heat: {heat}";
            penaltyLabel.Text = $"Penalty: {MathF.Round(penalty, 3)}";
        }
    }
}
