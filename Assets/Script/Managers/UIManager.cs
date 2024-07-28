using Godot;
using System;

namespace SDTesting.Assets.Script.UI
{
    public enum MenuState
    {
        Main,
        CarSelect,
        LevelSelect,
        HUD,
        Paused,
        Leaderboard,
        Options,
        Credits
    }

    /// <summary>
    /// A manager for UI Things
    /// </summary>
    public partial class UIManager : Control
    {
        public static UIManager Instance { get; private set; }

        public MenuState State { get => state; set => state = value; }
        private MenuState state;

        [Export] PackedScene mainMenuScene;
        [Export] PackedScene carSelectScene;
        [Export] PackedScene levelSelectScene;
        [Export] PackedScene hudScene;
        [Export] PackedScene pauseMenuScene;
        [Export] PackedScene leaderboardScene;
        [Export] PackedScene optionsMenuScene;
        [Export] PackedScene creditsScene;
        public Control mainMenu { get; private set; }
        public Control carSelect { get; private set; }
        public Control levelSelect { get; private set; }
        public HUD hud { get; private set; }
        public Control pauseMenu { get; private set; }
        public Control leaderboard { get; private set; }
        public Control optionsMenu { get; private set; }
        public Control credits { get; private set; }

        public override void _Ready()
        {
            // Singleton
            Instance = this;

            // Other stuff.
            UIHelpers.Init();
        }

        public override void _UnhandledKeyInput(InputEvent keyEvent)
        {
            base._UnhandledKeyInput(keyEvent);

            if (Input.IsActionJustPressed("pause")) { State = MenuState.Paused; }
        }

        private void SetState(MenuState state)
        {
            if (state != this.state)
            {
                this.state = state;
                MenuUpdate(state);
            }
        }

        void MenuUpdate(MenuState state)
        {
            UIHelpers.HideAll();

            switch (state)
            {
                case MenuState.Main:
                case MenuState.CarSelect:
                case MenuState.LevelSelect:
                case MenuState.HUD:
                case MenuState.Paused:
                case MenuState.Leaderboard:
                case MenuState.Options:
                case MenuState.Credits:
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// A class for helper methods to be delegated.
        /// </summary>
        private static class UIHelpers
        {
            public static void Init()
            {
                GD.Print("UIMan initialized.");
            }

            // This should be updated to always queuefree all UI elements.
            public static void HideAll()
            {
                Instance.mainMenu.QueueFree();
                Instance.carSelect.QueueFree();
                Instance.levelSelect.QueueFree();
                Instance.hud.QueueFree();
                Instance.pauseMenu.QueueFree();
                Instance.leaderboard.QueueFree();
                Instance.optionsMenu.QueueFree();
                Instance.credits.QueueFree();
            }

            public static void MainHelper()
            {
                Instance.mainMenu = (Control)Instance.mainMenuScene.Instantiate();

                // Button things here.
            }

            public static void CarHelper()
            {
                Instance.carSelect = (Control)Instance.carSelectScene.Instantiate();

                // button things
            }


        }
    }
}
