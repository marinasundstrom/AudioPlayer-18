using Xamarin.Forms;
using Axis.AudioPlayer.Views;
using System.Threading.Tasks;

namespace Axis.AudioPlayer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoadingPage();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts

            await LoadContext();

            MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            // Will not work on macOS

            SaveContext().Wait();
        }

        protected override async void OnResume()
        {
            // Handle when your app resumes
            // Will not work on macOS

            await LoadContext();

            MainPage = new MainPage();
        }

        private Task LoadContext() => (BindingContext as AppContext).LoadContext();

        private Task SaveContext() => (BindingContext as AppContext).SaveContext();
    }
}
