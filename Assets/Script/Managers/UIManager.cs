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

        public MenuState State { get => state; set { SetState(value); } }
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
            UIHelper.Init();
            inputManager = new UIInputManager(); AddChild(inputManager);

            State = MenuState.Main; MenuUpdate(MenuState.Main);

            PrintOrphanNodes();
            PrintTree();
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
            UIHelper.HideAll();

            GD.Print($"Attempting setstate {state}");

            switch (state)
            {
                case MenuState.Main: UIHelper.MainHelper(); break;                  // This should also update GM.
                case MenuState.CarSelect: UIHelper.CarHelper(); break;
                case MenuState.LevelSelect: UIHelper.LevelHelper(); break;
                case MenuState.HUD: UIHelper.HudHelper(); break;                    // This should also update GM.
                case MenuState.Paused: UIHelper.PauseHelper(); break;               // This should also update GM.
                case MenuState.Leaderboard: UIHelper.LeaderboardHelper(); break;
                case MenuState.Options: UIHelper.OptionsHelper(); break;
                case MenuState.Credits: UIHelper.CreditsHelper(); break;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// A class for helper methods to be delegated.
        /// </summary>
        private static class UIHelper
        {
            public static void Init()
            {
                GD.Print("UIMan initialized.");
            }

            // This should be updated to always queuefree all UI elements.
            public static void HideAll()
            {
                try { Instance.mainMenu.QueueFree(); } catch (Exception) { }
                try { Instance.carSelect.QueueFree(); } catch (Exception) { }
                try { Instance.levelSelect.QueueFree(); } catch (Exception) { }
                try { Instance.hud.QueueFree(); } catch (Exception) { }
                try { Instance.pauseMenu.QueueFree(); } catch (Exception) { }
                try { Instance.leaderboard.QueueFree(); } catch (Exception) { }
                try { Instance.optionsMenu.QueueFree(); } catch (Exception) { }
                try { Instance.credits.QueueFree(); } catch (Exception) { }
            }

            public static void MainHelper()
            {
                if (GameManager.Instance != null)
                {
                    if (GameManager.Instance.State != GameState.Menu) { GameManager.Instance.State = GameState.Menu; }
                }
                Control c = (Control)Instance.mainMenuScene.Instantiate();
                Instance.mainMenu = c; Instance.AddChild(c); c.PrintTree();

                // Button things here.
                Button play = (Button)c.GetNode("panel/container/play");
                play.Pressed += delegate { Instance.State = MenuState.CarSelect; };
                Button leaderboards = (Button)c.GetNode("panel/container/leaderboards");
                leaderboards.Pressed += delegate { Instance.State = MenuState.Leaderboard; };
                Button options = (Button)c.GetNode("panel/container/options");
                options.Pressed += delegate { Instance.State = MenuState.Options; };
                Button credits = (Button)c.GetNode("panel/container/credits");
                credits.Pressed += delegate { Instance.State = MenuState.Credits; };
                Button quit = (Button)c.GetNode("panel/container/quit");
                quit.Pressed += delegate { Instance.GetTree().Quit(); };
            }

            public static void CarHelper()
            {
                Control c = (Control)Instance.carSelectScene.Instantiate();
                Instance.carSelect = c; Instance.AddChild(c); c.PrintTree();

                // button things
                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.State = MenuState.Main; };
                Button next = (Button)c.GetNode("next");
                next.Pressed += delegate { Instance.State = MenuState.LevelSelect; };
            }

            public static void LevelHelper()
            {
                Control c = (Control)Instance.levelSelectScene.Instantiate();
                Instance.levelSelect = c; Instance.AddChild(c); c.PrintTree();

                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.State = MenuState.CarSelect; };
                Button play = (Button)c.GetNode("next");
                play.Pressed += delegate { Instance.State = MenuState.HUD; };
            }

            public static void PauseHelper()
            {
                if (GameManager.Instance.State != GameState.GameplayPaused) { GameManager.Instance.State = GameState.GameplayPaused; }
                Control c = (Control)Instance.pauseMenuScene.Instantiate();
                Instance.pauseMenu = c; Instance.AddChild(c); c.PrintTree();

                Button backButton = (Button)c.GetNode("panel/container/mainmenu");
                backButton.Pressed += delegate { Instance.State = MenuState.Main; };
                Button resumeButton = (Button)c.GetNode("panel/container/resume");
                resumeButton.Pressed += delegate { Instance.State = MenuState.HUD; };
            }

            public static void HudHelper()
            {
                if (GameManager.Instance.State != GameState.Gameplay) { GameManager.Instance.State = GameState.Gameplay; }
                HUD h = (HUD)Instance.hudScene.Instantiate();
                Instance.hud = h; Instance.AddChild(h);
            }

            public static void LeaderboardHelper()
            {
                Control c = (Control)Instance.leaderboardScene.Instantiate();
                Instance.leaderboard = c; Instance.AddChild(c); c.PrintTree();

                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.State = MenuState.Main; };
            }

            public static void OptionsHelper()
            {
                Control c = (Control)Instance.optionsMenuScene.Instantiate();
                Instance.optionsMenu = c; Instance.AddChild(c); c.PrintTree();

                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.State = MenuState.Main; };
            }

            public static void CreditsHelper()
            {
                Control c = (Control)Instance.creditsScene.Instantiate();
                Instance.credits = c; Instance.AddChild(c); c.PrintTree();

                Button backButton = (Button)c.GetNode("back");
                backButton.Pressed += delegate { Instance.State = MenuState.Main; };
            }
        }
    }
}
