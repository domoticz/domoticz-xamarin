using NL.HNOGames.Domoticz.Data;
using System;
using Xamarin.Forms;
using Device = NL.HNOGames.Domoticz.Models.Device;

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
        public DataTemplate DimmerRgbButtonTemplate { get; set; }
        public DataTemplate DimmerButtonTemplate { get; set; }
        public DataTemplate BlindsButtonTemplate { get; set; }
        public DataTemplate SwitchDimmerTemplate { get; set; }
        public DataTemplate SwitchDimmerRgbButtonTemplate { get; set; }
        public DataTemplate SecurityPanelTemplate { get; set; }

        /// <summary>
        /// Select templates
        /// </summary>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var oReturnvalue = DefaultTemplate;

            try
            {
                if (item is Device info)
                {
                    var mDeviceInfo = info;
                    oReturnvalue = GetDeviceTemplate(mDeviceInfo);
                }
                else if (item is Models.Scene mDeviceInfo)
                {
                    oReturnvalue = GetSceneTemplate(mDeviceInfo);
                }
                return oReturnvalue;
            }
            catch (Exception)
            {
                return oReturnvalue;
            }
        }

        /// <summary>
        /// Get Device templates
        /// </summary>
        private DataTemplate GetDeviceTemplate(Device mDeviceInfo)
        {
            DataTemplate oReturnvalue;
            if (mDeviceInfo.SwitchTypeVal == 0 &&
           (mDeviceInfo.SwitchType == null))
            {
                if (mDeviceInfo.SubType != null &&
                    string.Compare(mDeviceInfo.SubType, ConstantValues.Device.Utility.SubType.SMARTWARES, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    oReturnvalue = SetButtonTemplate;
                }
                else
                {
                    switch (mDeviceInfo.Type)
                    {
                        case ConstantValues.Device.Scene.Type.GROUP:
                            oReturnvalue = App.AppSettings.ShowSwitches ? OnOffSwitchTemplate : OnOffButtonTemplate;
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
                                oReturnvalue = string.Compare(mDeviceInfo.SubType, ConstantValues.Device.SubType.Name.SECURITYPANEL, StringComparison.OrdinalIgnoreCase) == 0 ? SecurityPanelTemplate : DefaultTemplate;
                                break;
                            case ConstantValues.Device.Type.Name.EVOHOME:
                                oReturnvalue = string.Compare(mDeviceInfo.SubType, ConstantValues.Device.SubType.Name.EVOHOME, StringComparison.OrdinalIgnoreCase) == 0 ? ModalButtonTemplate : DefaultTemplate;
                                break;
                            default:
                                oReturnvalue = App.AppSettings.ShowSwitches ? OnOffSwitchTemplate : OnOffButtonTemplate;
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
                            oReturnvalue = App.AppSettings.ShowSwitches ? SwitchDimmerRgbButtonTemplate : DimmerRgbButtonTemplate;
                        else
                        {
                            oReturnvalue = App.AppSettings.ShowSwitches ? SwitchDimmerTemplate : DimmerButtonTemplate;
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
                            oReturnvalue = App.AppSettings.ShowSwitches ? OnOffSwitchTemplate : OnOffButtonTemplate;
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

        /// <summary>
        /// Get scene template (group or scene)
        /// </summary>
        private DataTemplate GetSceneTemplate(Models.Scene mDeviceInfo)
        {
            if (mDeviceInfo == null)
                return null;

            var oReturnvalue = DefaultTemplate;
            switch (mDeviceInfo.Type)
            {
                case ConstantValues.Device.Scene.Type.GROUP:
                    oReturnvalue = App.AppSettings.ShowSwitches ? OnOffSwitchTemplate : OnOffButtonTemplate;
                    break;
                case ConstantValues.Device.Scene.Type.SCENE:
                    oReturnvalue = OnButtonTemplate;
                    break;
            }

            return oReturnvalue;
        }
    }
}
