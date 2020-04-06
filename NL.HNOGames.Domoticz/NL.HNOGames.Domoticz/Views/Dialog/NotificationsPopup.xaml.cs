using NL.HNOGames.Domoticz.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    /// <summary>
    /// Defines the <see cref="NotificationsPopup" />
    /// </summary>
    public partial class NotificationsPopup
    {
        #region Variables

        /// <summary>
        /// Defines the _selectedDevice
        /// </summary>
        private readonly Models.Device _selectedDevice;

        /// <summary>
        /// Defines the _notificationList
        /// </summary>
        private List<Notification> _notificationList;

        #endregion

        #region Constructor & Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsPopup"/> class.
        /// </summary>
        /// <param name="device">The device<see cref="Models.Device"/></param>
        public NotificationsPopup(Models.Device device)
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
            _notificationList = new List<Notification>();
            var notifications = await App.ApiService.GetNotifications(_selectedDevice);
            if (notifications?.result != null)
            {
                foreach (var n in notifications.result)
                    _notificationList.Add(n);
                listView.ItemsSource = _notificationList;
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
