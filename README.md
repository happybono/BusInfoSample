# BusInfoSample
Bus Information System for Gyeonggi Province, South Korea. The estimated arrival time is calculated based on real-time bus location data retrieved from the server once every 20 seconds. The system displays refined information on the Windows Forms Project.

## Features
- Retrieve and display real-time bus arrival information.
- Display bus route names, destinations, and arrival times.
- Show remaining stops and congestion levels.
- Indicate low-floor buses for accessibility.
- Handle error messages and provide updates.

## Technologies Used
- C#
- .NET Framework
- Windows Forms

## Usage
### MainForm
The `MainForm` class is the main user interface that displays real-time bus arrival information. It consists of various labels and panels to show bus routes, arrival times, remaining stops, congestion levels, and error messages.

### Timers
Two timers are used in the application:
- _timer : Updates bus information every 20 seconds.
- _currentTimeTimer : Updates the current time every second.

### Controllers
`BusController` : Handles the logic for retrieving bus information and processing it for display.

### Models
`BusInfo` : Represents the information of a bus, including route name, destination, arrival times, stop counts, congestion levels, and error messages.

### Code Structure
`MainForm.cs` : Contains the logic for initializing timers, updating bus information, and displaying it on the form.
`BusController.cs` : Manages the retrieval and processing of bus information.
`BusInfo.cs` : Defines the data structure for bus information.

### Functions and Methods
#### MainForm.cs
- `MainForm()` : Constructor that initializes the form and timers.
- `MainForm_Load(object sender, EventArgs e)` : Asynchronously updates bus information when the form loads.
- `InitializeTimer()` : Initializes the timer to update bus information every 20 seconds.
- `InitializeCurrentTimeTimer()` : Initializes the timer to update the current time every second.
- `Timer_Tick(object sender, EventArgs e)` : Asynchronous method to update bus information at each timer tick.
- `UpdateBusInfo()` : Asynchronously retrieves bus information and displays it.
- `DisplayBusInfo(List<BusInfo> busInfoList)` : Displays the retrieved bus information on the form.
- `CurrentTimeTimer_Tick(object sender, EventArgs e)` : Updates the current time label every second.

#### BusController.cs
- `GetBusInfoAsync()`: Asynchronously retrieves bus information from the data source.
- `GetUrgentBusInfo(List<BusInfo> busInfoList)` : Retrieves urgent bus information that needs immediate attention.
- `GetCongestionText(int congestionLevel)` : Returns a textual representation of the congestion level.
- `GetLowPlateText(bool isLowPlate)` : Returns a textual representation of whether the bus is a low-floor bus.

#### BusInfo.cs
- `RouteName` : Name of the bus route.
- `DestName` : Destination of the bus route.
- `ArrivalTime1` : First arrival time of the bus.
- `ArrivalTime2` : Second arrival time of the bus.
- `StopCount1` : Remaining stops until the first arrival.
- `StopCount2` : Remaining stops until the second arrival.
- `Crowded1` : Congestion level for the first arrival.
- `Crowded2` : Congestion level for the second arrival.
- `LowPlate1` : Indicates if the first bus is a low-floor bus.
- `LowPlate2` : Indicates if the second bus is a low-floor bus.
- `ErrorMessage` : Error message if there is an issue with retrieving bus information.

#### License
This project is licensed under the MIT License. See the `LICENSE` file for details.
