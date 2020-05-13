using Plugin.DeviceInfo;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace NL.HNOGames.Domoticz.Helpers
{
    /// <summary>
    /// Defines the <see cref="UsefulBits" />
    /// </summary>
    public static class UsefulBits
    {
        #region Public

        /// <summary>
        /// check if is numeric
        /// </summary>
        /// <param name="s">The s<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool IsNumeric(this string s)
        {
            return float.TryParse(s, out float output);
        }

        /// <summary>
        /// Get device id
        /// </summary>
        /// <returns></returns>
        public static String GetDeviceID()
        {
            return CrossDeviceInfo.Current.Id;
        }

        /// <summary>
        /// Get the MD5 string
        /// </summary>
        /// <param name="input">The input<see cref="string"/></param>
        /// <returns>The <see cref="String"/></returns>
        public static String GetMD5String(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return XLabs.Cryptography.MD5.GetMd5String(input);
        }

        /// <summary>
        /// Check if a string is base64 encoded
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsBase64Encoded(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            return Regex.Match(input, "^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$").Success;
        }

        internal static string getMd5String(string input)
        {
                // Use input string to calculate MD5 hash
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert the byte array to hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
        }

        /// <summary>
        /// Decode base 64 string
        /// </summary>
        /// <param name="input">The input<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string DecodeBase64String(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            byte[] plain = Convert.FromBase64String(input);
            return (Encoding.GetEncoding("UTF-8")).GetString(plain);
        }

        /// <summary>
        /// Get hex value of string
        /// </summary>
        /// <param name="color"></param>
        /// <param name="withAlpha">The withAlpha<see cref="bool"/></param>
        /// <returns></returns>
        public static string GetHexString(Color color, bool withAlpha = false)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            if (withAlpha)
                return $"{alpha:X2}{red:X2}{green:X2}{blue:X2}";
            else
                return $"{red:X2}{green:X2}{blue:X2}";
        }

        #endregion
    }
}
