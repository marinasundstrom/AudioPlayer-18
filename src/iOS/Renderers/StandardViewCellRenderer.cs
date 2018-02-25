using Axis.AudioPlayer.iOS;
using Axis.AudioPlayer.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(StandardViewCellRenderer))]
[assembly: ExportRenderer(typeof(TextCell), typeof(StandardTextCellRenderer))]
[assembly: ExportRenderer(typeof(ImageCell), typeof(StandardImageCellRenderer))]

namespace Axis.AudioPlayer.iOS.Renderers
{
    public class StandardViewCellRenderer : ViewCellRenderer
    {
        public override UIKit.UITableViewCell GetCell(Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            switch (item.StyleId)
            {
                case "none":
                    cell.Accessory = UIKit.UITableViewCellAccessory.None;
                    break;
                case "checkmark":
                    cell.Accessory = UIKit.UITableViewCellAccessory.Checkmark;
                    break;
                case "detail-button":
                    cell.Accessory = UIKit.UITableViewCellAccessory.DetailButton;
                    break;
                case "detail-disclosure-button":
                    cell.Accessory = UIKit.UITableViewCellAccessory.DetailDisclosureButton;
                    break;
                case "disclosure":
                default:
                    cell.Accessory = UIKit.UITableViewCellAccessory.DisclosureIndicator;
                    break;
            }
            return cell;
        }
    }
}
