using Godot;
using System;

namespace SDTesting.Assets.Script.Managers
{
    public enum GameState
    {
        Menu,
        GameplayPaused,
        Gameplay
    }

    internal partial class GameManager : Node3D
    {
        public static GameManager Instance { get; private set; }

        public GameState State { get => state; set { SetState(value); } }
        GameState state;

        [Export] UIManager UIMan;
        [Export] CarManager carManager;
        Car car;

        [Export] PackedScene[] mapScenes;
        public int currentMapIndex = 0;
        Map currentMap;

        void SetState(GameState state)
        {
            if (this.state != state)
            {
                this.state = state;
                UpdateState(state);
            }
        }

        void UpdateState(GameState state)
        {
            switch (state)
            {
                case GameState.Menu: MenuHelper(); break;
                case GameState.GameplayPaused: GameplayPausedHelper(); break;
                case GameState.Gameplay: GameplayHelper(); break;
                default: throw new NotImplementedException();
            }

            void MenuHelper()
            {
                // Check if we've set menu states
                if (UIMan.State != MenuState.Main) { UIMan.State = MenuState.Main; }

                carManager.KillCar(); car = null;
            }

            void GameplayPausedHelper()
            {

            }

            void GameplayHelper()
            {
                // Check if we set menu states
                if (UIMan.State != MenuState.HUD) { UIMan.State = MenuState.HUD; }

                currentMap = (Map)mapScenes[currentMapIndex].Instantiate(); AddChild(currentMap);
                car = carManager.SpawnCar(currentMap.startPos);
            }
        }

        public override void _Ready()
        {
            base._Ready();
            Instance = this;
            State = GameState.Menu;
        }
    }
}
