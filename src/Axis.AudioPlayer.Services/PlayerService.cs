using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Axis.AudioPlayer.Services
{
    public class PlayerService : IPlayerService
    {
        private IAudioPlayer player;
		private Uri baseUri;

		public event EventHandler StateChanged;
		public event EventHandler TrackChanged;
		public event EventHandler MetadataUpdated;

		public PlayerService()
        {
            player = new AudioPlayer();

			player.StateChanged += (s, e) =>
			{
				StateChanged?.Invoke(s, e);
			};

			player.TrackChanged += (s, e) =>
			{
				TrackChanged?.Invoke(s, e);
			};

			player.MetadataUpdated += (s, e) =>
			{
				MetadataUpdated?.Invoke(s, e);
			};
        }

        public async Task InitializeAsync()
        {
			// Necessary or rename?
        }

		public async Task StopService() 
		{
			player.StopWebSocket();
		}

		public async Task Setup(Uri baseUri, string username, string password)
		{
			this.baseUri = baseUri;

            player.Initialize(baseUri, username, password);
		}

		public async Task InitiateAsync()
		{
			player.StartWebSocket();
		}

		public async Task PreloadAsync()
		{
			await LoadAsync();
		}

		public async Task UpdateAsync()
		{
            await player.Library.FetchAllAsync();
			await player.UpdateStateAsync();
			await SaveAsync();
		}

		public PlayerState State => player.State;

		public bool IsPlaying => player.IsPlaying;

		public bool IsPaused => player.IsPaused;

		public bool IsStopped => player.IsStopped;

		public async Task UpdatePlaylistsAsync() 
		{
            await player.Library.FetchPlaylistsAsync();
		}

		public async Task UpdatePlaylistTracksAsync(Playlist playlist) 
		{
            await player.Library.FetchPlaylistTracksAsync(playlist);
			await SaveAsync();
		}

		public IEnumerable<Playlist> Playlists => player.Library.Playlists;

		public Playlist Playlist => player.Playlist;

		public Track Track => player.Track;

		public long TrackIndex => player.TrackIndex;

		public Track GetTrack(string id)
		{
			return player.Library.GetTrack(id);
		}

		public Playlist GetPlaylist(string id)
		{
			return player.Library.GetPlaylist(id);
		}

		public async Task LoadAsync()
		{
			var path = CacheFilePath();
			if (File.Exists(path))
			{
                await player.Library.LoadFromFileAsync(path);
			}
		}

		public async Task SaveAsync()
		{
			var path = CacheFilePath();
            await player.Library.SaveToFileAsync(path);
		}

		// Cache
		// LibraryPlaylist
		// Playlists
		// Streams

		private string CacheFilePath() 
		{
			var file = $"library-{baseUri.Host.Replace(".", "_")}.json";
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			return Path.Combine(path, file);
		}

		public Task NextAsync()
		{
			return player.NextAsync();
		}

		public Task PauseAsync()
		{
			return player.PauseAsync();
		}

		public Task PlayAsync()
		{
			return player.PlayAsync();
		}

		public Task PlayAsync(Playlist playlist, int trackIndex = 0)
		{
			return player.PlayAsync(playlist, trackIndex);
		}

		public Task PreviousAsync()
		{
			return player.PreviousAsync();
		}

		public Task SetMusicVolumeAsync(int volume, CancellationToken cancellationToken = default(CancellationToken))
		{
			return player.SetMusicVolumeAsync(volume, cancellationToken);
		}

		public Task MuteMusicVolumeAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return player.MuteMusicVolumeAsync(cancellationToken);
		}

		public Task UnmuteMusicVolumeAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return player.UnmuteMusicVolumeAsync(cancellationToken);
		}

		public Task UpdateStateAsync()
		{
			return player.UpdateStateAsync();
		}

		public Task<(int volume, bool isMuted)> GetMusicVolumeAsync()
		{
			return player.GetMusicVolumeAsync();
		}
	}
}
