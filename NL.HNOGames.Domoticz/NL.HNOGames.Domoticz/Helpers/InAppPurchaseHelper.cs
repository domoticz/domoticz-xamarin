using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using System;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Helpers
{
    public class InAppPurchaseHelper
    {

        public async Task<bool> PurchaseItem(string productId = "134845", string payload = "nl.hnogames.domoticz")
        {
            if (!CrossInAppBilling.IsSupported)
            {
                App.AddLog("InAppBilling not supported... ");
                return false;
            }

#if DEBUG
            CrossInAppBilling.Current.InTestingMode = true;
#else
            CrossInAppBilling.Current.InTestingMode = false;
#endif

            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();
                if (!connected)
                    return false;

                //check purchases
                var purchase = await billing.PurchaseAsync(productId, ItemType.InAppPurchase, payload);
                if (purchase == null)
                {
                    //did not purchase
                    App.AppSettings.PremiumBought = false;
                }
                else
                {
                    //purchased!
                    App.AppSettings.PremiumBought = true;
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                var message = string.Empty;
                switch (purchaseEx.PurchaseError)
                {
                    case PurchaseError.AppStoreUnavailable:
                        message = "Currently the app store seems to be unavailble. Try again later.";
                        break;
                    case PurchaseError.BillingUnavailable:
                        message = "Billing seems to be unavailable, please try again later.";
                        break;
                    case PurchaseError.PaymentInvalid:
                        message = "Payment seems to be invalid, please try again.";
                        break;
                    case PurchaseError.PaymentNotAllowed:
                        message = "Payment does not seem to be enabled/allowed, please try again.";
                        break;
                }

                //Something else has gone wrong, log it
                App.ShowToast(string.IsNullOrEmpty(message) ? "Issue with payment: " + purchaseEx.Message : "Issue with payment: " + message);
                App.AddLog(purchaseEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                App.AddLog("Issue connecting: " + ex);
                return false;
            }
            finally
            {
                await billing.DisconnectAsync();
                CrossInAppBilling.Dispose();
            }
            return true;
        }

    }
}
