using NL.HNOGames.Domoticz.Helpers;
using NL.HNOGames.Domoticz.Resources;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphPage : ContentPage
    {
        private Data.ConstantValues.GraphRange range;
        private PlotModel model;
        private Models.Device selectedDevice;
        private String type;
        private Random random = new Random();
        private List<Series> originalSeries;

        public GraphPage(Models.Device device,
            String sensor = "temp",
            Data.ConstantValues.GraphRange showRange = Data.ConstantValues.GraphRange.Day)
        {
            this.Title = device.Name;
            range = showRange;
            selectedDevice = device;
            type = sensor;
            InitializeComponent();
            initGraphData();
        }

        public async void initGraphData()
        {
            if (selectedDevice == null)
                return;

            try
            {
                var graphData = await App.ApiService.GetGraphData(selectedDevice.idx, type, range);

                InitModel();
                InitLegende();
                ProcessData(graphData);
                oGraphView.Model = model;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Process data to create the lines
        /// </summary>
        /// <param name="graphData"></param>
        private void ProcessData(Models.GraphModel graphData)
        {
            if (graphData != null && graphData.result != null && graphData.result.Length > 0)
            {
                var dateTimeList = graphData.result.Select(item => item.getDateTime()).ToList();

                var temperatureList = graphData.result.Select(item => item.getTemperature()).Where(item => item.HasValue).ToList();
                if (temperatureList != null && temperatureList.Count > 0)
                {
                    LineSeries line = CreateLine("Temperature", temperatureList, dateTimeList);
                    model.Series.Add(line);
                }

                var humidityList = graphData.result.Select(item => item.getHumidity()).Where(item => item.HasValue).ToList();
                if (humidityList != null && humidityList.Count > 0)
                {
                    LineSeries line = CreateLine("Humidity", humidityList, dateTimeList);
                    model.Series.Add(line);
                }

                var setPointList = graphData.result.Select(item => item.getSetPoint()).Where(item => item.HasValue).ToList();
                if (setPointList != null && setPointList.Count > 0)
                {
                    LineSeries line = CreateLine("SetPoint", setPointList, dateTimeList);
                    model.Series.Add(line);
                }

                var barometerList = graphData.result.Select(item => item.getBarometer()).Where(item => item.HasValue).ToList();
                if (barometerList != null && barometerList.Count > 0)
                {
                    LineSeries line = CreateLine("Barometer", barometerList, dateTimeList);
                    model.Series.Add(line);
                }

                var percentageList = graphData.result.Select(item => item.getValue()).Where(item => item.HasValue).ToList();
                if (percentageList != null && percentageList.Count > 0)
                {
                    LineSeries line = CreateLine("Percentage", percentageList, dateTimeList);
                    model.Series.Add(line);
                }

                var secondValueList = graphData.result.Select(item => item.getSecondValue()).Where(item => item.HasValue).ToList();
                if (secondValueList != null && secondValueList.Count > 0)
                {
                    LineSeries line = CreateLine("Second Percentage", secondValueList, dateTimeList);
                    model.Series.Add(line);
                }

                var PowerDeliveryList = graphData.result.Select(item => item.getPowerDelivery()).Where(item => item.HasValue).ToList();
                if (PowerDeliveryList != null && PowerDeliveryList.Count > 0)
                {
                    LineSeries line = CreateLine("Power Delivery", PowerDeliveryList, dateTimeList);
                    model.Series.Add(line);
                }

                var PowerUsageList = graphData.result.Select(item => item.getPowerUsage()).Where(item => item.HasValue).ToList();
                if (PowerUsageList != null && PowerUsageList.Count > 0)
                {
                    LineSeries line = CreateLine("Power Usage", PowerUsageList, dateTimeList);
                    model.Series.Add(line);
                }

                var CounterList = graphData.result.Select(item => item.getCounter()).Where(item => item.HasValue).ToList();
                if (CounterList != null && CounterList.Count > 0)
                {
                    LineSeries line = CreateLine("Counter", CounterList, dateTimeList);
                    model.Series.Add(line);
                }

                var SpeedList = graphData.result.Select(item => item.getSpeed()).Where(item => item.HasValue).ToList();
                if (SpeedList != null && SpeedList.Count > 0)
                {
                    LineSeries line = CreateLine("Speed", SpeedList, dateTimeList);
                    model.Series.Add(line);
                }

                var DirectionList = graphData.result.Select(item => item.getDirection()).Where(item => item.HasValue).ToList();
                if (DirectionList != null && DirectionList.Count > 0)
                {
                    LineSeries line = CreateLine("Direction", DirectionList, dateTimeList);
                    model.Series.Add(line);
                }

                var SunPowerList = graphData.result.Select(item => item.getSunPower()).Where(item => item.HasValue).ToList();
                if (SunPowerList != null && SunPowerList.Count > 0)
                {
                    LineSeries line = CreateLine("SunPower", SunPowerList, dateTimeList);
                    model.Series.Add(line);
                }

                var UsageList = graphData.result.Select(item => item.getUsage()).Where(item => item.HasValue).ToList();
                if (UsageList != null && UsageList.Count > 0)
                {
                    LineSeries line = CreateLine("Usage", UsageList, dateTimeList);
                    model.Series.Add(line);
                }

                var RainList = graphData.result.Select(item => item.getRain()).Where(item => item.HasValue).ToList();
                if (RainList != null && RainList.Count > 0)
                {
                    LineSeries line = CreateLine("Rain", RainList, dateTimeList);
                    model.Series.Add(line);
                }

                var Co2List = graphData.result.Select(item => item.getCo2()).Where(item => item.HasValue).ToList();
                if (Co2List != null && Co2List.Count > 0)
                {
                    LineSeries line = CreateLine("Co2", Co2List, dateTimeList);
                    model.Series.Add(line);
                }

                var Co2MinList = graphData.result.Select(item => item.getCo2Min()).Where(item => item.HasValue).ToList();
                if (Co2MinList != null && Co2MinList.Count > 0)
                {
                    LineSeries line = CreateLine("Min Co2", Co2MinList, dateTimeList);
                    model.Series.Add(line);
                }

                var Co2MaxList = graphData.result.Select(item => item.getCo2Max()).Where(item => item.HasValue).ToList();
                if (Co2MaxList != null && Co2MaxList.Count > 0)
                {
                    LineSeries line = CreateLine("Max Co2", Co2MaxList, dateTimeList);
                    model.Series.Add(line);
                }

                if (graphData.result.Any(item => item.hasTemperatureRange()))
                {
                    var TemperatureMinList = graphData.result.Select(item => item.getTemperatureMin()).Where(item => item.HasValue).ToList();
                    if (TemperatureMinList != null && TemperatureMinList.Count > 0)
                    {
                        LineSeries line = CreateLine("Min Temperature", TemperatureMinList, dateTimeList);
                        model.Series.Add(line);
                    }

                    var TemperatureMaxList = graphData.result.Select(item => item.getTemperatureMax()).Where(item => item.HasValue).ToList();
                    if (TemperatureMaxList != null && TemperatureMaxList.Count > 0)
                    {
                        LineSeries line = CreateLine("Max Temperature", TemperatureMaxList, dateTimeList);
                        model.Series.Add(line);
                    }
                }
            }
        }

        /// <summary>
        /// Create Graph Line
        /// </summary>
        private LineSeries CreateLine(String title, List<double?> graphData, List<DateTime?> graphDateTime)
        {
            LineSeries lineTemperature = CreateLine(title);

            int counter = 0;
            foreach (var data in graphData)
            {
                if (data.HasValue)
                    lineTemperature.Points.Add(new DataPoint(DateTimeAxis.ToDouble(graphDateTime[counter].Value), data.Value));
                counter++;
            }

            return lineTemperature;
        }

        /// <summary>
        /// Create Graph Line
        /// </summary>
        private LineSeries CreateLine(String title, List<float?> graphData, List<DateTime?> graphDateTime)
        {
            LineSeries lineTemperature = CreateLine(title);
            int counter = 0;
            foreach (var data in graphData)
            {
                if (data.HasValue)
                    lineTemperature.Points.Add(new DataPoint(DateTimeAxis.ToDouble(graphDateTime[counter].Value), Convert.ToDouble(data)));
                counter++;
            }

            return lineTemperature;
        }

        /// <summary>
        /// Create Graph Line
        /// </summary>
        private LineSeries CreateLine(String title, List<int> graphData, List<DateTime?> graphDateTime)
        {
            LineSeries lineTemperature = CreateLine(title);
            int counter = 0;
            foreach (var data in graphData)
            {
                lineTemperature.Points.Add(new DataPoint(DateTimeAxis.ToDouble(graphDateTime[counter].Value), Convert.ToDouble(data)));
                counter++;
            }

            return lineTemperature;
        }

        /// <summary>
        /// Init line object with values
        /// </summary>
        private LineSeries CreateLine(String title)
        {
            String lineColor = String.Format("#{0:X6}", random.Next(0x1000000));
            return new LineSeries
            {
                StrokeThickness = 1,
                MarkerSize = 2,
                MarkerStroke = OxyColor.Parse(lineColor),
                MarkerType = MarkerType.Circle,
                CanTrackerInterpolatePoints = true,
                Title = title,
                Smooth = false,
                Color = OxyColor.Parse(lineColor),
                Selectable = true,
                SelectionMode = SelectionMode.All,
                TrackerFormatString = "",
            };
        }

        /// <summary>
        /// create model object
        /// </summary>
        private void InitModel()
        {
            model = new PlotModel();
            String format = "HH:ss";
            if (this.range == Data.ConstantValues.GraphRange.Month)
                format = "dd/MM";
            else if (this.range == Data.ConstantValues.GraphRange.Year)
                format = "MM/yyyy";

            model.TextColor = OxyColor.Parse("#757575");
            model.Axes.Add(new DateTimeAxis { Title = "DateTime", Position = AxisPosition.Bottom, StringFormat = format });
            model.Axes.Add(new LinearAxis { Title = "Value", Position = AxisPosition.Left });
            model.Annotations.Add(new LineAnnotation()
            {
                Type = LineAnnotationType.Vertical,
                Color = OxyColors.Green,
                ClipByYAxis = false,
            });

        }

        /// <summary>
        /// setup legende
        /// </summary>
        private void InitLegende()
        {
            model.LegendTitle = "Legende";
            model.LegendOrientation = LegendOrientation.Horizontal;
            model.LegendPlacement = LegendPlacement.Outside;
            model.LegendPosition = LegendPosition.BottomCenter;
            if (App.AppSettings.DarkTheme)
                model.LegendBackground = OxyColor.FromAColor(200, OxyColors.Black);
            else
                model.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            model.LegendBorder = OxyColors.Black;
        }

        /// <summary>
        /// Filter the graph
        /// </summary>
        public async Task FilterAsync()
        {
            RevertOriginalSource();

            List<string> actions = CreateFilterMenu();
            if (actions.Count > 0)
            {
                var result = await this.DisplayActionSheet(AppResources.filter, AppResources.cancel, null, actions.ToArray());
                if (String.Compare(AppResources.filterOn_all, result, StringComparison.OrdinalIgnoreCase) == 0)
                    RevertOriginalSource();
                else if (String.Compare(AppResources.cancel, result, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    BackupSource();
                    model.Series.Clear();//remove list
                    foreach (Series serie in originalSeries)
                    {
                        if (String.Compare(serie.Title, result, StringComparison.OrdinalIgnoreCase) == 0)
                            model.Series.Add(serie);
                    }
                }

                oGraphView.Model = model;
                oGraphView.Model.InvalidatePlot(true);
            }
        }

        /// <summary>
        /// Backup the original graph source
        /// </summary>
        private void BackupSource()
        {
            originalSeries = new List<Series>();
            foreach (Series s in model.Series)
                originalSeries.Add(s);//backup
        }

        /// <summary>
        /// Revert to saved original source
        /// </summary>
        private void RevertOriginalSource()
        {
            if (originalSeries != null)
            {
                model.Series.Clear();//revert original
                foreach (Series s in originalSeries)
                    model.Series.Add(s);
                originalSeries = null;
            }
        }

        /// <summary>
        /// Create Filter menu
        /// </summary>
        private List<string> CreateFilterMenu()
        {
            List<string> actions = new List<string>();
            if (model != null)
            {
                actions.Add(AppResources.filterOn_all);
                foreach (Series serie in model.Series)
                    actions.Add(serie.Title);
            }

            return actions;
        }
    }
}