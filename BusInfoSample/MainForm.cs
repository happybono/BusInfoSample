using BusInfoSample.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BusInfoSample
{
    public partial class MainForm : Form
    {
        private Timer _timer;
        private List<string> routeIds;
        private List<string> bstopIds;
        private List<Label> routeLabels;
        private List<Label> destLabels;
        private List<Label> arrivalTimeLabels;
        private List<Label> stopCountLabels;
        private List<Label> secondArrivalTimeLabels;
        private List<Label> secondStopCountLabels;
        private List<Label> congestionLabels;
        private List<Label> urgentCongestionLabels;
        private List<Label> secondCongestionLabels;
        private List<Label> lowPlateLabels;
        private List<Label> secondLowPlateLabels;
        private List<Label> urgentRouteLabels;
        private List<Label> timeDiffLabels;
        private List<Label> errorLabels;
        private List<Panel> errorPanels;
        private List<Panel> secondBusPanels;

        public MainForm()
        {
            InitializeComponent();
            InitializeTimer();

            // Initialize route and stop IDs
            routeIds = new List<string> { "224000014", "216000011", "216000004" };
            bstopIds = new List<string> { "224000719", "224000719", "224000719" };

            // Initialize labels for displaying bus route, arrival time, remaining stops, and congestion level
            // Initialize labels for displaying bus route, arrival time, remaining stops, and congestion level
            routeLabels = new List<Label> { labelBusRoute1, labelBusRoute2, labelBusRoute3 };
            arrivalTimeLabels = new List<Label> { labelArrivalTime1, labelArrivalTime2, labelArrivalTime3 };
            stopCountLabels = new List<Label> { labelStopCount1, labelStopCount2, labelStopCount3 };
            secondArrivalTimeLabels = new List<Label> { labelSecondArrivalTime1, labelSecondArrivalTime2, labelSecondArrivalTime3 };
            secondStopCountLabels = new List<Label> { labelSecondStopCount1, labelSecondStopCount2, labelSecondStopCount3 };
            congestionLabels = new List<Label> { labelCongestion1, labelCongestion2, labelCongestion3 };
            secondCongestionLabels = new List<Label> { labelSecondCongestion1, labelSecondCongestion2, labelSecondCongestion3 };
            destLabels = new List<Label> { labelRouteDest1, labelRouteDest2, labelRouteDest3 };
            lowPlateLabels = new List<Label> { labelLowPlate1, labelLowPlate2, labelLowPlate3 };
            secondLowPlateLabels = new List<Label> { labelSecondLowPlate1, labelSecondLowPlate2, labelSecondLowPlate3 };
            secondBusPanels = new List<Panel> { panelSecondBus1, panelSecondBus2, panelSecondBus3 };

            urgentRouteLabels = new List<Label> { labelUrgentRoute1, labelUrgentRoute2, labelUrgentRoute3 };
            urgentCongestionLabels = new List<Label> { labelUrgentCongestion1, labelUrgentCongestion2, labelUrgentCongestion3 };
            timeDiffLabels = new List<Label> { labelTimeDiff1, labelTimeDiff2, labelTimeDiff3 };

            errorLabels = new List<Label> { labelError1, labelError2, labelError3 };
            errorPanels = new List<Panel> { panelError1, panelError2, panelError3 };
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            // Update the bus arrival information when the form loads
            await UpdateBusInfo();
        }

        private void InitializeTimer()
        {
            _timer = new Timer();
            _timer.Interval = 20000; // 20 seconds in milliseconds
            _timer.Tick += async (s, e) => await Timer_Tick(s, e);
            _timer.Start();
        }

        private async Task Timer_Tick(object sender, EventArgs e)
        {
            await UpdateBusInfo();
        }

        private async Task UpdateBusInfo()
        {
            string serviceKey = Resources.ServiceKey.ToString();
            List<BusInfo> busInfoList = new List<BusInfo>();

            for (int i = 0; i < routeIds.Count; i++)
            {
                string routeId = routeIds[i];
                string bstopId = bstopIds[i];
                string apiUrl = $"https://apis.data.go.kr/6410000/busarrivalservice/v2/getBusArrivalItemv2?serviceKey={serviceKey}&routeId={routeId}&stationId={bstopId}&format=xml";

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(apiUrl);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        var busInfo = ParseBusInfo(responseBody, i);
                        busInfoList.Add(busInfo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching bus information: " + ex.Message);
                }
            }

            DisplayBusInfo(busInfoList);
        }

        private BusInfo ParseBusInfo(string xmlData, int index)
        {
            XElement xml = XElement.Parse(xmlData);

            var arrivalTime1 = xml.Descendants("predictTime1").FirstOrDefault()?.Value;
            var stopCount1 = xml.Descendants("locationNo1").FirstOrDefault()?.Value;
            var crowded1 = xml.Descendants("crowded1").FirstOrDefault()?.Value;
            var arrivalTime2 = xml.Descendants("predictTime2").FirstOrDefault()?.Value;
            var stopCount2 = xml.Descendants("locationNo2").FirstOrDefault()?.Value;
            var crowded2 = xml.Descendants("crowded2").FirstOrDefault()?.Value;
            var routeName = xml.Descendants("routeName").FirstOrDefault()?.Value;
            var destName = xml.Descendants("routeDestName").FirstOrDefault()?.Value;
            var LowPlate1 = xml.Descendants("lowPlate1").FirstOrDefault()?.Value;
            var LowPlate2 = xml.Descendants("lowPlate2").FirstOrDefault()?.Value;

            return new BusInfo
            {
                RouteName = routeName,
                DestName = destName,
                ArrivalTime1 = string.IsNullOrEmpty(arrivalTime1) ? (int?)null : int.Parse(arrivalTime1),
                ArrivalTime2 = string.IsNullOrEmpty(arrivalTime2) ? (int?)null : int.Parse(arrivalTime2),
                StopCount1 = stopCount1,
                StopCount2 = stopCount2,
                Crowded1 = crowded1,
                Crowded2 = crowded2,
                LowPlate1 = LowPlate1,
                LowPlate2 = LowPlate2
            };
        }

        private void DisplayBusInfo(List<BusInfo> busInfoList)
        {
            var urgentBusInfoList = GetUrgentBusInfo(busInfoList);

            for (int i = 0; i < urgentRouteLabels.Count; i++)
            {
                if (i < urgentBusInfoList.Count)
                {
                    var busInfo = urgentBusInfoList[i];
                    urgentRouteLabels[i].Text = busInfo.RouteName;
                    urgentCongestionLabels[i].Text = GetCongestionText(busInfo.Crowded1);

                    if (i < timeDiffLabels.Count && busInfo.ArrivalTime2.HasValue)
                    {
                        int timeDiff = busInfo.ArrivalTime2.Value - busInfo.ArrivalTime1.Value;
                        timeDiffLabels[i].Text = "다음 " + timeDiff + " 분 후 도착";
                    }
                    else
                    {
                        timeDiffLabels[i].Text = "";
                    }
                }
                else
                {
                    urgentRouteLabels[i].Text = "";
                    if (i < timeDiffLabels.Count)
                    {
                        timeDiffLabels[i].Text = "";
                        urgentCongestionLabels[i].Text = "";
                    }
                }
            }

            for (int i = 0; i < busInfoList.Count; i++)
            {
                var busInfo = busInfoList[i];

                if (i < errorPanels.Count)
                {
                    if (!string.IsNullOrEmpty(busInfo.ErrorMessage))
                    {
                        errorPanels[i].Visible = true;
                        errorLabels[i].Text = busInfo.ErrorMessage;
                    }
                    else
                    {
                        errorPanels[i].Visible = false;
                        errorLabels[i].Text = "";
                    }
                }

                if (busInfo.ArrivalTime1.HasValue)
                {
                    if (i < routeLabels.Count) routeLabels[i].Text = busInfo.RouteName;
                    if (i < destLabels.Count) destLabels[i].Text = busInfo.DestName + "행";
                    if (i < arrivalTimeLabels.Count) arrivalTimeLabels[i].Text = busInfo.ArrivalTime1.HasValue ? busInfo.ArrivalTime1.Value + " 분" : string.Empty;
                    if (i < stopCountLabels.Count) stopCountLabels[i].Text = busInfo.StopCount1 + " 남음";
                    if (i < congestionLabels.Count) congestionLabels[i].Text = GetCongestionText(busInfo.Crowded1);
                    if (i < lowPlateLabels.Count) lowPlateLabels[i].Text = GetLowPlateText(busInfo.LowPlate1);
                }
                else
                {
                    errorPanels[i].Visible = true;
                    errorLabels[i].Text = "";
                    if (i < arrivalTimeLabels.Count) arrivalTimeLabels[i].Text = string.Empty;
                    if (i < stopCountLabels.Count) stopCountLabels[i].Text = string.Empty;
                    if (i < congestionLabels.Count) congestionLabels[i].Text = string.Empty;
                    if (i < lowPlateLabels.Count) lowPlateLabels[i].Text = string.Empty;
                }

                if (busInfo.ArrivalTime2.HasValue)
                {
                    secondBusPanels[i].Visible = false;
                    if (i < secondArrivalTimeLabels.Count) secondArrivalTimeLabels[i].Text = busInfo.ArrivalTime2.Value + " 분";
                    if (i < secondStopCountLabels.Count) secondStopCountLabels[i].Text = busInfo.StopCount2 + " 남음";
                    if (i < secondCongestionLabels.Count) secondCongestionLabels[i].Text = GetCongestionText(busInfo.Crowded2);
                    if (i < secondLowPlateLabels.Count) secondLowPlateLabels[i].Text = GetLowPlateText(busInfo.LowPlate2);
                }
                else
                {
                    secondBusPanels[i].Visible = true;
                    if (i < secondArrivalTimeLabels.Count) secondArrivalTimeLabels[i].Text = string.Empty;
                    if (i < secondStopCountLabels.Count) secondStopCountLabels[i].Text = string.Empty;
                    if (i < secondCongestionLabels.Count) secondCongestionLabels[i].Text = string.Empty;
                    if (i < lowPlateLabels.Count) secondLowPlateLabels[i].Text = string.Empty;
                }
            }
        }


        private string GetCongestionText(string crowded)
        {
            return crowded == "2" ? "혼잡" : crowded == "1" ? "여유" : "보통";
        }

        private void labelSecondStopCount1_Click(object sender, EventArgs e)
        {

        }


        public List<BusInfo> GetUrgentBusInfo(List<BusInfo> busInfoList)
        {
            var urgentBusInfoList = busInfoList
                .Where(busInfo => busInfo.ArrivalTime1.HasValue && busInfo.ArrivalTime1.Value < 2)
                .OrderBy(busInfo => busInfo.ArrivalTime1.Value)
                .ToList();

            return urgentBusInfoList;
        }

        public string GetLowPlateText(string lowPlate)
        {
            return lowPlate == "0" ? "일반" : lowPlate == "1" ? "저상" : "";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            labelCurTime.Text = DateTime.Now.ToShortTimeString();
        }
    }

    public class BusInfo
    {
        public string RouteName { get; set; }
        public int? ArrivalTime1 { get; set; }
        public int? ArrivalTime2 { get; set; }
        public string DestName { get; set; }
        public string StopCount1 { get; set; }
        public string StopCount2 { get; set; }
        public string Crowded1 { get; set; }
        public string Crowded2 { get; set; }
        public string LowPlate1 { get; set; }
        public string LowPlate2 { get; set; }

        public string ErrorMessage { get; set; }
    }
}
