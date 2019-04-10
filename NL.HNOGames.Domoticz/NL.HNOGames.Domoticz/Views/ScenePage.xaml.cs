using System;
using System.Threading.Tasks;
using NL.HNOGames.Domoticz.ViewModels;
using Xamarin.Forms;
using NL.HNOGames.Domoticz.Resources;
using System.Linq;
using NL.HNOGames.Domoticz.Data;
using Rg.Plugins.Popup.Services;
using NL.HNOGames.Domoticz.Views.Dialog;
using System.Collections.Generic;

namespace NL.HNOGames.Domoticz.Views
{
   public partial class ScenePage
   {
      private readonly SceneViewModel _viewModel;

      public ScenePage()
      {
         InitializeComponent();
         BindingContext = _viewModel = new SceneViewModel();
         _viewModel.SetListViewVisibilityMethod += DelegateListViewMethod;
         App.AddLog("Loading screen: Scenes");
         adView.IsVisible = !App.AppSettings.PremiumBought;
      }

      /// <summary>
      /// Set listview visibility (no items found)
      /// </summary>
      /// <param name="isvisible"></param>
      private void DelegateListViewMethod(bool isvisible)
      {
         listView.IsVisible = isvisible;
      }


      /// <summary>
      /// Show a actionsheet on item selected
      /// </summary>
      private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
      {
         var item = args.SelectedItem as Models.Scene;
         if (item == null)
            return;

         var actions = new List<string> { AppResources.favorite, AppResources.button_status_log };
         if (!string.IsNullOrEmpty(item.Timers) &&
             string.Compare(item.Timers, "true", StringComparison.OrdinalIgnoreCase) == 0)
            actions.Add(AppResources.button_status_timer);
         var tempDevice = new Models.Scene
         {
            idx = item.idx,
            Type = item.Type,
            Name = item.Name
         };

         var result = await DisplayActionSheet(item.Name, AppResources.cancel, null, actions.ToArray());
         if (result == AppResources.favorite)
            await SetFavorite(item);
         else if (result == AppResources.button_status_log)
            await PopupNavigation.Instance.PushAsync(new LogsPopup(tempDevice));
         else if (result == AppResources.button_status_timer)
            await PopupNavigation.Instance.PushAsync(new TimersPopup(tempDevice));
         listView.SelectedItem = null;
      }

      /// <summary>
      /// Set Favorite
      /// </summary>
      private async Task SetFavorite(Models.Scene pair)
      {
         if (!pair.FavoriteBoolean)
            App.ShowToast(pair.Name + " " + AppResources.favorite_added);
         else
            App.ShowToast(pair.Name + " " + AppResources.favorite_removed);
         var result = await App.ApiService.SetFavorite(pair.idx, true, !pair.FavoriteBoolean);
         if (!result)
            App.ShowToast(pair.Name + " " + AppResources.error_favorite);

         _viewModel.RefreshFavoriteCommand.Execute(null);
      }

      /// <summary>
      /// On Appearing
      /// </summary>
      protected override async void OnAppearing()
      {
         base.OnAppearing();
         if (_viewModel.Devices == null || _viewModel.OldData)
            _viewModel.RefreshFavoriteCommand.Execute(null);
         //listView.RowHeight = 130;

         var info = await App.GetSunRiseInfoAsync();
         if (info != null)
            subtitle.Text = $"↑{info.Sunrise} ↓{info.Sunset}";

      }

      /// <summary>
      /// Filter changed
      /// </summary>
      private void sbSearch_TextChanged(object sender, TextChangedEventArgs e)
      {
         try
         {
            var filterText = e.NewTextValue.ToLower().Trim();
            if (filterText == string.Empty)
            {
               listView.ItemsSource = null;
               listView.ItemsSource = _viewModel.Devices;
            }
            else
            {
               listView.ItemsSource = null;
               listView.ItemsSource =
                   _viewModel.Devices.Where(i => i.Name.ToLower().Trim().Contains(filterText));
            }
         }
         catch (Exception)
         {
            listView.ItemsSource = null;
            listView.ItemsSource = _viewModel.Devices;
         }
      }

      #region On Off Switches

      /// <summary>
      /// Turn switch/device ON
      /// </summary>
      private async void btnOnButton_Clicked(object sender, EventArgs e)
      {
         if (_viewModel.OldData)
            return;
         Button oButton = (Button)sender;
         Models.Scene oDevice = (Models.Scene)oButton.BindingContext;
         App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);

         await App.ApiService.SetSwitch(oDevice.idx, true,
             oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
             oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
         _viewModel.RefreshActionCommand.Execute(null);
      }

      /// <summary>
      /// Turn switch/device OFF
      /// </summary>
      private async void btnOffButton_Clicked(object sender, EventArgs e)
      {
         var oButton = (Button)sender;
         var oDevice = (Models.Scene)oButton.BindingContext;
         App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);

         await App.ApiService.SetSwitch(oDevice.idx, false,
             oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
             oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
         _viewModel.RefreshActionCommand.Execute(null);
      }

      /// <summary>
      /// Toggle switch
      /// </summary>
      private async void btnSwitch_Toggled(object sender, ToggledEventArgs e)
      {
         var oSwitch = (Switch)sender;
         var oDevice = (Models.Scene)oSwitch.BindingContext;

         if (oSwitch.IsToggled == oDevice.StatusBoolean) return;
         if (oDevice.StatusBoolean)
            App.ShowToast(AppResources.switch_off + ": " + oDevice.Name);
         else
            App.ShowToast(AppResources.switch_on + ": " + oDevice.Name);
         await App.ApiService.SetSwitch(oDevice.idx, oSwitch.IsToggled,
             oDevice.Type == ConstantValues.Device.Scene.Type.GROUP ||
             oDevice.Type == ConstantValues.Device.Scene.Type.SCENE);
         _viewModel.RefreshActionCommand.Execute(null);
      }

      #endregion On Off Switches


      #region Selector

      /// <summary>
      /// Set Selector value to Domoticz
      /// </summary>
      private void pSelector_Unfocused(object sender, FocusEventArgs e)
      {
      }

      #endregion Selector

      #region Blinds

      /// <summary>
      /// Turn blinds ON/DOWN
      /// </summary>
      private void btnBlindOnButton_Clicked(object sender, EventArgs e)
      {
      }

      /// <summary>
      /// Turn blinds OFF/UP
      /// </summary>
      private void btnBlindOffButton_Clicked(object sender, EventArgs e)
      {
      }

      /// <summary>
      /// Turn blinds Stop
      /// </summary>
      private void btnBlindStopButton_Clicked(object sender, EventArgs e)
      {
      }

      #endregion Blinds


      #region Dimmer

      /// <summary>
      /// Slider value of the dimmer
      /// </summary>
      private void btnLevelButton_Clicked(object sender, EventArgs e)
      {
      }

      #endregion Dimmer
   }
}