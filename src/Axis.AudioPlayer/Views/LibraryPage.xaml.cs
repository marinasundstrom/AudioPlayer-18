using System;
using Axis.AudioPlayer.Services;
using Axis.AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Views
{
	public partial class LibraryPage : ContentPage
    {
		public LibraryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
				await (BindingContext as LibraryViewModel).Initialize();
            }
            catch (Exception)
            {
                await DisplayAlert("Connection error", "Unable to load library.", "OK");
            }
        }
    }
}
