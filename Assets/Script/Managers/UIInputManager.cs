using Godot;

namespace SDTesting.Assets.Script.Managers
{
    internal partial class UIInputManager : Node
    {
        CarManager carMan;

        public UIInputManager()
        {
            carMan = CarManager.Instance;
        }

        public override void _UnhandledKeyInput(InputEvent keyEvent)
        {
            base._UnhandledKeyInput(keyEvent);

            // Determine if we're in a valid UI, then do things.
            if (Input.IsActionJustPressed("pause") && UIManager.Instance.State == MenuState.HUD) { UIManager.Instance.State = MenuState.Paused; }
        }
    }
}
