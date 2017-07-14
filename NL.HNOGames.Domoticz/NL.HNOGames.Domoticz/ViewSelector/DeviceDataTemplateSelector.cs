using NL.HNOGames.Domoticz.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.ViewSelector
{
    public class DeviceDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate SelectorTemplate { get; set; }
        public DataTemplate OnOffButtonTemplate { get; set; }
        public DataTemplate OnOffSwitchTemplate { get; set; }
        public DataTemplate OnButtonTemplate { get; set; }
        public DataTemplate SetButtonTemplate { get; set; }
        public DataTemplate OffButtonTemplate { get; set; }
        public DataTemplate ModalButtonTemplate { get; set; }
        public DataTemplate DimmerRGBButtonTemplate { get; set; }
        public DataTemplate DimmerButtonTemplate { get; set; }
        public DataTemplate BlindsButtonTemplate { get; set; }
        public DataTemplate SwitchDimmerTemplate { get; set; }
        public DataTemplate SwitchDimmerRGBButtonTemplate { get; set; }
        public DataTemplate SecurityPanelTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            DataTemplate oReturnvalue = DefaultTemplate;

            try
            {
                Models.Device mDeviceInfo = (Models.Device)item;

                if (mDeviceInfo.SwitchTypeVal == 0 &&
                        (mDeviceInfo.SwitchType == null))
                {
                    if (mDeviceInfo.SubType != null &&
                        String.Compare(mDeviceInfo.SubType, ConstantValues.Device.Utility.SubType.SMARTWARES, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        oReturnvalue = SetButtonTemplate;
                    }
                    else
                    {
                        switch (mDeviceInfo.Type)
                        {
                            case ConstantValues.Device.Scene.Type.GROUP:
                                if (App.AppSettings.ShowSwitches)
                                    oReturnvalue = OnOffSwitchTemplate;
                                else
                                    oReturnvalue = OnOffButtonTemplate;
                                break;
                            case ConstantValues.Device.Scene.Type.SCENE:
                                oReturnvalue = OnButtonTemplate;
                                break;
                            case ConstantValues.Device.Utility.Type.THERMOSTAT:
                                oReturnvalue = SetButtonTemplate;
                                break;
                            case ConstantValues.Device.Utility.Type.HEATING:
                                oReturnvalue = SetButtonTemplate;
                                break;
                            default:
                                oReturnvalue = DefaultTemplate;
                                break;
                        }
                    }
                }
                else if ((mDeviceInfo.SwitchType == null))
                    oReturnvalue = DefaultTemplate;
                else
                {
                    switch (mDeviceInfo.SwitchTypeVal)
                    {
                        case ConstantValues.Device.Type.Value.ON_OFF:
                        case ConstantValues.Device.Type.Value.MEDIAPLAYER:
                        case ConstantValues.Device.Type.Value.DOORLOCK:
                        case ConstantValues.Device.Type.Value.DOORCONTACT:
                            switch (mDeviceInfo.SwitchType)
                            {
                                case ConstantValues.Device.Type.Name.SECURITY:
                                    if (String.Compare(mDeviceInfo.SubType, ConstantValues.Device.SubType.Name.SECURITYPANEL, StringComparison.OrdinalIgnoreCase) == 0)
                                        oReturnvalue = SecurityPanelTemplate;
                                    else
                                        oReturnvalue = DefaultTemplate;
                                    break;
                                case ConstantValues.Device.Type.Name.EVOHOME:
                                    if (String.Compare(mDeviceInfo.SubType, ConstantValues.Device.SubType.Name.EVOHOME, StringComparison.OrdinalIgnoreCase) == 0)
                                        oReturnvalue = ModalButtonTemplate;
                                    else
                                        oReturnvalue = DefaultTemplate;
                                    break;
                                default:
                                    if (App.AppSettings.ShowSwitches)
                                        oReturnvalue = OnOffSwitchTemplate;
                                    else
                                        oReturnvalue = OnOffButtonTemplate;
                                    break;
                            }
                            break;

                        case ConstantValues.Device.Type.Value.X10SIREN:
                        case ConstantValues.Device.Type.Value.MOTION:
                        case ConstantValues.Device.Type.Value.CONTACT:
                        case ConstantValues.Device.Type.Value.DUSKSENSOR:
                        case ConstantValues.Device.Type.Value.SMOKE_DETECTOR:
                        case ConstantValues.Device.Type.Value.DOORBELL:
                            oReturnvalue = OnButtonTemplate;
                            break;
                        case ConstantValues.Device.Type.Value.PUSH_ON_BUTTON:
                            oReturnvalue = OnButtonTemplate;
                            break;

                        case ConstantValues.Device.Type.Value.PUSH_OFF_BUTTON:
                            oReturnvalue = OffButtonTemplate;
                            break;

                        case ConstantValues.Device.Type.Value.DIMMER:
                        case ConstantValues.Device.Type.Value.BLINDPERCENTAGE:
                        case ConstantValues.Device.Type.Value.BLINDPERCENTAGEINVERTED:
                            if (mDeviceInfo.SubType.StartsWith(ConstantValues.Device.SubType.Name.RGB))
                                if (App.AppSettings.ShowSwitches)
                                    oReturnvalue = SwitchDimmerRGBButtonTemplate;
                                else
                                    oReturnvalue = DimmerRGBButtonTemplate;
                            else
                            {
                                if (App.AppSettings.ShowSwitches)
                                    oReturnvalue = SwitchDimmerTemplate;
                                else
                                    oReturnvalue = DimmerButtonTemplate;
                            }
                            break;

                        case ConstantValues.Device.Type.Value.SELECTOR:
                            oReturnvalue = SelectorTemplate;
                            break;

                        case ConstantValues.Device.Type.Value.BLINDS:
                        case ConstantValues.Device.Type.Value.BLINDINVERTED:
                            if (ConstantValues.CanHandleStopButton(mDeviceInfo))
                                oReturnvalue = BlindsButtonTemplate;
                            else
                            {
                                if (App.AppSettings.ShowSwitches)
                                    oReturnvalue = OnOffSwitchTemplate;
                                else
                                    oReturnvalue = OnOffButtonTemplate;
                            }
                            break;

                        case ConstantValues.Device.Type.Value.BLINDVENETIAN:
                        case ConstantValues.Device.Type.Value.BLINDVENETIANUS:
                            oReturnvalue = BlindsButtonTemplate;
                            break;

                        default:
                            oReturnvalue = DefaultTemplate;
                            break;
                    }
                }

                return oReturnvalue;
            }
            catch (Exception)
            {
                return oReturnvalue;
            }
        }
    }
}
