using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace NL.HNOGames.Domoticz.iOS.Helpers
{
   public static class Extensions
   {
      public static IEnumerable<TType> GetSubViews<TType>(this UIView view)
        where TType : UIView
      {
         List<TType> result = new List<TType>();
         result.AddRange(view.Subviews.Where(sv => sv.GetType() == typeof(TType)).Cast<TType>());

         foreach (var sv in view.Subviews)
            result.AddRange(sv.GetSubViews<TType>());

         return result;
      }
   }
}