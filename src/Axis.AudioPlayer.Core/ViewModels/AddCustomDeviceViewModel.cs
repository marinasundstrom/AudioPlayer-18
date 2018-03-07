using Axis.AudioPlayer.Services;
using MvvmUtils;
using Plugin.Connectivity.Abstractions;
using System.ComponentModel.DataAnnotations;
using MvvmUtils.DataAnnotations;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.ComponentModel;
using Axis.AudioPlayer.Messages;

namespace Axis.AudioPlayer.ViewModels
{
    public class AddCustomDeviceViewModel : AxisViewModelBase
    {
        private RelayCommand cancelCommand;
        private RelayCommand finishStep1Command;
        private RelayCommand backCommand;
        private RelayCommand finishStep2Command;

        private WizardCustom step1;
        private WizardSetAlias step2;

        public AddCustomDeviceViewModel(IAppContext context,
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

        public WizardCustom Step1
        {
            get => step1;
            set => SetAndValidateProperty(ref step1, value);
        }

        public WizardSetAlias Step2
        {
            get => step2;
            set => SetAndValidateProperty(ref step2, value);
        }

        public void Initialize()
        {
            Step1 = new WizardCustom();
            Step2 = new WizardSetAlias();

            Step1.PropertyChanged += P1;
            Step2.PropertyChanged += P2;

            //((RelayCommand)FinishStep1Command).RaiseCanExecuteChanged();
            //((RelayCommand)FinishStep2Command).RaiseCanExecuteChanged();
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
            else if (!await Connectivity.IsReachable($"http://{Step1.IPAddress}"))
            {
                await PopupService.DisplayAlertAsync("Host is not reachable", "Host is not recheable.", new[] {
                        new PopupAction {
                            Text = "OK"
                        }
                    });
            }
            else if (!await ConnectionUtils.TestConnectionAsync(new System.Uri($"http://{Step1.IPAddress}"), Step1.Username, Step1.Password))
            {
                await PopupService.DisplayAlertAsync("Invalid credentials", "The credentials are invalid.", new[] {
                            new PopupAction {
                                Text = "OK"
                            }
                        });
            }
            else
            {
                await Navigation.NavigateTo(Pages.AddDeviceCustomAlias);
            }
        }));

        public ICommand BackCommand => backCommand ?? (backCommand = RelayCommand.Create(async () => await Navigation.GoBack()));

        public ICommand FinishStep2Command => finishStep2Command ?? (finishStep2Command = RelayCommand.Create(async () =>
        {
            if (!Step2.Validate()) return;

            var device = new Data.Device()
            {
                DisplayName = Step2.Alias,
                // Product = string.Empty;
                IPAddress = Step1.IPAddress,
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
        }));

        public override void CleanUp()
        {
            Step1.PropertyChanged -= P1;
            Step2.PropertyChanged -= P2;

            Step1 = null;
            Step2 = null;
        }

        public IPopupService PopupService { get; }
        public IDataService DataService { get; }
        public IDeviceNavigator Navigation { get; }
        public IConnectivity Connectivity { get; }

        private void P1(object sender, PropertyChangedEventArgs e) => finishStep1Command.RaiseCanExecuteChanged();
        private void P2(object sender, PropertyChangedEventArgs e) => finishStep2Command.RaiseCanExecuteChanged();
    }

    public class WizardCustom : ValidatableObject
    {
        private string username;
        private string password;
        private string ipAddress;

        [Required(ErrorMessage = "Username is required")]
        public string Username
        {
            get => username;
            set => SetAndValidateProperty(ref username, value);

        }

        [Required(ErrorMessage = "Required")]
        [HostName("Invalid IP address or hostname")]
        public string IPAddress
        {
            get => ipAddress;
            set => SetAndValidateProperty(ref ipAddress, value);

        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public bool Validate()
        {
            this.ValidateAll();
            return !HasErrors;
        }
    }

    public class WizardSetAlias : ValidatableObject
    {
        private string alias;

        [Required]
        public string Alias
        {
            get => alias;
            set => SetAndValidateProperty(ref alias, value);
        }

        public bool Validate() 
        {
            this.ValidateAll();
            return !HasErrors;
        }
    }
}
