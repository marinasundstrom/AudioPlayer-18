using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Axis.AudioPlayer.Messages;
using Axis.AudioPlayer.Services;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
	public class DevicesViewModel : AxisViewModelBase
	{
		private RelayCommand addDeviceCommand;
		private RelayCommand selectDeviceCommand;
		private readonly RelayCommand addCustomDeviceCommand;

		private Data.Device selectedDevice;

		private IDisposable deviceAddedSubscription;
		private IDisposable deviceUpdatedSubscription;

		public DevicesViewModel(IAppContext context,
								IMessageBus messageBus,
								IMusicNavigator navigator,
								IDataService dataService,
		                        IPopupService popupService) : base(context, messageBus)
		{
			Player = Context.Player;
			Navigator = navigator;
			DataService = dataService;
			PopupService = popupService;

			Devices = new ObservableCollection<Data.Device>();
		}

		public IPlayerService Player { get; }
		public IMusicNavigator Navigator { get; }
		public IDataService DataService { get; }
		public IPopupService PopupService { get; }

		public ObservableCollection<Data.Device> Devices { get; }

		public Data.Device SelectedDevice
		{
			get => selectedDevice;
			set => SetProperty(ref selectedDevice, value);
		}

		public async Task InitializeAsync()
		{
            if (Devices.Count == 0)
            {
                await UpdateList();
            }

            deviceAddedSubscription = MessageBus.WhenPublished<DeviceAdded>().Subscribe(async deviceAdded => await UpdateList());
            deviceUpdatedSubscription = MessageBus.WhenPublished<DeviceDeleted>().Subscribe(async deviceDeleted => await UpdateList());
		}

		private async Task UpdateList()
		{
			var devices = await DataService.GetDevicesAsync();
			Devices.Clear();
			foreach (var device in devices.OrderBy(x => x.Id))
			{
				Devices.Add(device);
			}
		}

		public override void CleanUp()
		{
			base.CleanUp();

			deviceAddedSubscription.Dispose();
			deviceUpdatedSubscription.Dispose();
		}

		public ICommand AddDeviceCommand => addDeviceCommand ?? (addDeviceCommand = RelayCommand.Create(async () => await Navigator.PushModal(Pages.DeviceWizard)));

		public ICommand SelectDeviceCommand => selectDeviceCommand ?? (selectDeviceCommand = RelayCommand.Create(async () =>
		{
			try
			{
				await Context.SetDevice(SelectedDevice);
			}
			catch (Exception)
			{
				await PopupService.DisplayAlertAsync("Loading failed", "Device could not be loaded into context.", new[] {
					new PopupAction {
						Text = "OK",
						IsCancel = true
					}
				});
			}
		}));
	}
}
