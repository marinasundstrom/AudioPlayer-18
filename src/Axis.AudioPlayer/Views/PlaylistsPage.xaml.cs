using System;
using Axis.AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Views
{
    public partial class PlaylistsPage : ContentPage
    {
        public PlaylistsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                await (BindingContext as PlaylistsViewModel).Initialize();
            }
            catch (Exception)
            {
                await DisplayAlert("Connection error", "Unable to load playlists.", "OK");
            }
        }
    }
}
