using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.HNOGames.Domoticz.Helpers
{
    public static class UsefulBits
    {
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        public static String GetDeviceID()
        {
            return CrossDeviceInfo.Current.Id;
        }

        public static String GetMD5String(String input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return XLabs.Cryptography.MD5.GetMd5String(input);
        }
    }
}
