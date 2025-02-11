# BusInfoSample
This project is designed to display bus information, including arrival times, route names, destination names, congestion levels, and error messages. The project uses a list of `BusInfo` objects to populate various labels on the user interface. The information is categorized and displayed based on urgency and availability of arrival times.

<div align="center">
<img alt="GitHub Last Commit" src="https://img.shields.io/github/last-commit/happybono/BusInfoSample"> 
<img alt="GitHub Repo Size" src="https://img.shields.io/github/repo-size/happybono/BusInfoSample">
<img alt="GitHub Repo Languages" src="https://img.shields.io/github/languages/count/happybono/BusInfoSample">
<img alt="GitHub Top Languages" src="https://img.shields.io/github/languages/top/HappyBono/BusInfoSample">
</div>

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

## Functions
### `DisplayBusInfo(List<BusInfo> busInfoList)`
This function displays bus information on the user interface. It categorizes the bus information into urgent and non-urgent lists and updates various labels based on the available data.

### `GetUrgentBusInfo(List<BusInfo> busInfoList)`
This function filters and sorts the bus information to return a list of urgent bus information. A bus is considered urgent if its arrival time is less than 2 minutes.

### `GetCongestionText(string crowded)`
This function returns the congestion status text based on the input value:
- "2" returns "혼잡" (congested)
- "1" returns "여유" (spacious)
- Any other value returns "보통" (normal)

### `GetLowPlateText(string lowPlate)`
This function returns the type of bus based on the input value:
- "0" returns "일반" (regular)
- "1" returns "저상" (low-floor)
- Any other value returns an empty string

### `ParseBusInfo(string xmlData, int index)`
This function parses the XML data from the bus arrival API and returns a `BusInfo` object with the relevant information.

### `InitializeTimer()`
This function initializes a timer that triggers every 20 seconds to update the bus information.

### `UpdateBusInfo()`
This asynchronous function fetches bus information from the API and updates the user interface.

### `Timer_Tick(object sender, EventArgs e)`
This function is triggered by the timer to call the `UpdateBusInfo()` function.

### `MainForm_Load(object sender, EventArgs e)`
This function is called when the main form loads and initiates the first update of the bus information.

### `labelSecondStopCount1_Click(object sender, EventArgs e)`
This function handles click events for the second stop count label. (Currently, it is empty and can be customized as needed.)

### `timer2_Tick(object sender, EventArgs e)`
This function updates the current time label with the short time format of the current date and time.

## BusInfo Class
The `BusInfo` class contains the following properties:
- `string RouteName`
- `int? ArrivalTime1`
- `int? ArrivalTime2`
- `string DestName`
- `string StopCount1`
- `string StopCount2`
- `string Crowded1`
- `string Crowded2`
- `string LowPlate1`
- `string LowPlate2`
- `string ErrorMessage`

These properties store information about the bus route, arrival times, destination, stop counts, congestion levels, bus type, and any error messages.

## Usage
In order to use this project, follow these steps:
1. Create instances of the `BusInfo` class with the relevant bus information.
2. Populate a `List<BusInfo>` with the `BusInfo` instances.
3. Call the `DisplayBusInfo(List<BusInfo> busInfoList)` function with the populated list to display the bus information on the user interface.

The labels and panels in the code should be replaced with actual controls from your user interface design.
This project can be extended and customized to fit specific requirements, such as adding more detailed bus information, enhancing the user interface, or integrating with a real-time bus information API.

## Example
```csharp
List<BusInfo> busInfoList = new List<BusInfo>
{
    new BusInfo
    {
        RouteName = "101",
        ArrivalTime1 = 5,
        ArrivalTime2 = 15,
        DestName = "Central Station",
        StopCount1 = "3",
        StopCount2 = "7",
        Crowded1 = "1",
        Crowded2 = "2",
        LowPlate1 = "0",
        LowPlate2 = "1",
        ErrorMessage = ""
    },
    // Add more BusInfo instances as needed
};

DisplayBusInfo(busInfoList);
```

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

## Copyright 
Copyright ⓒ HappyBono 2021 - 2025. All rights Reserved.
