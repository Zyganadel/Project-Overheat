using Godot;

namespace SDTesting.Assets.Script.UI
{
    /// <summary>
    /// Represents a menu.
    /// </summary>
    public partial class Menu : Control
    {
        public static Menu Instance { get; private set; }

        [Export] public static PackedScene prefab;

        public static void Enable()
        {
            Instance = (Menu)prefab.Instantiate();
        }

        public static void Disable()
        {
            Instance.QueueFree();
        }
    }
}