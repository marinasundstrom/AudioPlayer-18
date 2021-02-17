using Xamarin.Forms;
using AudioPlayer.Views;
using System.Threading.Tasks;
using MvvmUtils;
using CommonServiceLocator;
using AudioPlayer.Services;

namespace AudioPlayer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoadingPage();

            Dispatcher.Current = new MyDispatcher();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            // Will not work on GTK

            await LoadContext();

            MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            // Will not work on GTK

            SaveContext().Wait();
        }

        protected override async void OnResume()
        {
            // Handle when your app resumes
            // Will not work on GTK

            await LoadContext();

            MainPage = new MainPage();
        }

        private Task LoadContext() => ServiceLocator.Current.GetInstance<IAppContext>().Initialize(true);

        private Task SaveContext() => ServiceLocator.Current.GetInstance<IAppContext>().Save();
    }
}
