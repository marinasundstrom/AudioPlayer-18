using System.ComponentModel;
using System.Windows.Input;
using Axis.AudioPlayer.Messages;
using Axis.AudioPlayer.Services;
using MvvmUtils;
using MvvmUtils.DataAnnotations;
using Plugin.Connectivity.Abstractions;

namespace Axis.AudioPlayer.ViewModels
{
    public class AddDeviceViewModel : AxisViewModelBase
    {
        private RelayCommand cancelCommand;
        private RelayCommand finishStep1Command;
        private RelayCommand backCommand;

        private WizardDetails step1;
        private DiscoveryDevice device;

        public AddDeviceViewModel(IAppContext context,
                                     IMessageBus messageBus,
                                     IDeviceNavigator navigationService,
                                     IDataService dataService,
                                     IPopupService popupService,
                                     IConnectivity connectivity)
            : base(context, messageBus)
        {
            DataService = dataService;
            Navigation = navigationService;
            PopupService = popupService;
            Connectivity = connectivity;
        }

        public DiscoveryDevice Device
        {
            get => device;
            set => SetAndValidateProperty(ref device, value);
        }

        public WizardDetails Step1
        {
            get => step1;
            set => SetAndValidateProperty(ref step1, value);
        }

        public void Initialize(DiscoveryDevice device)
        {
            Device = device;

            Step1 = new WizardDetails();
            Step1.PropertyChanged += P1;
        }

        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = RelayCommand.Create(async () => await Navigation.PopModal()));

        public ICommand FinishStep1Command => finishStep1Command ?? (finishStep1Command = RelayCommand.Create(async () =>
        {
            if (!Step1.Validate()) return;

            if (!Connectivity.IsConnected)
            {
                await PopupService.DisplayAlertAsync("Not connected", "Your device is not connected.", new[] {
                    new PopupAction {
                        Text = "OK"
                    }
                });
            }
            else if (!await Connectivity.IsReachable($"http://{Device.IPAddress}"))
            {
                await PopupService.DisplayAlertAsync("Host is not reachable", "Host is not recheable.", new[] {
                        new PopupAction {
                            Text = "OK"
                        }
                    });
            }
            else if (!await ConnectionUtils.TestConnectionAsync(new System.Uri($"http://{Device.IPAddress}"), Step1.Username, Step1.Password))
            {
                await PopupService.DisplayAlertAsync("Invalid credentials", "The credentials are invalid.", new[] {
                            new PopupAction {
                                Text = "OK"
                            }
                        });
            }
            else
            {
                var device = new Device()
                {
                    DisplayName = !string.IsNullOrWhiteSpace(Step1.Alias) ? Step1.Alias : Device.DisplayName,
                    IPAddress = Device.IPAddress,
                    Username = Step1.Username,
                    Password = Step1.Password
                };

                device = await DataService.AddOrUpdateDeviceAsync(device);

                MessageBus.Publish(new DeviceAdded(device.Id));

                if (Context.Device == null)
                {
                    await Context.SetDevice(device);
                }

                CleanUp();

                await Navigation.PopModal();
            }
        }));

        public ICommand BackCommand => backCommand ?? (backCommand = RelayCommand.Create(async () => await Navigation.GoBack()));

        public override void CleanUp()
        {
            Step1.PropertyChanged -= P1;

            Step1 = null;
        }

        public IPopupService PopupService { get; }
        public IDataService DataService { get; }
        public IDeviceNavigator Navigation { get; }
        public IConnectivity Connectivity { get; }

        private void P1(object sender, PropertyChangedEventArgs e) => finishStep1Command.RaiseCanExecuteChanged();
    }
}
