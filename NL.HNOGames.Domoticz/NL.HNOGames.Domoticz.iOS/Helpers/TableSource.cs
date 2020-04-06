using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace NL.HNOGames.Domoticz.iOS.Helpers
{
   public class TableSource : UITableViewSource
   {
      List<ToolbarItem> _tableItems;
      string[] _tableItemTexts;
      string CellIdentifier = "TableCell";
      UIView _tableSuperView = null;

      public TableSource(List<ToolbarItem> items, UIView tableSuperView)
      {
         _tableItems = items;
         _tableSuperView = tableSuperView;
         _tableItemTexts = items.Select(a => a.Text).ToArray();
      }

      public override nint RowsInSection(UITableView tableview, nint section)
      {
         return _tableItemTexts?.Length ?? 0;
      }

      public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
      {
         UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
         string item = _tableItemTexts[indexPath.Row];
         if (cell == null)
         { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }
         cell.TextLabel.Text = item;
         return cell;
      }

      public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
      {
         return 56;
      }

      public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
      {
         var command = _tableItems[indexPath.Row].Command;
         command.Execute(_tableItems[indexPath.Row].CommandParameter);
         tableView.DeselectRow(indexPath, true);
         tableView.RemoveFromSuperview();
         if (_tableSuperView != null)
         {
            _tableSuperView.RemoveFromSuperview();
         }
      }
   }
}