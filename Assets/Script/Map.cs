using Godot;
using SDTesting.Assets.Script.Managers;

namespace SDTesting.Assets.Script
{
    internal partial class Map : Node3D
    {
        [Export] Area3D endTrigger;
        [Export] public Vector3 startPos;

        public override void _Ready()
        {
            base._Ready();

            endTrigger.BodyEntered += delegate { GameManager.Instance.State = GameState.Menu; };
        }
    }
}
