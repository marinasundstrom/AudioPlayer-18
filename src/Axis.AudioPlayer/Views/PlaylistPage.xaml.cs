using System;
using Axis.AudioPlayer.Services;
using Axis.AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Views
{
    public partial class PlaylistPage : ContentPage
    {
        public PlaylistPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                var arg = this.GetNavigationArgs() as string;
                await (BindingContext as PlaylistViewModel).Initialize(arg);
            }
            catch (Exception)
            {
                await DisplayAlert("Connection error", "Unable to load playlist.", "OK");
            }
        }
    }
}
