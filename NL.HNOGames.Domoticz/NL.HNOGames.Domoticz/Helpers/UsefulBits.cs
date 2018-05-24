using Plugin.DeviceInfo;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NL.HNOGames.Domoticz.Helpers
{
   public static class UsefulBits
   {
      /// <summary>
      /// check if is numeric
      /// </summary>
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

      /// <summary>
      /// Decode base 64 string
      /// </summary>
      public static string DecodeBase64String(string input)
      {
         if (String.IsNullOrEmpty(input))
            return null;
         byte[] plain = Convert.FromBase64String(input);
         return (Encoding.GetEncoding("UTF-8")).GetString(plain);
      }
   }
}
