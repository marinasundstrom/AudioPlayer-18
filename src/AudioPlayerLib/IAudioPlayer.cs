using System;
using System.Threading;
using System.Threading.Tasks;

namespace Axis.AudioPlayer
{
    public interface IAudioPlayer
    {
        PlayerState State { get; }
        bool IsPlaying { get; }
        bool IsStopped { get; }
        bool IsPaused { get; }
        Playlist Playlist { get; }
        Track Track { get; }
        long TrackIndex { get; }
        ILibrary Library { get; }

        event EventHandler StateChanged;
        event EventHandler TrackChanged;
        event EventHandler PlaylistChanged;
        event EventHandler MetadataUpdated;

        Task<(int volume, bool isMuted)> GetMusicVolumeAsync();
        Task NextAsync();
        Task PauseAsync();
        Task PlayAsync();
        Task PlayAsync(Playlist playlist, int trackIndex = 0);
        Task PreviousAsync();
        void Initialize(Uri baseUri, string username, string password);
        Task SetMusicVolumeAsync(int volume, CancellationToken cancellationToken = default);
        Task MuteMusicVolumeAsync(CancellationToken cancellationToken = default);
        Task UnmuteMusicVolumeAsync(CancellationToken cancellationToken = default);
        void StartWebSocket();
        void StopWebSocket();
        Task UpdateStateAsync();
    }
}
