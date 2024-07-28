using Godot;
using SDTesting.Assets.Script.UI;
using System;

namespace SDTesting.Assets.Script.Managers
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

        UIInputManager inputManager;

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
            inputManager = new UIInputManager(); AddChild(inputManager);
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
                Control c = (Control)Instance.mainMenuScene.Instantiate();
                Instance.mainMenu = c;

                // Button things here.
            }

            public static void CarHelper()
            {
                Control c = (Control)Instance.carSelectScene.Instantiate();
                Instance.carSelect = c;

                // button things
                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.MenuUpdate(MenuState.Main); };
            }

            public static void LevelHelper()
            {
                Control c = (Control)Instance.levelSelectScene.Instantiate();
                Instance.levelSelect = c;

                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.MenuUpdate(MenuState.CarSelect); };
            }

            public static void PauseHelper()
            {
                Control c = (Control)Instance.pauseMenuScene.Instantiate();
                Instance.pauseMenu = c;

                Button backButton = (Button)c.GetNode("panel/container/mainmenu");
                backButton.Pressed += delegate { Instance.MenuUpdate(MenuState.Main); };
            }

            public static void LeaderboardHelper()
            {
                Control c = (Control)Instance.leaderboardScene.Instantiate();
                Instance.leaderboard = c;

                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.MenuUpdate(MenuState.Main); };
            }

            public static void OptionsHelper()
            {
                Control c = (Control)Instance.optionsMenuScene.Instantiate();
                Instance.optionsMenu = c;

                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.MenuUpdate(MenuState.Main); };
            }

            public static void CreditsHelper()
            {
                Control c = (Control)Instance.creditsScene.Instantiate();
                Instance.credits = c;

                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.MenuUpdate(MenuState.Main); };
            }
        }
    }
}
