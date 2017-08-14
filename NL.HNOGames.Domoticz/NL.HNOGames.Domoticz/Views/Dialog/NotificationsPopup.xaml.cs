using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using Rg.Plugins.Popup.Services;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public partial class NotificationsPopup
    {
        private readonly Models.Device _selectedDevice;
        private List<Notification> _notificationList;

        public NotificationsPopup(Models.Device device)
        {
            _selectedDevice = device;
            InitializeComponent();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadNotificationsCommand()).Execute(null);
        }

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

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
    }
}