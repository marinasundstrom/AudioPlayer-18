using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Axis.AudioPlayer.Services;
using Axis.AudioPlayer.ViewModels;
using CommonServiceLocator;
using MvvmUtils;

namespace Axis.AudioPlayer
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<AppContext>()
                            .SingleInstance();

            containerBuilder.RegisterType<MessageBus>()
                            .As<IMessageBus>()
                            .SingleInstance();

            containerBuilder.RegisterType<MainViewModel>()
                            .SingleInstance();
            
            containerBuilder.RegisterType<DataService>()
                            .As<IDataService>()
                            .InstancePerDependency();

            var container = containerBuilder.Build();

            var autofacServiceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => autofacServiceLocator);
        }

        public AppContext AppContext => ServiceLocator.Current.GetInstance<AppContext>();

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
