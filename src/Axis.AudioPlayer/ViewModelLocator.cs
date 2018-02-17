using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Axis.AudioPlayer.ViewModels;
using CommonServiceLocator;

namespace Axis.AudioPlayer
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<MainViewModel>().SingleInstance();

            var container = containerBuilder.Build();

            var autofacServiceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => autofacServiceLocator);
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
