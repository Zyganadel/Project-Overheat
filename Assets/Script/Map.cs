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

            endTrigger.BodyEntered += ExitLevel;

            void ExitLevel(Node3D node) { GameManager.Instance.State = GameState.Menu; }
        }
    }
}
