using System;
using Axis.AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Views
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                await (BindingContext as DashboardViewModel).Initialize();
            }
            catch (Exception)
            {
                await DisplayAlert("Connection error", "Unable to load playlists.", "OK");
            }
        }
    }
}
