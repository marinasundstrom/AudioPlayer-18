using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Axis.AudioPlayer.Services;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
	public class MainViewModel : AxisViewModelBase
    {
        private RelayCommand navigateToDeviceDiscoveryCommand;
		private MenuItem selectedItem;
		private bool isDevicesVisible;

		public MainViewModel(IAppContext context,
		                     IMessageBus messageBus,
		                     IMusicNavigator navigationService,
		                     IPopupService popupService)
			: base(context, messageBus)
        {
			Navigation = navigationService;
			PopupService = popupService;

			Context.DeviceChanged += Context_DeviceSet;

            Items = new ObservableCollection<MenuItem>();
        }

        private void Context_DeviceSet(object sender, System.EventArgs e)
        {
            UpdateTabs();

			IsDevicesVisible = false;
        }

        public async Task Initialize()
        {
            UpdateTabs();
        }

        private void UpdateTabs()
        {
            if (Context.Device != null)
            {
                if (Items.Count != 3)
                {
                    Items.Clear();

                    Items.Add(new MenuItem() { Title = "Dashboard" });
                    Items.Add(new MenuItem() { Title = "Music" });
                    //Items.Add(new MenuItem() { Title = "Announcement" });
                    Items.Add(new MenuItem() { Title = "Settings" });
                }
            }
            else
            {
				Items.Clear();
                Items.Add(new MenuItem() { Title = "Welcome" });
            }
        }

		public bool IsDevicesVisible 
		{
			get => isDevicesVisible;
			set => SetProperty(ref isDevicesVisible, value);
		}

		public MenuItem SelectedItem 
        {
			get => selectedItem;
			set => SetProperty(ref selectedItem, value);
		}

		public ObservableCollection<MenuItem> Items { get; }

		public ICommand NavigateToDeviceDiscoveryCommand => navigateToDeviceDiscoveryCommand ?? (navigateToDeviceDiscoveryCommand = RelayCommand.Create(async () => await Navigation.PushModal(Pages.DeviceWizard)));

        public IPopupService PopupService { get; }
        public IMusicNavigator Navigation { get; }
    }
}
