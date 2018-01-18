# Windows Built-in
- LogMan
- TraceRpt
- EventVwr
- WEvtUtil
- WPR

# SDK
- WPRUI
- WPA
- TraceLog
- TraceFmt

# Other
- PerfView
- Microsoft Message Analyzer (MMA)
- Visual Studio Diagnostic Events Viewer (VS)

# PowerShell
- EventTracingManagement
- Desired State Configuration ?

# Library
- Microsoft.Diagnostics.Tracing.TraceEvent
- ETW2JSON
- Tx (LINQ to Events)
- KrabsETW

# Features of event viewer tools
| Tool     | GUI | File | Live | TraceLogging | EventSource |
|----------|:---:|:----:|:----:|:------------:|:-----------:|
| TraceRpt | [ ] | [x]  | [x]  | [x]          | [ ]         |
| TraceFmt | [ ] | [x]  | [x]  | [x]          | [ ]         |
| EventVwr | [x] | [x]  | [ ]  | [ ]          | [ ]         |
| WPA      | [x] | [x]  | [ ]  | [x]          | [x]         |
| PerfView | [x] | [x]  | [ ]  | [x]          | [x]         |
| MMA      | [x] | [x]  | [x]  | [x]          | [ ]         |
| VS       | [x] | [ ]  | [x]  | [x]          | [x]         |

- File: can read etl files.
- Live: real-time trace session support.
- TraceLogging: can parse TraceLogging events.
- EventSource: can parse .NET EventSource events with dynamic ETW manifest event.

# What is missing?
- A simple but powerful live event trace viewer that supports the .NET EventSource.
- A simple installer that supports permanent logging of a .NET EventSource to a file.
