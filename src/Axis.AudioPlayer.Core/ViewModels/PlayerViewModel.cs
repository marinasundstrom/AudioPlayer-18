using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Axis.AudioPlayer.Services;
using MvvmUtils.Reactive;
using System.Reactive.Linq;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
	public class PlayerViewModel : AxisViewModelBase
	{
		private bool canGoBack = false;
		private bool canGoNext = false;

		private RelayCommand buttonCommand;
		private RelayCommand previousCommand;
		private RelayCommand nextCommand;
		private RelayCommand muteCommand;
		private RelayCommand _resetMuteCommand;

		private bool isInitialized;

		private Track track;
		private string trackTitle;
		private string device;
		private Playlist playlist;
		private static int musicVolume;
		private static bool isMuted;
		private bool notLineIn;

		private PlayerState state;
		private PlayerState stateInternal;

		public PlayerViewModel(IAppContext context,
							 IMessageBus messageBus,
							 IMusicNavigator navigationService,
							 IPopupService popupService)
			: base(context, messageBus)
		{
			Navigation = navigationService;
			PopupService = popupService;
			Player = context.Player;

			MessageBus.WhenPublished<PlayTrack>().Subscribe(OnPlayTrack);
			MessageBus.WhenPublished<UpdatePlayer>().Subscribe(OnUpdatePlayer);

			Context.DeviceChanged += Context_DeviceSet;

			IsInitialized = true;

			// Set Player state
			this.WhenAnyValue(x => x.StateInternal)
				.Where(x => x != PlayerState.Stopped)
				.SetProperty(this, x => x.State);

			// Set Music Volume
			this.ObservableForProperty(x => x.MusicVolume)
				.Throttle(TimeSpan.FromMilliseconds(100))
				.Subscribe(async volume =>
				{
					await Player.SetMusicVolumeAsync(volume);

					if (IsMuted)
					{
						await Player.UnmuteMusicVolumeAsync();
						IsMuted = false;
					}
				});
		}

		private async void Context_DeviceSet(object sender, System.EventArgs e)
		{
			UnsubscribeToPlayerEvents();
			await UpdatePlayer();
			SubscribeToPlayerEvents();
		}

		private async void OnPlayTrack(PlayTrack message)
		{
			await InitializeWithTrack(message.PlaylistId, message.TrackId);
		}

		private async void OnUpdatePlayer(UpdatePlayer message)
		{
			if (message.Fetch)
			{
				await Player.InitiateAsync();
				await Player.UpdateStateAsync();
			}

			await UpdatePlayer(true);
		}

		private void SubscribeToPlayerEvents()
		{
			Player.StateChanged += PlayerService_StateChanged;
			Player.TrackChanged += PlayerService_TrackChanged;
			Player.MetadataUpdated += PlayerService_MetadataUpdated;
		}

		private void UnsubscribeToPlayerEvents()
		{
			Player.StateChanged -= PlayerService_StateChanged;
			Player.TrackChanged -= PlayerService_TrackChanged;
			Player.MetadataUpdated -= PlayerService_MetadataUpdated;
		}

		private void PlayerService_MetadataUpdated(object sender, EventArgs e) => UpdateTrackTitle();

		private void PlayerService_StateChanged(object sender, EventArgs e) => UpdatePlayerControlState();

		private async void PlayerService_TrackChanged(object sender, EventArgs e) => await UpdateTrackState();

		public async Task InitializeWithTrack(string playlist, string track) => await PlayTrack(playlist, track);

		public async Task Initialize()
		{
            
		}

		private async Task UpdatePlayer(bool direct = false)
		{
			await UpdateVolumeState();
			await UpdateTrackState();
		}

		private async Task UpdateVolumeState()
		{
			var volume = await Player.GetMusicVolumeAsync();

			MusicVolume = volume.volume;
			IsMuted = volume.isMuted;
		}

		private async Task UpdateTrackState()
		{
			if (Player.Track != null)
			{
				Track = Player.Track;
				Playlist = Player.Playlist;

				NotLineIn = !Track.IsLineIn() && !Track.IsStream();
				UpdateTrackTitle();

				UpdatePlayerControlState();
			}
		}

		private void UpdatePlayerControlState()
		{
			StateInternal = Player.State;

			CanGoBack = Player.TrackIndex > 0;
			CanGoNext = Player.TrackIndex < Playlist.Tracks.Count - 1;
		}

		private void UpdateTrackTitle()
		{
			var trackTitle = Track.Title;
			if (trackTitle == Track.AIS_AD_BREAK || trackTitle == Track.STOP_AD_BREAK)
			{
				TrackTitle = string.Empty;
			}
			else
			{
				TrackTitle = trackTitle;
			}

			Device = Context.Device.DisplayName;
		}

		public bool IsInitialized
		{
			get => isInitialized;
			set => SetProperty(ref isInitialized, value);
		}

		public string Device
		{
			get => device;
			set => SetProperty(ref device, value);
		}

		public string TrackTitle
		{
			get => trackTitle;
			set => SetProperty(ref trackTitle, value);
		}

		public Track Track
		{
			get => track;
			set => SetProperty(ref track, value);
		}

		public Playlist Playlist
		{
			get => playlist;
			set => SetProperty(ref playlist, value);
		}

		public int MusicVolume
		{
			get => musicVolume;
			set => SetProperty(ref musicVolume, value);
		}

		public bool IsMuted
		{
			get => isMuted;
			set => SetProperty(ref isMuted, value);
		}

		public bool CanGoBack
		{
			get => canGoBack;
			set => SetProperty(ref canGoBack, value);
		}

		public bool CanGoNext
		{
			get => canGoNext;
			set => SetProperty(ref canGoNext, value);
		}

		private PlayerState StateInternal
		{
			get => stateInternal;
			set => SetProperty(ref stateInternal, value);
		}

		public PlayerState State
		{
			get => state;
			set => SetProperty(ref state, value);
		}

		public bool NotLineIn
		{
			get => notLineIn;
			set => SetProperty(ref notLineIn, value);
		}

		private async Task PlayTrack()
		{
			await Player.PlayAsync();
			if (Player.Track.IsStream())
			{
				await UpdateTrackState();
			}
		}

		private async Task PlayTrack(string playlistId, string trackId)
		{
			var playlist = Player.GetPlaylist(playlistId);

			var track = playlist.GetTrack(trackId);
			var trackIndex = playlist.Tracks.IndexOf(track);

			if (!IsCurrentTrack(playlist, trackIndex) || !Player.IsPlaying)
			{
				await Player.PlayAsync(playlist, trackIndex);
			}

			await UpdateTrackState();
		}

		private bool IsCurrentTrack(Playlist playlist, int trackIndex) => Player.Playlist == playlist && Player.TrackIndex == trackIndex;

		private async Task PauseTrack() => await Player.PauseAsync();

		private async Task UpdatePlaylistTracks()
		{
			if (Track == null)
				return;

			await Player.UpdatePlaylistTracksAsync(Playlist);
		}

		public ICommand PreviousCommand => previousCommand ?? (previousCommand = RelayCommand.Create(async () => await Player.PreviousAsync()));

		public ICommand NextCommand => nextCommand ?? (nextCommand = RelayCommand.Create(async () => await Player.NextAsync()));

		public ICommand ButtonCommand => buttonCommand ?? (buttonCommand = RelayCommand.Create(async () =>
		{
			switch (Player.State)
			{
				case PlayerState.Playing:
					await PauseTrack();
					break;

				case PlayerState.Stopped:
				case PlayerState.Paused:
					await PlayTrack();
					break;
			}
		}));

		public ICommand MuteCommand => muteCommand ?? (muteCommand = RelayCommand.Create(async () =>
		{
			if (IsMuted)
			{
				await Player.UnmuteMusicVolumeAsync();
			}
			else
			{
				await Player.MuteMusicVolumeAsync();
			}
			IsMuted = !IsMuted;
		}));

		public ICommand ResetMuteCommand => _resetMuteCommand ?? (_resetMuteCommand = RelayCommand.Create(async () =>
		{
			if (IsMuted)
			{
				await Player.UnmuteMusicVolumeAsync();
				IsMuted = false;
			}
		}));

		public IPlayerService Player { get; }
		public IMusicNavigator Navigation { get; }
		public IPopupService PopupService { get; }
	}
}
