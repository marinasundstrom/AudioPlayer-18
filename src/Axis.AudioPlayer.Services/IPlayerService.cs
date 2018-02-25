using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Axis.AudioPlayer.Services
{
	public interface IPlayerService
	{
		Task InitializeAsync();
		Task StopService();

		Task Setup(Uri baseUri, string username, string password);
		Task PreloadAsync();
		Task UpdateAsync();
		Task InitiateAsync();

		Task UpdatePlaylistsAsync();
		Task UpdatePlaylistTracksAsync(Playlist playlist);

		IEnumerable<Playlist> Playlists { get; }
		Track GetTrack(string id);
		Playlist GetPlaylist(string id);

		Playlist Playlist { get; }
		Track Track { get; }
		long TrackIndex { get; }

		PlayerState State { get; }
		bool IsPlaying { get; }
		bool IsPaused { get; }
		bool IsStopped { get; }

		Task LoadAsync();
		Task SaveAsync();

		event EventHandler StateChanged;
		event EventHandler TrackChanged;
		event EventHandler MetadataUpdated;

		Task NextAsync();
		Task PauseAsync();
		Task PlayAsync();
		Task PlayAsync(Playlist playlist, int trackIndex = 0);
		Task PreviousAsync();
		Task SetMusicVolumeAsync(int volume, CancellationToken cancellationToken = default);
		Task MuteMusicVolumeAsync(CancellationToken cancellationToken = default);
		Task UnmuteMusicVolumeAsync(CancellationToken cancellationToken = default);
		Task UpdateStateAsync();
		Task<(int volume, bool isMuted)> GetMusicVolumeAsync();
	}
}