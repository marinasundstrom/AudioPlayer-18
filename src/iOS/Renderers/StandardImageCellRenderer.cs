using AudioPlayer.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AudioPlayer.iOS.Renderers
{
    public class StandardImageCellRenderer : ImageCellRenderer
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
