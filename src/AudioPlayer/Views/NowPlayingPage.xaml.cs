using System;
using AudioPlayer.Services;
using AudioPlayer.ViewModels;
using Xamarin.Forms;
using MvvmUtils.Reactive;

namespace AudioPlayer.Views
{
    public partial class NowPlayingPage : ContentPage
    {
		private IDisposable stateChanged;

		public NowPlayingPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                var viewModel = BindingContext as PlayerViewModel;

				stateChanged = viewModel.WhenAnyValue(x => x.State)
				                        .Subscribe((state) => {
					switch(state) 
					{
						case PlayerState.Playing:
							Image.ScaleTo(1);
							break;

						case PlayerState.Paused:
							Image.ScaleTo(0.8);
							break;
					}
				});

                var arg = this.GetNavigationArgs();
                if (arg != null)
                {
                    var (playlist, track) = ((string playlist, string track))arg;
					await viewModel.InitializeWithTrack(playlist, track);
                }
                else
                {
                    await viewModel.Initialize();
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Connection error", "Unable to load playlists.", "OK");
            }
        }

        protected override void OnDisappearing()
        {
			stateChanged?.Dispose();
            (BindingContext as PlayerViewModel).CleanUp();
        }

        private async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
