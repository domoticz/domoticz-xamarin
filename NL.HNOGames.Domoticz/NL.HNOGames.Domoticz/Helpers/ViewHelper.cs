using NL.HNOGames.Domoticz.Data;
using System;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// Defines the <see cref="ViewHelper" />
    /// </summary>
    public static class ViewHelper
    {
        #region Public

        /// <summary>
        /// Dynamicly determine the height of a table row
        /// </summary>
        /// <param name="mDeviceInfo">The mDeviceInfo<see cref="Models.Device"/></param>
        /// <param name="dashboard">The dashboard<see cref="Boolean"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int GetTemplateHeight(Models.Device mDeviceInfo, Boolean dashboard)
        {
            int oReturnvalue = 130;
            if (mDeviceInfo.SwitchTypeVal == 0 &&
                    (mDeviceInfo.SwitchType == null))
            {
                if (mDeviceInfo.SubType != null &&
                    string.Compare(mDeviceInfo.SubType, ConstantValues.Device.Utility.SubType.SMARTWARES, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    oReturnvalue = 130;
                }
                else
                {
                    switch (mDeviceInfo.Type)
                    {
                        case ConstantValues.Device.Scene.Type.GROUP:
                        case ConstantValues.Device.Scene.Type.SCENE:
                        case ConstantValues.Device.Utility.Type.THERMOSTAT:
                        case ConstantValues.Device.Utility.Type.HEATING:
                            oReturnvalue = 130;
                            break;
                        default:
                            oReturnvalue = 130;
                            break;
                    }
                }
            }
            else if ((mDeviceInfo.SwitchType == null))
                oReturnvalue = 130;
            else
            {
                switch (mDeviceInfo.SwitchTypeVal)
                {
                    case ConstantValues.Device.Type.Value.X10SIREN:
                    case ConstantValues.Device.Type.Value.MOTION:
                    case ConstantValues.Device.Type.Value.CONTACT:
                    case ConstantValues.Device.Type.Value.DUSKSENSOR:
                    case ConstantValues.Device.Type.Value.SMOKE_DETECTOR:
                    case ConstantValues.Device.Type.Value.DOORBELL:
                    case ConstantValues.Device.Type.Value.PUSH_ON_BUTTON:
                    case ConstantValues.Device.Type.Value.PUSH_OFF_BUTTON:
                    case ConstantValues.Device.Type.Value.ON_OFF:
                    case ConstantValues.Device.Type.Value.MEDIAPLAYER:
                    case ConstantValues.Device.Type.Value.DOORLOCK:
                    case ConstantValues.Device.Type.Value.DOORCONTACT:
                        oReturnvalue = 130;
                        break;

                    case ConstantValues.Device.Type.Value.DIMMER:
                    case ConstantValues.Device.Type.Value.BLINDPERCENTAGE:
                    case ConstantValues.Device.Type.Value.BLINDPERCENTAGEINVERTED:
                        oReturnvalue = 160;
                        break;

                    case ConstantValues.Device.Type.Value.SELECTOR:
                        oReturnvalue = 160;
                        break;

                    case ConstantValues.Device.Type.Value.BLINDS:
                    case ConstantValues.Device.Type.Value.BLINDINVERTED:
                    case ConstantValues.Device.Type.Value.BLINDVENETIAN:
                    case ConstantValues.Device.Type.Value.BLINDVENETIANUS:
                        oReturnvalue = 130;
                        break;
                }
            }

            if (dashboard && !App.AppSettings.ShowExtraData)
                oReturnvalue = oReturnvalue - 30;

            return oReturnvalue;
        }

        #endregion
    }
}
