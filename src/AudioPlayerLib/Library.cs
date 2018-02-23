using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Axis.AudioPlayer
{
    public class Library : ILibrary
    {
        private readonly Api.Player player;

        private List<Playlist> playlists;
        private Playlist libraryPlaylist;

        public Library(Api.Player player)
        {
            this.player = player;

            playlists = new List<Playlist>();
        }

        public IEnumerable<Track> Tracks => playlists.SelectMany(x => x.Tracks);

        public IEnumerable<Playlist> Playlists => playlists;

        public Playlist LibraryPlaylist => libraryPlaylist ?? (libraryPlaylist = playlists.First(x => x.Id == Playlist.LibraryPlaylistId));

        public async Task FetchAllAsync()
        {
            await FetchPlaylistsAsync();

            await FetchPlaylistTracksAsync(LibraryPlaylist);

            foreach (var playlist in Playlists.Except(new[] { LibraryPlaylist }))
            {
                await FetchPlaylistTracksAsync(playlist);
            }
        }

        public async Task FetchPlaylistsAsync()
        {
            var playlists = (await player.GetPlaylistsAsync()).ToList();

            if (playlists.Count == 0)
            {
                return;
            }

            // Remove playlists and tracks
            foreach (var p in this.playlists.ToArray())
            {
                if (!playlists.Any(x => x.Id == p.Id))
                {
                    this.playlists.Remove(p);
                }
            }

            foreach (var playlist in playlists)
            {
                var p = Playlists.FirstOrDefault(x => x.Id == playlist.Id);
                if (p == null)
                {
                    p = new Playlist
                    {
                        Tracks = new List<Track>()
                    };
                    this.playlists.Add(p);
                }

                p.Id = playlist.Id;
                p.Name = playlist.Name;
                p.TrackIds = playlist.TrackIds.ToArray();
            }
        }

        public async Task FetchPlaylistTracksAsync(Playlist p)
        {
            if (p.TrackIds.Length == 0)
            {
                return;
            }

            foreach (var track in p.Tracks.ToArray())
            {
                if (!p.TrackIds.Any(x => x == track.Id))
                {
                    p.Tracks.Remove(track);
                }
            }

            var tracks = await player.GetTracksAsync(p.TrackIds);

            int i = 0;
            foreach (var track in tracks)
            {
                var t = p.Tracks.FirstOrDefault(x => x.Id == track.Id);
                if (t == null)
                {
                    t = Tracks.FirstOrDefault(x => x.Id == track.Id);
                    if (t == null)
                    {
                        t = new Track();
                        if (track.Type != Track.TYPE_HTTP && track.Type != Track.TYPE_LINEIN)
                        {
                            LibraryPlaylist.Tracks.Add(t);
                        }
                    }
                    if (p != libraryPlaylist)
                    {
                        p.Tracks.Insert(i++, t);
                    }
                }

                SetTrackInfo(track, t);
            }
        }

        private static void SetTrackInfo(Api.Player.Track track, Track t)
        {
            t.Id = track.Id;
            t.Title = string.IsNullOrEmpty(track.Title) ? "Untitled track" : track.Title;
            t.Artist = string.IsNullOrEmpty(track.Artist) ? "Unknown artist" : track.Artist;
            t.Album = string.IsNullOrEmpty(track.Album) ? "Untitled album" : track.Album;
            t.Type = track.Type;
        }

        public Track GetTrack(string id) => Tracks.FirstOrDefault(x => x.Id == id);

        public Playlist GetPlaylist(string id) => Playlists.FirstOrDefault(x => x.Id == id);

        public IEnumerator<Track> GetEnumerator() => Tracks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public async Task SaveToFileAsync(string path)
		{
			var str = JsonConvert.SerializeObject(playlists);
			File.WriteAllText(path, str);
		}

		public async Task LoadFromFileAsync(string path)
		{
			var str = File.ReadAllText(path);
			playlists = JsonConvert.DeserializeObject<List<Playlist>>(str);
			libraryPlaylist = playlists.First(x => x.Id == Playlist.LibraryPlaylistId);
		}
	}
}
