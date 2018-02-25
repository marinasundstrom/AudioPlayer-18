using System.Reflection;
using System.Resources;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Axis.AudioPlayer.Resources;
using Axis.AudioPlayer.Services;
using Axis.AudioPlayer.ViewModels;
using CommonServiceLocator;
using MvvmUtils;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Axis.AudioPlayer
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<AppContext>()
                            .As<IAppContext>()
                            .SingleInstance();

            containerBuilder.RegisterType<MessageBus>()
                            .As<IMessageBus>()
                            .SingleInstance();

            containerBuilder.RegisterType<MainViewModel>().SingleInstance();
            containerBuilder.RegisterType<DashboardViewModel>().InstancePerDependency();
            containerBuilder.RegisterType<PlaylistViewModel>().InstancePerDependency();
            containerBuilder.RegisterType<PlaylistsViewModel>().SingleInstance();
            containerBuilder.RegisterType<PlayerViewModel>().SingleInstance();
            containerBuilder.RegisterType<MusicViewModel>().SingleInstance();
            containerBuilder.RegisterType<LibraryViewModel>().SingleInstance();
            containerBuilder.RegisterType<StreamsViewModel>().SingleInstance();
            containerBuilder.RegisterType<DevicesViewModel>().SingleInstance();
            containerBuilder.RegisterType<DeviceWizardViewModel>().SingleInstance();
            containerBuilder.RegisterType<SettingsViewModel>().InstancePerDependency();

            containerBuilder.RegisterType<PlayerService>()
                            .As<IPlayerService>()
                            .SingleInstance();
            
            containerBuilder.RegisterType<AppContext>()
                            .As<IAppContext>()
                            .SingleInstance();
            
            containerBuilder.RegisterType<DeviceServices>()
                            .As<IDeviceServices>()
                            .SingleInstance();
            
            containerBuilder.RegisterType<DeviceDiscoverer>()
                            .As<IDeviceDiscoverer>()
                            .SingleInstance();
            
            containerBuilder.RegisterType<PopupService>()
                            .As<IPopupService>()
                            .SingleInstance();

            containerBuilder.Register(c => CrossSettings.Current)
                            .As<ISettings>()
                            .SingleInstance();
            
            containerBuilder.Register(c => CrossConnectivity.Current)
                            .As<IConnectivity>()
                            .SingleInstance();

            containerBuilder.Register(c => new MusicNavigator())
                            .As<IMusicNavigator>()
                            .SingleInstance();
            
            containerBuilder.Register(c => new SettingsNavigator())
                            .As<ISettingsNavigator>()
                            .SingleInstance();
            
            containerBuilder.Register(c => new DeviceNavigator())
                            .As<IDeviceNavigator>()
                            .SingleInstance();

            if (DesignerLibrary.IsInDesignMode)
            {
                // TBA
            }
            else
            {
                containerBuilder.RegisterType<DataService>()
                                .As<IDataService>()
                                .SingleInstance();
            }

            containerBuilder.Register<IResourceContainer>(ctx => new ResourceContainer(new ResourceManager(ResourceContainer.ResourceId, typeof(AppResources).GetTypeInfo().Assembly), new Localize()));

            var container = containerBuilder.Build();

            var autofacServiceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => autofacServiceLocator);

            var player = Player;
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public DashboardViewModel Dashboard => ServiceLocator.Current.GetInstance<DashboardViewModel>();

        public PlayerViewModel Player => ServiceLocator.Current.GetInstance<PlayerViewModel>();

        public PlaylistsViewModel Playlists => ServiceLocator.Current.GetInstance<PlaylistsViewModel>();

        public PlaylistViewModel Playlist => ServiceLocator.Current.GetInstance<PlaylistViewModel>();

        public MusicViewModel Music => ServiceLocator.Current.GetInstance<MusicViewModel>();

        public LibraryViewModel Library => ServiceLocator.Current.GetInstance<LibraryViewModel>();

        public StreamsViewModel Streams => ServiceLocator.Current.GetInstance<StreamsViewModel>();

        public DevicesViewModel Devices => ServiceLocator.Current.GetInstance<DevicesViewModel>();

        public DeviceWizardViewModel DeviceDiscovery => ServiceLocator.Current.GetInstance<DeviceWizardViewModel>();

        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
