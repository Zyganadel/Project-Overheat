using Godot;
using SDTesting.Assets.Script.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDTesting.Assets.Script.Managers
{
    public enum GameState
    {
        Menu,
        GameplayIntro,
        Gameplay
    }

    internal partial class GameManager : Node3D
    {
        public static GameManager Instance { get; private set; }

        [Export] UIManager UIMan;

        [Export] PackedScene Car;
        Car car;

        [Export]PackedScene[] mapScenes;
        Map currentMap;


    }
}
