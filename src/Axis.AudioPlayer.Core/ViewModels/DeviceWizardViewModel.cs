﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmUtils.Reactive;
using Axis.AudioPlayer.Services;
using MvvmUtils;
using Plugin.Connectivity.Abstractions;
using Axis.AudioPlayer.Messages;
using System.ComponentModel;
using CommonServiceLocator;

namespace Axis.AudioPlayer.ViewModels
{
    public class DeviceWizardViewModel : AxisViewModelBase
    {
        private RelayCommand selectDeviceCommand;
        private RelayCommand navigateToAddCustomDeviceCommand;
        private RelayCommand cancelCommand;
        private RelayCommand addDeviceCommand;

        private IDisposable subscription;
        private IDisposable subscription2;
        private IDisposable subscription3;

        private Device selectedDevice;
        private string searchText = String.Empty;

        public DeviceWizardViewModel(IAppContext context,
                                     IMessageBus messageBus,
                                     IDeviceNavigator navigationService,
                                     IDataService dataService,
                                     IPopupService popupService,
                                     IDeviceDiscoverer deviceDiscoverer,
                                     IConnectivity connectivity)
            : base(context, messageBus)
        {
            DataService = dataService;
            Navigation = navigationService;
            PopupService = popupService;
            DeviceDiscoverer = deviceDiscoverer;
            Connectivity = connectivity;

            var deviceComparer = Comparer<Device>.Create(deviceComparison);

            AllDevices = new ReactiveSortedCollection<Device>(deviceComparer);
            Devices = new ReactiveSortedCollection<Device>(deviceComparer);
        }

        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        public Comparison<Device> deviceComparison = new Comparison<Device>((d1, d2) => d1.DisplayName.CompareTo(d2.DisplayName));

        public ReactiveSortedCollection<Device> AllDevices { get; }

        public ReactiveSortedCollection<Device> Devices { get; }

        public Device SelectedDevice
        {
            get => selectedDevice;
            set => SetProperty(ref selectedDevice, value);
        }

        public ICommand SelectDeviceCommand => selectDeviceCommand ?? (selectDeviceCommand = RelayCommand.Create(async () =>
        {
            ServiceLocator.Current
                          .GetInstance<AddDeviceViewModel>()
                          .Initialize(SelectedDevice);
            
            await Navigation.NavigateTo(Pages.AddDevice);
        }));

        public ICommand NavigateToAddCustomDeviceCommand
        => navigateToAddCustomDeviceCommand ?? (navigateToAddCustomDeviceCommand = new RelayCommand(async () =>
        {
            ServiceLocator.Current
                          .GetInstance<AddCustomDeviceViewModel>()
                          .Initialize();
            
            await Navigation.NavigateTo(Pages.AddDeviceCustom);
        }));

        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = RelayCommand.Create(async () => await Navigation.PopModal()));

        private bool IsDeviceInFilter(Device d)
        {
            if(string.IsNullOrEmpty(SearchText)) 
            {
                return true;
            }
            return d.DisplayName.ToLower().Contains(SearchText.ToLower()) || d.IPAddress.ToLower().Contains(SearchText.ToLower());
        }

        public async Task Initialize()
        {
            var whenDeviceDiscovered = DeviceDiscoverer.WhenDeviceDiscovered;
            var whenSearchTextChanged = this.WhenAnyValue(vm => vm.SearchText);
            var whenItemAddedToAllDevices = AllDevices.WhenItemInserted;

            subscription = whenDeviceDiscovered
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Where(IsDeviceInFilter)
                .Subscribe((device) =>
                {
                    if (!AllDevices.Any(d => d.IPAddress == device.IPAddress))
                    {
                        AllDevices.Add(device);
                    }
                    if (IsDeviceInFilter(device))
                    {
                        if (!Devices.Any(d => d.IPAddress == device.IPAddress))
                        {
                            Devices.Add(device);
                        }
                    }
                });



            subscription2 = whenSearchTextChanged
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromMilliseconds(800))
                .Subscribe((searchText) =>
           {
               
           });


            DeviceDiscoverer.Start();

            SearchText = string.Empty;
        }

        public void Cleanup()
        {
            DeviceDiscoverer.Stop();
            subscription.Dispose();
            subscription2.Dispose();

            AllDevices.Clear();
            Devices.Clear();
        }

        public IDeviceDiscoverer DeviceDiscoverer { get; }
        public IPopupService PopupService { get; }
        public IDataService DataService { get; }
        public IDeviceNavigator Navigation { get; }
        public IConnectivity Connectivity { get; }
    }
}
