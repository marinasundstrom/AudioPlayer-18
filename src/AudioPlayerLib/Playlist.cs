using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer
{
    public class Playlist
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<Track> Tracks { get; set; }

        public Track GetTrack(string id) => Tracks.First(x => x.Id == id);

		public string[] TrackIds { get; set; }

        public const string LibraryPlaylistId = "416c6c55-706c-6f61-6465-6446696c6500";

        public const string LineInPlaylistId = "4c494e45-494e-504c-4159-4c4953542000";

        public override string ToString() => Name;
    }
}
