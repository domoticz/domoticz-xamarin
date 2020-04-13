using Acr.UserDialogs;
using NL.HNOGames.Domoticz.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace NL.HNOGames.Domoticz.Views.Settings
{
    /// <summary>
    /// Defines the <see cref="LocationPickerPage" />
    /// </summary>
    public partial class LocationPickerPage
    {
        #region Variables

        /// <summary>
        /// The callback method
        /// </summary>
        private readonly CallbackDelegate _Callback;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeofenceSettingsPage"/> class.
        /// </summary>
        public LocationPickerPage(CallbackDelegate callback)
        {
            _Callback = callback;
            InitializeComponent();

            SetCurrentLocation();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for the callback method
        /// </summary>
        public delegate void CallbackDelegate(int radius, string address, Xamarin.Forms.Maps.Position position);

        #endregion

        #region Private

        /// <summary>
        /// Set current location by default
        /// </summary>
        private async void SetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var position = new Xamarin.Forms.Maps.Position(location.Latitude, location.Longitude);
                    map.Pins.Clear();
                    map.Pins.Add(new Pin() { Label = $"{position.Latitude} | {position.Longitude}", Position = position });
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                          position,
                          Xamarin.Forms.Maps.Distance.FromKilometers(1)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (map.Pins.FirstOrDefault() == null)
                return;

            // Set location
            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig() { InputType = InputType.Number, Title = AppResources.radius, Text = "300" });
            if (result != null && result.Ok)
            {
                var name = txtAddress.Text;
                if (string.IsNullOrEmpty(name))
                    name = await GetNameAsync();
                if (!string.IsNullOrEmpty(name))
                {
                    _Callback.Invoke(int.Parse(result.Value), name, map.Pins.First().Position);
                    await Navigation.PopAsync();
                }
            }
        }

        private async Task<string> GetNameAsync()
        {
            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig() { InputType = InputType.Number, Title = AppResources.Location_name });
            if (result.Ok)
                return result.Value;
            return null;
        }

        private async void Entry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                var address = txtAddress.Text;
                var locations = await Geocoding.GetLocationsAsync(address);
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    var position = new Xamarin.Forms.Maps.Position(location.Latitude, location.Longitude);
                    map.Pins.Clear();
                    map.Pins.Add(new Xamarin.Forms.Maps.Pin() { Label = address, Position = position });
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                          position,
                          Xamarin.Forms.Maps.Distance.FromKilometers(1)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void map_MapClicked(object sender, MapClickedEventArgs e)
        {
            map.Pins.Clear();
            map.Pins.Add(new Pin() { Label = $"{e.Position.Latitude} | {e.Position.Longitude}", Position = e.Position });
            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                  e.Position,
                  Xamarin.Forms.Maps.Distance.FromKilometers(1)));
        }

        #endregion
    }
}
