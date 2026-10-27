# Elevator Control - Setup Guide (Visual Studio + MySQL Workbench)

C# WinForms project for CIS016-2 Assignment 1. Two elevators, State + Observer patterns,
MySQL logging through `MySqlDataAdapter`, timer-based animation, `BackgroundWorker` for database work.

```text
studentID_AssignmentOne/
  ElevatorControl.sln            <- open this in Visual Studio
  README_SETUP.md                <- this file
  TestReport_Template.md         <- Task 6 report skeleton + marking matrix
  Database/elevator_db.sql       <- run this in MySQL Workbench
  ElevatorControl/
    appsettings.json             <- your MySQL user/password
    Program.cs, MainForm.cs      <- GUI + event handlers (Tasks 1, 2, 4)
    Logic/                       <- Elevator, states, controller (Tasks 2, 4, 5.2)
    Persistence/                 <- MySQL access, BackgroundWorker helper (Tasks 3, 5.1)
    UI/ShaftView.cs              <- animated drawing of the shafts
```

## 1. What you need installed

| Tool | Notes |
|------|-------|
| Visual Studio 2022 (17.8 or newer) | Workload **".NET desktop development"** must be ticked (Tools > Get Tools and Features). |
| MySQL Server 8.x + MySQL Workbench | The server must be running (Windows: `services.msc` > MySQL80 > Start). |

If your Visual Studio is older and complains about .NET 8, open `ElevatorControl/ElevatorControl.csproj`
and change `net8.0-windows` to `net6.0-windows`.

## 2. Create the database (MySQL Workbench)

1. Open Workbench and connect to your local instance (usually `Local instance MySQL80`).
2. **File > Open SQL Script...** and choose `Database/elevator_db.sql`.
3. Click the lightning-bolt button (Execute). You should see `elevator_db` appear under **Schemas** (press refresh if not).
4. Test: `SELECT * FROM elevator_db.elevator_log;` returns an empty table.

## 3. Tell the program your MySQL password

Open `ElevatorControl/appsettings.json` and replace `CHANGE_ME` with your MySQL root password
(or use another user that can access `elevator_db`). Visual Studio copies this file next to the .exe on build,
and the program finds it with a relative path (`AppContext.BaseDirectory`), so the project works on any PC.

## 4. Build and run

1. Double-click `ElevatorControl.sln`.
2. Wait for NuGet to restore the **MySql.Data** package (needs internet the first time).
3. Press **F5**. The status bar at the bottom right should say `Database: connected ...`.

## 5. Using the program

* **Hall calls (outside)** - the arrow buttons. The system picks the elevator with the shortest estimated
  response time; the button stays yellow until that elevator arrives.
* **Car panel (inside)** - floor buttons, open/close door and emergency stop for each elevator. Inside requests are
  handled only by that elevator.
* **Log** - every event is written to MySQL in the background and the grid refreshes itself.

## 6. Troubleshooting

| Message / symptom | Fix |
|---|---|
| `Unable to connect to any of the specified MySQL hosts` | MySQL service not running, or wrong Server/Port in `appsettings.json`. |
| `Access denied for user 'root'` | Wrong password in `appsettings.json`. |
| `Unknown database 'elevator_db'` / `Table ... doesn't exist` | Step 2 not done (script not executed). |
| `The type or namespace name 'MySql' could not be found` | NuGet restore failed: right-click the solution > Restore NuGet Packages. |
| Build error about the target framework | See the note under section 1 (use `net6.0-windows`). |
| Elevators move but the log grid is empty | Look at the red text bottom right; the simulation works without the database and the queued entries are saved once it is reachable. |

## 7. Where each marking-matrix item lives (use this for your self-assessment and demo)

| Task | Where |
|------|-------|
| 1 GUI | `MainForm.BuildLayout` and the `Build...` methods |
| 1 Event handler skeletons | `HallCallButton_Click`, `CarFloorButton_Click`, `OpenDoorButton_Click`, `CloseDoorButton_Click`, `EmergencyButton_Click`, `RefreshLogButton_Click` in `MainForm.cs` |
| 2 Handlers work as specified | `Logic/Elevator.cs` (`AddStop`, `OpenDoors`, `CloseDoors`, `EmergencyStop`) |
| 3 DB designed and connected | `Database/elevator_db.sql`, `Persistence/AppSettings.cs` |
| 3 Log shown in GUI | `LogRepository.LoadLogs` + `MainForm.OnLogsLoaded` |
| 3 Log stored in DB | `LogRepository.SaveEntries` + `LoggingObserver` |
| 3 Disconnected model | `MySqlDataAdapter.Fill` and `adapter.Update(table)` in `LogRepository` (no `ExecuteNonQuery`) |
| 3 Relative paths / exceptions | `AppSettings.SettingsPath`; `try/catch` in `AppSettings.Load`, `LogRepository.Guard`, `OnDatabaseError` |
| 3 No duplicated DB code | one `Guard` helper, one `DbJob<T>` class, all DB handlers go through `RequestReload` / `EnqueueLog` |
| 4 Animation (delegates + timer) | `MainForm.AnimationTimer_Tick` (WinForms `Timer`), `ElevatorEventHandler` delegate in `Logic/Events.cs`, drawing in `UI/ShaftView.cs` |
| 5.1 BackgroundWorker | `Persistence/DbJob.cs`, used by `_loadJob` and `_saveJob` in `MainForm` |
| 5.2 Two elevators, State, Observer | `Logic/ElevatorStates.cs` (State), `IElevatorObserver` + `Elevator.Notify` (Observer), `ElevatorController.RequestFromHall` + `Elevator.EstimateResponseSecon[...]
| 6 Test report | `TestReport_Template.md` |

## 8. How the logic works (for the Week 7 demonstration)

* **State pattern** - `Elevator` holds a current `IElevatorState` (`IdleState`, `MovingUpState`, `MovingDownState`,
  `DoorsOpenState`, `EmergencyStopState`). The timer calls `Elevator.Tick(dt)`, which just calls `Tick` on the current
  state; each state moves the car, or changes the state, by itself.
* **Observer pattern** - `Elevator` keeps a list of `IElevatorObserver`s and calls `Notify` on every event. Two observers
  are attached: `LoggingObserver` (writes the log) and `MainForm` (button highlights). The same event is also raised through
  the `ElevatorEventHandler` delegate, which updates the status line.
* **Dispatching** - `ElevatorController.RequestFromHall` asks each elevator for `EstimateResponseSeconds(floor)`. That method
  replays the elevator's remaining stops (including door time) in the same SCAN order the real elevator will use, and returns the
  time until the new floor is reached. The call goes to the smallest estimate; a tie goes to Elevator A. The floor is only
  *added* to that elevator's stop list, so nothing already queued is dropped. Emergency-stopped elevators estimate infinity.
* **Animation** - a 30 ms `Timer` advances elevator positions (floors per second) and repaints `ShaftView`.
* **Database** - events go into a thread-safe queue; a `BackgroundWorker` writes the batch with `DataAdapter.Update`, then a
  second one reloads the grid, so the GUI never waits for MySQL.

## 9. Things to check against your own brief

The pasted brief started at Task 3's bullet points, so **Tasks 1 and 2 (exact GUI and button behaviour) were not visible**.
This project assumes: 5 floors (G-4), outside Up/Down buttons, inside floor buttons, open/close door, emergency stop, a log
grid. Compare with your full brief and adjust names, floor count (`Logic/Building.cs`) or buttons where it differs.
Also known simplification: a hall call's Up/Down direction is logged but does not influence which elevator is chosen (only
estimated response time does, as the brief says).

## 10. Before you submit

1. In Workbench: **Server > Data Export**, export `elevator_db` as a self-contained file into `Database/` (so the zip contains the database and its data). Take an EER diagram screenshot (**Databa[...]
2. Delete the `bin` and `obj` folders inside `ElevatorControl` (Visual Studio recreates them).
3. Zip the folder as **`<yourStudentID>_AssignmentOne.zip`** and upload to *Assignment One C#* on BREO.
4. Fill in `TestReport_Template.md` with your own test results and screenshots (max 2000 words) and submit it too.
5. Run the program yourself, read through each file, and be ready to explain it: the Week 7 demonstration is mandatory and
   you will be asked to walk through the code. Also check your unit's rules on how AI-generated code may be used and
   declared.
