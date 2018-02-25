using System.Linq;
using Xamarin.Forms;
using Axis.AudioPlayer.Views;

namespace Axis.AudioPlayer.Services
{
    public class SettingsNavigator : NavigationService, ISettingsNavigator
    {
        public SettingsNavigator()
            : base(() => ((Application.Current.MainPage as MasterDetailPage)
                                .Detail as TabbedPage)
                                .Children
                               .OfType<NavigationPage>()
                               .First(x => x.Title == "Settings"))
        {
            //Configure("DeviceDiscovery", typeof(DeviceDiscoveryPage));
        }
    }
}
