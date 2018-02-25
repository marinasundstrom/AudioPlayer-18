using System;
using System.Collections.Generic;
using Axis.AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Views
{
    public partial class DevicesPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public DevicesPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await (BindingContext as DevicesViewModel).InitializeAsync();
            }
            catch (Exception)
            {
                await DisplayAlert("Connection error", "Unable to load playlists.", "OK");
            }
        }
    }
}
