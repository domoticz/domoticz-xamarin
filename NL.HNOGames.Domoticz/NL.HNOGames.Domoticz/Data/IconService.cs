using System;

namespace NL.HNOGames.Domoticz.Data
{
    public class IconService
    {
        public static String getDrawableIcon(String imgType, String devType, String switchType, bool State, bool useCustomImage, String CustomImage)
        {
            String standardImage = IconService.getDrawableIcon(imgType, devType, switchType, State);
            if ((useCustomImage
                        && ((CustomImage != null)
                        && (CustomImage.Length > 0))))
            {
                switch (CustomImage)
                {
                    case "Alarm":
                        standardImage = "alarm.png"; break;


                    case "Freezing":
                        standardImage = "freezing.png"; break;


                    case "Amplifier":
                        standardImage = "volume.png"; break;


                    case "Computer":
                    case "ComputerPC":
                        standardImage = "computer.png"; break;


                    case "Cooling":
                        standardImage = "cooling.png"; break;


                    case "ChristmasTree":
                        standardImage = "christmastree.png"; break;


                    case "Door":
                        standardImage = "door.png"; break;


                    case "Fan":
                        standardImage = "wind.png"; break;


                    case "Fireplace":
                        standardImage = "flame.png"; break;


                    case "Generic":
                        standardImage = "generic.png"; break;


                    case "Harddisk":
                        standardImage = "harddisk.png"; break;


                    case "Heating":
                        standardImage = "heating.png"; break;


                    case "Light":
                        standardImage = "lights.png"; break;


                    case "Media":
                        standardImage = "video.png"; break;


                    case "Phone":
                        standardImage = "phone.png"; break;


                    case "Speaker":
                        standardImage = "sub.png"; break;


                    case "Printer":
                        standardImage = "printer.png"; break;


                    case "TV":
                        standardImage = "tv.png"; break;


                    case "WallSocket":
                        standardImage = "wall.png"; break;


                    case "Water":
                        standardImage = "water.png"; break;

                }
            }

            return standardImage;
        }

        public static String getDrawableIcon(String imgType, String devType, String switchType, bool State)
        {
            String iconDrawable = "defaultimage";
            switch (imgType.ToLower())
            {
                case "scene":
                    iconDrawable = "generic";
                    break;

                case "group":
                    iconDrawable = "generic";
                    break;

                case "wind":
                    iconDrawable = "wind";
                    break;

                case "doorbell":
                    iconDrawable = "door";
                    break;

                case "door":
                    iconDrawable = "door";
                    break;

                case "lightbulb":
                    if (((switchType != null)
                                && ((switchType.Length > 0)
                                && switchType == ConstantValues.Device.Type.Name.DUSKSENSOR)))
                    {
                        if (State)
                            iconDrawable = "uvdark";
                        else
                            iconDrawable = "uvsunny";
                    }
                    else
                    {
                        iconDrawable = "lights";
                    }
                    break;

                case "push":
                    iconDrawable = "pushoff";
                    break;

                case "pushoff":
                    iconDrawable = "pushoff";
                    break;

                case "siren":
                    iconDrawable = "siren";
                    break;

                case "smoke":
                    iconDrawable = "smoke";
                    break;

                case "uv":
                    iconDrawable = "uv";
                    break;

                case "contact":
                    iconDrawable = "contact";
                    break;

                case "logitechMediaServer":
                    iconDrawable = "media";
                    break;

                case "media":
                    iconDrawable = "media";
                    break;

                case "blinds":
                    iconDrawable = "down";
                    break;

                case "dimmer":
                    if (((switchType != null)
                                && ((switchType.Length > 0)
                                && switchType.StartsWith(ConstantValues.Device.SubType.Name.RGB))))
                        iconDrawable = "rgb";
                    else
                        iconDrawable = "dimmer";
                    break;
                case "motion":
                    iconDrawable = "motion";
                    break;
                case "security":
                    iconDrawable = "security";
                    break;
                case "temperature":
                case "override_mini":
                    if (State)
                        iconDrawable = "heating";
                    else
                        iconDrawable = "cooling";
                    break;
                case "counter":
                    if (((devType != null)
                                && ((devType.Length > 0)
                                && devType == "P1 Smart Meter")))
                        iconDrawable = "wall";
                    else
                        iconDrawable = "up";
                    break;
                case "visibility":
                    iconDrawable = "visibility";
                    break;
                case "radiation":
                    iconDrawable = "radiation";
                    break;
                case "moisture":
                case "rain":
                    iconDrawable = "rain";
                    break;
                case "leaf":
                    iconDrawable = "leaf";
                    break;
                case "hardware":
                    iconDrawable = "computer";
                    break;
                case "fan":
                    iconDrawable = "fan";
                    break;
                case "speaker":
                    iconDrawable = "speaker";
                    break;
                case "current":
                    iconDrawable = "wall";
                    break;
                case "text":
                    iconDrawable = "text";
                    break;
                case "alert":
                    iconDrawable = "siren";
                    break;
                case "gauge":
                    iconDrawable = "gauge";
                    break;
                case "clock":
                    iconDrawable = "clock48";
                    break;
                case "mode":
                    iconDrawable = "defaultimage";
                    break;
                case "utility":
                    iconDrawable = "scale";
                    break;
                case "scale":
                    iconDrawable = "scale";
                    break;
                case "lux":
                    iconDrawable = "uvsunny";
                    break;
            }

            if (!string.IsNullOrEmpty(iconDrawable) && !string.IsNullOrEmpty(devType))
            {
                switch (devType.ToLower())
                {
                    case "heating":
                        iconDrawable = "heating";
                        break;
                    case "thermostat":
                        iconDrawable = "flame";
                        break;
                }
            }
            return iconDrawable + ".png";
        }
    }
}
