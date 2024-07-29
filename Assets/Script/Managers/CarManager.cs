using Godot;

namespace SDTesting.Assets.Script.Managers
{
    /// <summary>
    /// This should handle the car, when/where it spawns, and what upgrades are used.
    /// </summary>
    internal partial class CarManager : Node
    {
        [Export] PackedScene[] carScenes;
        int currentCarIndex;
        Car currentCar;

        float[] radiators = new float[] { 1, 1.1f, 1.5f };
        float[] tires = new float[] { 1, 1.25f };
        float[] engines = new float[] { 1, 1.1f, 1.25f };
        int currentRadiator = 0;
        int currentTires = 0;
        int currentEngines = 0;

        public void KillCar()
        {
            currentCar.QueueFree();
        }

        public Car SpawnCar(Vector3 position)
        {
            Car car = (Car)carScenes[currentCarIndex].Instantiate();
            GameManager.Instance.AddChild(car);

            car.Position = position;
            car.SetStartValues(radiators[currentRadiator], tires[currentTires], engines[currentEngines]);

            return car;
        }
    }
}
