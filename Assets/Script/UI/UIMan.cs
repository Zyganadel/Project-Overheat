using Godot;

namespace SDTesting.Assets.Script.UI
{
    /// <summary>
    /// A manager for UI Things
    /// </summary>
    public partial class UIMan : Control
    {
        public static UIMan Instance { get; private set; }

        [Export] PackedScene hudScene;
        [Export] PackedScene mainMenuScene;
        [Export] PackedScene pauseMenuScene;
        public HUD hud { get; private set; }
        public Menu mainMenu { get; private set; }
        public Menu pauseMenu { get; private set; }

        public override void _Ready()
        {
            // Singleton
            Instance = this;

            // Other stuff.
        }
    }
}
