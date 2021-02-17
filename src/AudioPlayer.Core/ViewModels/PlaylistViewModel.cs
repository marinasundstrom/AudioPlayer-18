using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioPlayer.Services;
using MvvmUtils;

namespace AudioPlayer.ViewModels
{
	public class PlaylistViewModel : AxisViewModelBase
	{
		private RelayCommand<Track> navigateToTrackCommand;
		private Playlist playlist;
		private IEnumerable<Track> tracks;
		private Track selectedTrack;

		public PlaylistViewModel(IAppContext context,
							 IMessageBus messageBus,
							 IMusicNavigator navigationService,
							 IPopupService popupService)
			: base(context, messageBus)
		{
			Navigation = navigationService;
			PopupService = popupService;
			Player = context.Player;
		}

		public async Task Initialize(string playlistId)
		{
			var playlist = Player.Playlists.First(p => p.Id == playlistId);

			SetPlaylist(playlist);

			try
			{
				playlist = Player.Playlists.First(p => p.Id == playlistId);

				await Player.UpdatePlaylistTracksAsync(playlist);

				SetPlaylist(playlist);
			}
			catch (Exception)
			{
				// Show message: Update failed
				throw;
			}
		}

		protected void SetPlaylist(Playlist playlist)
		{
			Playlist = playlist;
			Tracks = Playlist.Tracks.Where(x => !x.IsLineIn());
			SelectedTrack = Player.Track;
		}

		public IPlayerService Player { get; }

		public ICommand NavigateToTrackCommand => navigateToTrackCommand ?? (navigateToTrackCommand = RelayCommand.Create(async (Track track) =>
		{
			MessageBus.Publish(new PlayTrack(track.Id, Playlist.Id));
			await Navigation.PushModal(Pages.NowPlaying);
		}));

		public Playlist Playlist
		{
			get => playlist;
			set => SetProperty(ref playlist, value);
		}

		public IEnumerable<Track> Tracks
		{
			get => tracks;
			set => SetProperty(ref tracks, value);
		}

		public Track SelectedTrack
		{
			get => selectedTrack;
			set => SetProperty(ref selectedTrack, value);
		}

		public IMusicNavigator Navigation { get; }
		public IPopupService PopupService { get; }
	}
}
