using System;
using AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace AudioPlayer.Views
{
    public partial class MusicPage : ContentPage
    {
        public MusicPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
               (BindingContext as MusicViewModel).Initialize();
            }
            catch (Exception)
            {
                await DisplayAlert("Connection error", "Unable to load playlists.", "OK");
            }
        }

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
		}
	}
}
