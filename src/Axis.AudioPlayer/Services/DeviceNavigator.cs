using System.Linq;
using Xamarin.Forms;
using Axis.AudioPlayer.Views;

namespace Axis.AudioPlayer.Services
{
	public class DeviceNavigator : NavigationService, IDeviceNavigator
	{
		public DeviceNavigator()
			: base(() => (Application.Current.MainPage.Navigation.ModalStack[0] as NavigationPage), true)
		{
			Configure(Pages.DeviceDiscovery, typeof(DeviceDiscoveryPage));
			Configure(Pages.AddDevice, typeof(AddDevicePage));
			Configure(Pages.AddDeviceCustom, typeof(AddDeviceCustomPage));
			Configure(Pages.AddDeviceCustomAlias, typeof(AddDeviceCustomAliasPage));
		}
	}
}
