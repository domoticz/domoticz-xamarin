using NL.HNOGames.Domoticz.Data;
using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Plugin.DeviceOrientation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace NL.HNOGames.Domoticz.Views
{
    /// <summary>
    /// Defines the <see cref="GraphPage" />
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphPage
    {
        #region Variables

        /// <summary>
        /// Defines the _range
        /// </summary>
        private readonly ConstantValues.GraphRange _range;

        /// <summary>
        /// Defines the _model
        /// </summary>
        private PlotModel _model;

        /// <summary>
        /// Defines the _selectedDevice
        /// </summary>
        private readonly Device _selectedDevice;

        /// <summary>
        /// Defines the _type
        /// </summary>
        private readonly string _type;

        /// <summary>
        /// Defines the _random
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Defines the _originalSeries
        /// </summary>
        private List<Series> _originalSeries;

        /// <summary>
        /// Defines the activeFilter
        /// </summary>
        private String activeFilter = AppResources.filterOn_all;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphPage"/> class.
        /// </summary>
        /// <param name="device">The device<see cref="Device"/></param>
        /// <param name="sensor">The sensor<see cref="string"/></param>
        /// <param name="showRange">The showRange<see cref="ConstantValues.GraphRange"/></param>
        public GraphPage(Device device,
            string sensor = "temp",
            ConstantValues.GraphRange showRange = ConstantValues.GraphRange.Day)
        {
            Title = device.Name;
            _range = showRange;
            _selectedDevice = device;
            _type = sensor;
            InitializeComponent();
            InitGraphData();

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                CrossDeviceOrientation.Current.OrientationChanged += OrientationChanged;
        }

        #endregion

        #region Public

        /// <summary>
        /// Init graph objects/views
        /// </summary>
        public async void InitGraphData()
        {
            if (_selectedDevice == null)
                return;

            try
            {
                var graphData = await App.ApiService.GetGraphData(_selectedDevice.idx, _type, _range);

                InitModel();
                InitLegende();
                ProcessData(graphData);
                FilterGraphResult();
                oGraphView.Model = _model;
            }
            catch (Exception ex)
            {
                App.AddLog(ex.Message);
            }
        }

        /// <summary>
        /// Filter the graph
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task FilterAsync()
        {
            RevertOriginalSource();
            var actions = CreateFilterMenu();
            if (actions.Count > 0)
            {
                activeFilter = await DisplayActionSheet(AppResources.filter, AppResources.cancel, null,
                    actions.ToArray());
                FilterGraphResult();
                oGraphView.Model = _model;
                oGraphView.Model.InvalidatePlot(true);
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// The OrientationChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="Plugin.DeviceOrientation.Abstractions.OrientationChangedEventArgs"/></param>
        private void OrientationChanged(object sender, Plugin.DeviceOrientation.Abstractions.OrientationChangedEventArgs e)
        {
            //refresh graph
            InitGraphData();
        }

        /// <summary>
        /// Process data to create the lines
        /// </summary>
        /// <param name="graphData"></param>
        private void ProcessData(GraphModel graphData)
        {
            if (graphData?.result == null || graphData.result.Length <= 0) return;
            var dateTimeList = graphData.result.Select(item => item.getDateTime()).ToList();

            var temperatureList = graphData.result.Select(item => item.getTemperature()).Where(item => item.HasValue)
                .ToList();
            if (temperatureList != null && temperatureList.Count > 0)
            {
                var line = CreateLine("Temperature", temperatureList, dateTimeList);
                _model.Series.Add(line);
            }

            var humidityList = graphData.result.Select(item => item.getHumidity()).Where(item => item.HasValue)
                .ToList();
            if (humidityList != null && humidityList.Count > 0)
            {
                var line = CreateLine("Humidity", humidityList, dateTimeList);
                _model.Series.Add(line);
            }

            var setPointList = graphData.result.Select(item => item.getSetPoint()).Where(item => item.HasValue)
                .ToList();
            if (setPointList != null && setPointList.Count > 0)
            {
                var line = CreateLine("SetPoint", setPointList, dateTimeList);
                _model.Series.Add(line);
            }

            var barometerList = graphData.result.Select(item => item.getBarometer()).Where(item => item.HasValue)
                .ToList();
            if (barometerList != null && barometerList.Count > 0)
            {
                var line = CreateLine("Barometer", barometerList, dateTimeList);
                _model.Series.Add(line);
            }

            var percentageList = graphData.result.Select(item => item.getValue()).Where(item => item.HasValue).ToList();
            if (percentageList != null && percentageList.Count > 0)
            {
                var line = CreateLine("Percentage", percentageList, dateTimeList);
                _model.Series.Add(line);
            }

            var secondValueList = graphData.result.Select(item => item.getSecondValue()).Where(item => item.HasValue)
                .ToList();
            if (secondValueList != null && secondValueList.Count > 0)
            {
                var line = CreateLine("Second Percentage", secondValueList, dateTimeList);
                _model.Series.Add(line);
            }

            var PercentageMinList = graphData.result.Select(item => item.getValueMin()).Where(item => item.HasValue).ToList();
            if (PercentageMinList != null && PercentageMinList.Count > 0)
            {
                var line = CreateLine("Min Percentage", PercentageMinList, dateTimeList);
                _model.Series.Add(line);
            }

            var PercentageMaxList = graphData.result.Select(item => item.getValueMax()).Where(item => item.HasValue).ToList();
            if (PercentageMaxList != null && PercentageMaxList.Count > 0)
            {
                var line = CreateLine("Max Percentage", PercentageMaxList, dateTimeList);
                _model.Series.Add(line);
            }

            var PercentageAvgList = graphData.result.Select(item => item.getValueAvg()).Where(item => item.HasValue).ToList();
            if (PercentageAvgList != null && PercentageAvgList.Count > 0)
            {
                var line = CreateLine("Avg Percentage", PercentageAvgList, dateTimeList);
                _model.Series.Add(line);
            }

            var powerDeliveryList = graphData.result.Select(item => item.getPowerDelivery())
                .Where(item => item.HasValue).ToList();
            if (powerDeliveryList != null && powerDeliveryList.Count > 0)
            {
                var line = CreateLine("Power Delivery", powerDeliveryList, dateTimeList);
                _model.Series.Add(line);
            }

            var powerUsageList = graphData.result.Select(item => item.getPowerUsage()).Where(item => item.HasValue)
                .ToList();
            if (powerUsageList != null && powerUsageList.Count > 0)
            {
                var line = CreateLine("Power Usage", powerUsageList, dateTimeList);
                _model.Series.Add(line);
            }

            var counterList = graphData.result.Select(item => item.getCounter()).Where(item => item.HasValue).ToList();
            if (counterList != null && counterList.Count > 0)
            {
                var line = CreateLine("Counter", counterList, dateTimeList);
                _model.Series.Add(line);
            }

            var speedList = graphData.result.Select(item => item.getSpeed()).Where(item => item.HasValue).ToList();
            if (speedList != null && speedList.Count > 0)
            {
                var line = CreateLine("Speed", speedList, dateTimeList);
                _model.Series.Add(line);
            }

            var directionList = graphData.result.Select(item => item.getDirection()).Where(item => item.HasValue)
                .ToList();
            if (directionList != null && directionList.Count > 0)
            {
                var line = CreateLine("Direction", directionList, dateTimeList);
                _model.Series.Add(line);
            }

            var sunPowerList = graphData.result.Select(item => item.getSunPower()).Where(item => item.HasValue)
                .ToList();
            if (sunPowerList != null && sunPowerList.Count > 0)
            {
                var line = CreateLine("SunPower", sunPowerList, dateTimeList);
                _model.Series.Add(line);
            }

            var usageList = graphData.result.Select(item => item.getUsage()).Where(item => item.HasValue).ToList();
            if (usageList != null && usageList.Count > 0)
            {
                var line = CreateLine("Usage", usageList, dateTimeList);
                _model.Series.Add(line);
            }

            var rainList = graphData.result.Select(item => item.getRain()).Where(item => item.HasValue).ToList();
            if (rainList != null && rainList.Count > 0)
            {
                var line = CreateLine("Rain", rainList, dateTimeList);
                _model.Series.Add(line);
            }

            var co2List = graphData.result.Select(item => item.getCo2()).Where(item => item.HasValue).ToList();
            if (co2List != null && co2List.Count > 0)
            {
                var line = CreateLine("Co2", co2List, dateTimeList);
                _model.Series.Add(line);
            }

            var co2MinList = graphData.result.Select(item => item.getCo2Min()).Where(item => item.HasValue).ToList();
            if (co2MinList != null && co2MinList.Count > 0)
            {
                var line = CreateLine("Min Co2", co2MinList, dateTimeList);
                _model.Series.Add(line);
            }

            var co2MaxList = graphData.result.Select(item => item.getCo2Max()).Where(item => item.HasValue).ToList();
            if (co2MaxList != null && co2MaxList.Count > 0)
            {
                var line = CreateLine("Max Co2", co2MaxList, dateTimeList);
                _model.Series.Add(line);
            }

            var luxList = graphData.result.Select(item => item.getLux()).Where(item => item.HasValue).ToList();
            if (luxList != null && luxList.Count > 0)
            {
                var line = CreateLine("Lux", luxList, dateTimeList);
                _model.Series.Add(line);
            }

            var luxMinList = graphData.result.Select(item => item.getLuxMin()).Where(item => item.HasValue).ToList();
            if (luxMinList != null && luxMinList.Count > 0)
            {
                var line = CreateLine("Min Lux", luxMinList, dateTimeList);
                _model.Series.Add(line);
            }

            var luxMaxList = graphData.result.Select(item => item.getLuxMax()).Where(item => item.HasValue).ToList();
            if (luxMaxList != null && luxMaxList.Count > 0)
            {
                var line = CreateLine("Max Lux", luxMaxList, dateTimeList);
                _model.Series.Add(line);
            }

            var luxAvgList = graphData.result.Select(item => item.getLuxAvg()).Where(item => item.HasValue).ToList();
            if (luxAvgList != null && luxAvgList.Count > 0)
            {
                var line = CreateLine("Avg Lux", luxAvgList, dateTimeList);
                _model.Series.Add(line);
            }

            MapTemperatureLines(graphData, dateTimeList);
        }

        /// <summary>
        /// Map Temperature lines
        /// </summary>
        /// <param name="graphData">The graphData<see cref="GraphModel"/></param>
        /// <param name="dateTimeList">The dateTimeList<see cref="IReadOnlyList{DateTime?}"/></param>
        private void MapTemperatureLines(GraphModel graphData, IReadOnlyList<DateTime?> dateTimeList)
        {
            if (!graphData.result.Any(item => item.hasTemperatureRange())) return;
            {
                var temperatureMinList = graphData.result.Select(item => item.getTemperatureMin())
                    .Where(item => item.HasValue)
                    .ToList();
                if (temperatureMinList != null && temperatureMinList.Count > 0)
                {
                    var line = CreateLine("Min Temperature", temperatureMinList, dateTimeList);
                    _model.Series.Add(line);
                }

                var temperatureMaxList = graphData.result.Select(item => item.getTemperatureMax())
                    .Where(item => item.HasValue)
                    .ToList();
                if (temperatureMaxList == null || temperatureMaxList.Count <= 0) return;
                {
                    var line = CreateLine("Max Temperature", temperatureMaxList, dateTimeList);
                    _model.Series.Add(line);
                }
            }
        }

        /// <summary>
        /// Create Graph Line
        /// </summary>
        /// <param name="title">The title<see cref="string"/></param>
        /// <param name="graphData">The graphData<see cref="IEnumerable{double?}"/></param>
        /// <param name="graphDateTime">The graphDateTime<see cref="IReadOnlyList{DateTime?}"/></param>
        /// <returns>The <see cref="LineSeries"/></returns>
        private LineSeries CreateLine(string title, IEnumerable<double?> graphData,
            IReadOnlyList<DateTime?> graphDateTime)
        {
            var lineTemperature = CreateLine(title);
            var counter = 0;
            foreach (var data in graphData)
            {
                if (data.HasValue)
                {
                    var dateTime = graphDateTime[counter];
                    if (dateTime != null)
                        lineTemperature.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime.Value),
                            data.Value));
                }
                counter++;
            }

            return lineTemperature;
        }

        /// <summary>
        /// Init line object with values
        /// </summary>
        /// <param name="title">The title<see cref="string"/></param>
        /// <returns>The <see cref="LineSeries"/></returns>
        private LineSeries CreateLine(string title)
        {
            var lineColor = $"#{_random.Next(0x1000000):X6}";
            return new LineSeries
            {
                StrokeThickness = 2,
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
            _model = new PlotModel();
            var format = "HH:ss";
            switch (_range)
            {
                case ConstantValues.GraphRange.Month:
                    format = "dd/MM";
                    break;
                case ConstantValues.GraphRange.Year:
                    format = "MM/yyyy";
                    break;
                case ConstantValues.GraphRange.Day:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _model.TextColor = OxyColor.Parse("#757575");
            _model.Axes.Add(
                new DateTimeAxis { Title = "DateTime", Position = AxisPosition.Bottom, StringFormat = format });
            _model.Axes.Add(new LinearAxis { Title = "Value", Position = AxisPosition.Left });
            _model.Annotations.Add(new LineAnnotation()
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
            _model.LegendTitle = "Legende";
            _model.LegendOrientation = LegendOrientation.Horizontal;
            _model.LegendPlacement = LegendPlacement.Outside;
            _model.LegendPosition = LegendPosition.BottomCenter;
            _model.LegendBackground =
                OxyColor.FromAColor(200, App.AppSettings.DarkTheme ? OxyColors.Black : OxyColors.White);
            _model.LegendBorder = OxyColors.Black;
        }

        /// <summary>
        /// Filter the graph result
        /// </summary>
        private void FilterGraphResult()
        {
            if (string.Compare(AppResources.filterOn_all, activeFilter, StringComparison.OrdinalIgnoreCase) == 0)
                RevertOriginalSource();
            else if (string.Compare(AppResources.cancel, activeFilter, StringComparison.OrdinalIgnoreCase) != 0)
            {
                BackupSource();
                _model.Series.Clear(); //remove list
                foreach (var serie in _originalSeries)
                {
                    if (string.Compare(serie.Title, activeFilter, StringComparison.OrdinalIgnoreCase) == 0)
                        _model.Series.Add(serie);
                }
            }
        }

        /// <summary>
        /// Backup the original graph source
        /// </summary>
        private void BackupSource()
        {
            _originalSeries = new List<Series>();
            foreach (var s in _model.Series)
                _originalSeries.Add(s); //backup
        }

        /// <summary>
        /// Revert to saved original source
        /// </summary>
        private void RevertOriginalSource()
        {
            if (_originalSeries == null) return;
            _model.Series.Clear(); //revert original
            foreach (var s in _originalSeries)
                _model.Series.Add(s);
            _originalSeries = null;
        }

        /// <summary>
        /// Create Filter menu
        /// </summary>
        /// <returns>The <see cref="List{string}"/></returns>
        private List<string> CreateFilterMenu()
        {
            var actions = new List<string>();
            if (_model == null) return actions;
            actions.Add(AppResources.filterOn_all);
            actions.AddRange(_model.Series.Select(serie => serie.Title));
            return actions;
        }

        #endregion
    }
}
