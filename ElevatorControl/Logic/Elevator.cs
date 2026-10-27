using System;

namespace ElevatorControl.Logic
{
    public sealed class Elevator
    {
        public string Name { get; set; } = "Elevator";
        public int CurrentFloor { get; set; }
        public bool DoorsOpen { get; set; }
        public bool IsEmergencyStopped { get; set; }

        public void AddStop(int floor) { }
        public void OpenDoors() { }
        public void CloseDoors() { }
        public void EmergencyStop() { }
        public void Resume() { }
        public double EstimateResponseSeconds(int floor) => 0;
    }
}
