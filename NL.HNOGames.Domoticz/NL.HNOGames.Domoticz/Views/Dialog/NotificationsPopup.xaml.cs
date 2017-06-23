using System;

using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using System.Collections.Generic;
using NL.HNOGames.Domoticz.Models;
using Rg.Plugins.Popup.Services;

namespace NL.HNOGames.Domoticz.Views.Dialog
{
    public partial class NotificationsPopup : PopupPage
    {
        private Models.Device selectedDevice;
        private List<Notification> notificationList;

        public NotificationsPopup(Models.Device device)
        {
            selectedDevice = device;
            InitializeComponent();
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            listView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            new Command(async () => await ExecuteLoadNotificationsCommand()).Execute(null);
        }

        async Task ExecuteLoadNotificationsCommand()
        {
            notificationList = new List<Notification>();

            var notifications = await App.ApiService.GetNotifications(selectedDevice);
            if (notifications != null && notifications.result != null)
            {
                foreach (Notification n in notifications.result)
                    notificationList.Add(n);
                listView.ItemsSource = notificationList;
            }
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }

    }
}
