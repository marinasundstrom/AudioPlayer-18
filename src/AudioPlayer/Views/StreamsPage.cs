using System;
using AudioPlayer.Services;
using AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace AudioPlayer.Views
{
    public partial class StreamsPage : ContentPage
    {
        public StreamsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                var arg = this.GetNavigationArgs() as string;
                await (BindingContext as StreamsViewModel).Initialize();
            }
            catch (Exception)
            {
                await DisplayAlert("Connection error", "Unable to load streams.", "OK");
            }
        }
    }
}
