using System;
using System.Linq;
using System.Threading.Tasks;
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
				throw;
			}
		}

		private Playlist GetLibraryPlaylist() =>
			 Player
			.Playlists.GetLibraryPlaylist();
			
	}
}
