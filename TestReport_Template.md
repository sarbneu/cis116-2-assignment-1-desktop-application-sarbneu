# Testing Report - Assignment 1: Control an Elevator

**Student ID:** ____________  **Unit:** CIS016-2  **Date:** ____________
*(Limit: 2000 words. Replace every ____ with your own results and add screenshots.)*

## 1. Introduction
Short description of what was built and the test approach (manual black-box testing of the GUI, database checks in MySQL Workbench, and code review).

## 2. Test environment
| Item | Value |
|---|---|
| OS | |
| Visual Studio version | |
| .NET target | net8.0-windows |
| MySQL Server version | |
| MySql.Data version | 8.4.0 |

## 3. Test cases

Fill in *Actual result* and *Pass/Fail* after running each test and note the screenshot number.

| ID | Task | Steps | Expected result | Actual result | Pass/Fail | Screenshot |
|---|---|---|---|---|---|---|
| T1.1 | 1 | Start the program | Window shows building view, hall call buttons, one car panel per elevator, log grid | | | |
| T1.2 | 1 | Look at the hall panel | Ground floor has only an Up button, top floor only a Down button | | | |
| T2.1 | 2 | Press car button for floor 3 in Elevator A | Button lights, A moves up to 3, button clears on arrival, doors open then close, A goes idle | | | |
| T2.2 | 2 | Press car buttons 4 and 2 while A is idle at ground | A stops at 2 first, then 4 (single sweep) | | | |
| T2.3 | 2 | While A is moving up, press a lower floor | Lower floor is served after the upward stops | | | |
| T2.4 | 2 | Press Open door while idle | Doors open for the normal period and close | | | |
| T2.5 | 2 | Press Open door while moving | Refused, message in status bar and log | | | |
| T2.6 | 2 | Press Close door while doors are open | Doors close immediately | | | |
| T2.7 | 2 | Press the car button for the current floor | Doors open (or stay open longer), no movement | | | |
| T2.8 | 2 | Press Emergency stop while moving | Car freezes (red); floor buttons are refused | | | |
| T2.9 | 2 | Press Resume | Car continues to its remaining stops | | | |
| T3.1 | 3 | Start with MySQL running | Status shows connected; log grid shows rows | | | |
| T3.2 | 3 | Generate events, press Refresh log | New rows appear in grid and in Workbench (`SELECT`) | | | |
| T3.3 | 3 | Stop MySQL service, start program | Warning message; elevators still work; no crash | | | |
| T3.4 | 3 | Restart MySQL, trigger an event | Queued entries are saved | | | |
| T3.5 | 3 | Copy the built program folder to another location | Program still finds `appsettings.json` (relative path) | | | |
| T3.6 | 3 | Rename `appsettings.json`, start | Message about default settings; no crash | | | |
| T4.1 | 4 | Send an elevator several floors | Car moves smoothly, doors visibly open/close, colour changes by state | | | |
| T5.1 | 5.1 | Click buttons rapidly while logging | GUI stays responsive while rows are written | | | |
| T5.2a | 5.2 | Both elevators at ground, hall call at floor 2 | Elevator A takes it (tie) | | | |
| T5.2b | 5.2 | Then a hall call at floor 3 | Elevator B takes it (shorter estimate) | | | |
| T5.2c | 5.2 | Elevator A busy going to 4, hall call at 1 | The closer / quicker elevator answers; A's queue is not lost | | | |
| T5.2d | 5.2 | Car-panel request in B | Only B serves it | | | |
| T5.2e | 5.2 | Emergency stop A, hall call | B answers; with both stopped the call is rejected and logged | | | |

## 4. Database evidence
Screenshots: Workbench table structure / EER diagram, `SELECT` showing logged rows, program log grid.

## 5. Defects found and fixed
| Defect | Cause | Fix |
|---|---|---|
| | | |

## 6. Marking Matrix with Self-Assessment

| Task | Sub-task | Possible | Self-assessment (Yes/No) | Reference to testing report | Mark awarded |
|---|---|---|---|---|---|
| 1 | Complete GUI for Task 1 | 10 | | T1.1, T1.2 | |
| 1 | Event handler skeleton implemented for all GUI buttons | 10 | | T1.1 | |
| 2 | All event handlers function as specified | 10 | | T2.1 - T2.9 | |
| 3 | Database designed and successfully connected | 5 | | T3.1, section 4 | |
| 3 | Log information retrieved from DB and displayed in GUI | 5 | | T3.1, T3.2 | |
| 3 | Log information stored in DB | 5 | | T3.2, T3.4 | |
| 3 | Disconnected model used (DataAdapter.Update()) | 5 | | code: `LogRepository.SaveEntries` | |
| 3 | Relative paths used; logical errors eliminated; exceptions handled | 5 | | T3.3, T3.5, T3.6 | |
| 3 | No duplication in DB-related event handlers | 5 | | code: `Guard`, `DbJob<T>` | |
| 4 | Events from Task 2 animated using delegation and timer | 10 | | T4.1 | |
| 5 | Efficiency improved with BackgroundWorker | 5 | | T5.1 | |
| 5 | Two elevators; State and Observer patterns | 15 | | T5.2a - T5.2e | |
| 6 | Testing report includes all expected content | 10 | | this document | |
| | **Total** | **100** | | | |

## 7. Conclusion and known limitations
Be honest here, for example: the hall call direction does not influence the choice of elevator; door animation is two-state (open/closed).
