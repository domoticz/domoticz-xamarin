using NL.HNOGames.Domoticz.Models;
using NL.HNOGames.Domoticz.Resources;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Device = NL.HNOGames.Domoticz.Models.Device;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    /// <summary>
    /// Defines the <see cref="LogsPopup" />
    /// </summary>
    public partial class LogsPopup
    {
        #region Variables

        /// <summary>
        /// Defines the _selectedDevice
        /// </summary>
        private readonly object _selectedDevice;

        /// <summary>
        /// Defines the _logList
        /// </summary>
        private List<Log> _logList;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsPopup"/> class.
        /// </summary>
        /// <param name="device">The device<see cref="object"/></param>
        public LogsPopup(object device)
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
            var idx = "";
            if (_selectedDevice is Device device)
                idx = device.idx;
            else if (_selectedDevice is Scene)
                idx = ((Scene)_selectedDevice).idx;

            _logList = new List<Log>();
            var logs = await App.ApiService.GetLogs(idx, _selectedDevice is Scene);
            if (logs?.result == null)
                logs = await App.ApiService.GetLogs(idx, false, true);

            int counterMaxLogs = 0;
            if (logs?.result != null)
            {
                foreach (var n in logs.result)
                {
                    if (counterMaxLogs > 250)
                        break;
                    _logList.Add(n);
                    counterMaxLogs++;
                }
                listView.ItemsSource = _logList;
            }
            else
            {
                App.ShowToast(AppResources.error_logs);
                await Navigation.PopAsync();
            }
        }

        /// <summary>
        /// The btnOK_Clicked
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        private void btnOK_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
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
