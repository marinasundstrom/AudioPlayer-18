using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Axis.AudioPlayer.Services;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
	public class LibraryViewModel : PlaylistViewModel
	{
		public LibraryViewModel(IAppContext context,
							IMessageBus messageBus,
							IMusicNavigator navigationService,
							IPopupService popupService)
			: base(context, messageBus, navigationService, popupService)
		{
			Context.DeviceChanged += Context_DeviceSet;
		}

		private async void Context_DeviceSet(object sender, System.EventArgs e)
		{
			await Update();
		}

		public Task Initialize()
		{
			return Update();
		}

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        private RelayCommand refreshCommand;
        private bool isRefreshing;

        public ICommand RefreshCommand => refreshCommand ?? (refreshCommand = new RelayCommand(async () => {
            IsRefreshing = true;
            await Update();
            IsRefreshing = false;
        }));

		private async Task Update()
		{
			try
			{
				await Player.UpdatePlaylistsAsync();

				var playlist = GetLibraryPlaylist();

				if (Playlist == null)
				{
					SetPlaylist(playlist);
				}
				else
				{
					if (!Playlist.Tracks.SequenceEqual(playlist.Tracks))
					{
						SetPlaylist(playlist);
					}
				}
			}
			catch (Exception)
			{
				// Show message: Update failed
                // Show message: Update failed
                await PopupService.DisplayAlertAsync("Update failed", "Failed to reload library.", new[] {
                            new PopupAction {
                                Text = "OK"
                            }
                        });
			}
		}

		private Playlist GetLibraryPlaylist() =>
			 Player
			.Playlists.GetLibraryPlaylist();
			
	}
}
