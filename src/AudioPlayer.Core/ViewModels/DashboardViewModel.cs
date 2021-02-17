using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioPlayer.Services;
using System.Reactive.Linq;
using MvvmUtils;

namespace AudioPlayer.ViewModels
{
	public class DashboardViewModel : AxisViewModelBase
    {
        private string displayName;
        private RelayCommand navigateToTrackCommand;
        private RelayCommand openAudioPlayerCommand;

		public DashboardViewModel(IAppContext context,
		                          IMessageBus messageBus,
		                          IMusicNavigator navigationService,
		                          IDeviceServices deviceServices)
			: base(context, messageBus)
        {
			Navigation = navigationService;
			DeviceServices = deviceServices;

            Context.DeviceChanged += Context_DeviceSet;
        }

        private void Context_DeviceSet(object sender, System.EventArgs e)
        {
            DisplayName = Context.Device?.DisplayName;
        }

        public string DisplayName
        {
            get => displayName;
            set => SetProperty(ref displayName, value);
        }

        public async Task Initialize()
        {
            DisplayName = Context?.Device?.DisplayName;
        }

        public ICommand ShowPlayingNowCommand => navigateToTrackCommand ?? (navigateToTrackCommand = new RelayCommand(async () =>
		{
			MessageBus.Publish(new UpdatePlayer());
			await Navigation.PushModal(Pages.NowPlaying);
		}));

        public ICommand OpenAudioPlayerCommand => openAudioPlayerCommand ?? (openAudioPlayerCommand = new RelayCommand(()
            => DeviceServices.OpenBrowser(new Uri($"http://{Context.Device.IPAddress}"))));

        public IDeviceServices DeviceServices { get; }
        public IMusicNavigator Navigation { get; }
    }
}
