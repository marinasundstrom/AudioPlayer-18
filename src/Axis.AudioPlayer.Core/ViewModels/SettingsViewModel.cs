using System.Collections.ObjectModel;
using System.Windows.Input;
using Axis.AudioPlayer.Messages;
using Axis.AudioPlayer.Services;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
	public class SettingsViewModel : AxisViewModelBase
    {
        private RelayCommand goToDeviceDiscovery;
        private RelayCommand forgetZoneCommand;

		public SettingsViewModel(IAppContext context,
							 IMessageBus messageBus,
		                     ISettingsNavigator navigationService,
							 IPopupService popupService)
			: base(context, messageBus)
		{
			Navigation = navigationService;
			PopupService = popupService;

            MenuItems = new ObservableCollection<MenuItem>()
            {
                new MenuItem() {
                    Title = "Forget zone",
                    Command = ForgetZoneCommand
                }
            };
        }

        public ObservableCollection<MenuItem> MenuItems { get; }

        public ICommand GoToDeviceDiscovery => goToDeviceDiscovery ?? (goToDeviceDiscovery = RelayCommand.Create(() => Navigation.NavigateTo(Pages.DeviceDiscovery)));

        public ICommand ForgetZoneCommand => forgetZoneCommand ?? (forgetZoneCommand = RelayCommand.Create(async () =>
        {
            var device = Context.Device;
            await Context.ForgetDevice();
			MessageBus.Publish(new DeviceDeleted(device.Id));
            await Context.SetDevice(null);
        }));

		public ISettingsNavigator Navigation { get; }
        public IPopupService PopupService { get; }
    }
}
