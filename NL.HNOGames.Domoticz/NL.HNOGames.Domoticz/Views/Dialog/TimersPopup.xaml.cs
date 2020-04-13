using NL.HNOGames.Domoticz.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Device = NL.HNOGames.Domoticz.Models.Device;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    /// <summary>
    /// Defines the <see cref="TimersPopup" />
    /// </summary>
    public partial class TimersPopup
    {
        #region Variables

        /// <summary>
        /// Defines the _selectedDevice
        /// </summary>
        private readonly object _selectedDevice;

        /// <summary>
        /// Defines the _timerList
        /// </summary>
        private List<Timer> _timerList;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TimersPopup"/> class.
        /// </summary>
        /// <param name="device">The device<see cref="object"/></param>
        public TimersPopup(object device)
        {
            _selectedDevice = device;
            InitializeComponent();
        }

        #endregion

        #region Private

        /// <summary>
        /// The OnItemSelected
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="args">The args<see cref="SelectedItemChangedEventArgs"/></param>
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        /// <summary>
        /// The ExecuteLoadNotificationsCommand
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExecuteLoadNotificationsCommand()
        {
            _timerList = new List<Timer>();

            var idx = "";
            if (_selectedDevice is Device device)
                idx = device.idx;
            else if (_selectedDevice is Scene)
                idx = ((Scene)_selectedDevice).idx;

            var timers = await App.ApiService.GetTimers(idx);
            if (timers?.result != null)
            {
                foreach (var t in timers.result)
                    _timerList.Add(t);
                listView.ItemsSource = _timerList;
            }
        }

        /// <summary>
        /// The btnOK_Clicked
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnOK_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        #endregion

        /// <summary>
        /// The OnAppearing
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadNotificationsCommand()).Execute(null);
        }
    }
}
