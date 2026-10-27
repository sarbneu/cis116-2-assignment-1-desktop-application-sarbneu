using System;

namespace ElevatorControl.Logic
{
    public interface IElevatorState { }
    public interface IElevatorObserver { void Update(string message); }
}
