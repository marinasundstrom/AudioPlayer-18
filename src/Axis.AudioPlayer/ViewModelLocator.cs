using System.Reflection;
using System.Resources;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Axis.AudioPlayer.Resources;
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
                            .As<IAppContext>()
                            .SingleInstance();

            containerBuilder.RegisterType<MessageBus>()
                            .As<IMessageBus>()
                            .SingleInstance();

            containerBuilder.RegisterType<MainViewModel>()
                            .SingleInstance();

            if (DesignerLibrary.IsInDesignMode)
            {
                // TBA
            }
            else
            {
                containerBuilder.RegisterType<DataService>()
                                .As<IDataService>()
                                .InstancePerDependency();
            }

            containerBuilder.Register<IResourceContainer>(ctx => new ResourceContainer(new ResourceManager(ResourceContainer.ResourceId, typeof(AppResources).GetTypeInfo().Assembly), new Localize()));

            var container = containerBuilder.Build();

            var autofacServiceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => autofacServiceLocator);
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
