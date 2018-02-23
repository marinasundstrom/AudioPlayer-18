using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace Axis.AudioPlayer
{
    public class AudioPlayer : IAudioPlayer
    {
        private Api.Player player;
        private Api.AudioControl audioControl;

        private WebSocket ws;
        internal Uri baseUri;
        internal string username;
        internal string password;

        public AudioPlayer()
        {

        }

        public void Initialize(Uri baseUri, string username, string password)
        {
            this.baseUri = baseUri;
            this.username = username;
            this.password = password;

            player = new Api.Player(baseUri, new NetworkCredential(username, password));
            audioControl = new Api.AudioControl(baseUri, new NetworkCredential(username, password));

            Library = new Library(player);
        }

        public async Task UpdateStateAsync()
        {
            var playerState = await player.GetPlayerStatusAsync();

            Playlist = Library.GetPlaylist(playerState.PlaylistId);
            Track = Playlist?.GetTrack(playerState.TrackId);
            TrackIndex = playerState.TrackIndex;

            var status = playerState.Status;

            IsPlaying = status == "Playing";
            IsPaused = status == "Paused";
            IsStopped = status == "Stopped";

            TrackChanged?.Invoke(this, EventArgs.Empty);
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public PlayerState State
        {
            get
            {
                if (IsPlaying)
                    return PlayerState.Playing;
                if (IsStopped)
                    return PlayerState.Stopped;
                return PlayerState.Paused;
            }
        }

        public bool IsPlaying { get; private set; }

        public bool IsStopped { get; private set; }

        public bool IsPaused { get; private set; }

        public Playlist Playlist { get; private set; }

        public Track Track { get; private set; }

        public long TrackIndex { get; private set; }

        public event EventHandler StateChanged;

        public event EventHandler MetadataUpdated;

        public event EventHandler TrackChanged;

        public event EventHandler PlaylistChanged;

        public void StartWebSocket()
        {
            ws = new WebSocket($"ws://{baseUri.Host}/local/player/ws/");
            ws.OnMessage += OnWsData;
            ws.SetCredentials(username, password, false);
            ws.Connect();
        }

        public async Task<(int volume, bool isMuted)> GetMusicVolumeAsync()
        {
            var volumeInfo = await audioControl.GetVolumeAsync();
            return ((int)volumeInfo.BackgroundVolume, volumeInfo.BackgroundVolumeMute);
        }

        public Task SetMusicVolumeAsync(int volume, CancellationToken cancellationToken = default) =>
            audioControl.SetVolumeAsync(backgroundVolume: volume, cancellationToken: cancellationToken);

        public Task MuteMusicVolumeAsync(CancellationToken cancellationToken = default) =>
            audioControl.SetVolumeAsync(backgroundVolumeMute: true, cancellationToken: cancellationToken);

        public Task UnmuteMusicVolumeAsync(CancellationToken cancellationToken = default) =>
            audioControl.SetVolumeAsync(backgroundVolumeMute: false, cancellationToken: cancellationToken);

        private void OnWsData(object sender, MessageEventArgs e)
        {
            var obj = JObject.Parse(e.Data);

            //Console.WriteLine(obj.ToString(Formatting.Indented));

            if (obj.TryGetValue("AAP_WS_STATUS", out var stateObj))
            {
                var status = stateObj.Value<string>("status");
                IsPlaying = status == "Playing";
                IsPaused = status == "Paused";
                IsStopped = status == "Stopped";

                StateChanged?.Invoke(this, EventArgs.Empty);
            }
            else if (obj.TryGetValue("AAP_WS_TRACK_ID", out var trackIdObj))
            {
                var trackId = trackIdObj.Value<string>("TrackId");

                Track = Library.GetTrack(trackId);
            }
            else if (obj.TryGetValue("AAP_WS_TRACK_INDEX", out var trackIndexObj))
            {
                TrackIndex = int.Parse(trackIndexObj.Value<string>("TrackIndex"));

				TrackChanged?.Invoke(this, EventArgs.Empty);
            }
            else if (obj.TryGetValue("AAP_WS_PLAYLIST_ID", out var playlistObj))
            {
                var playlistId = playlistObj.Value<string>("PlaylistId");

                Playlist = Library.GetPlaylist(playlistId);

                PlaylistChanged?.Invoke(this, EventArgs.Empty);
            }
            else if (obj.TryGetValue("AAP_WS_METADATA_TITLE", out var metadataTitleObj))
            {
                Track.Title = metadataTitleObj.Value<string>("title");

                MetadataUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public Task NextAsync() => player.PlayNextAsync();

        public Task PreviousAsync() => player.PlayPreviousAsync();

        public ILibrary Library { get; private set; }

        public void StopWebSocket() => ws?.Close();

        public Task PlayAsync() => player.PlayAsync();

        public async Task PlayAsync(Playlist playlist, int trackIndex = 0)
        {
            await player.PlayAsync(playlist.Id, trackIndex);

            Playlist = playlist;
            Track = playlist.Tracks[trackIndex];
            TrackIndex = trackIndex;
        }

        public Task PauseAsync() => player.PauseAsync();
    }
}
