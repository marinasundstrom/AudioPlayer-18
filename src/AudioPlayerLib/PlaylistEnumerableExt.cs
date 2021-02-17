using System.Collections.Generic;
using System.Linq;

namespace AudioPlayer
{
    public static class PlaylistEnumerableExt
    {
		public static Playlist GetLibraryPlaylist(this IEnumerable<Playlist> playlists) =>
		playlists.First(x => x.Id == Playlist.LibraryPlaylistId);

        public static IEnumerable<Playlist> GetStreamPlaylists(this IEnumerable<Playlist> playlists) => playlists
                .Except(new[] { playlists.GetLibraryPlaylist() })
                .Where(IsStream);

        public static Playlist GetLineInPlaylist(this IEnumerable<Playlist> playlists) => playlists
               .FirstOrDefault(IsLineIn);

        /// <summary>
        /// Get all proper playlists (not library, streams nor line-in)
        /// </summary>
        /// <param name="playlists"></param>
        /// <returns></returns>
        public static IEnumerable<Playlist> GetAllProperPlaylists(this IEnumerable<Playlist> playlists) => playlists
            .Except(new[] { playlists.GetLibraryPlaylist() })
            .Except(playlists.GetStreamPlaylists())
            .Where(x => !x.IsLineIn());

        public static bool IsStream(this Playlist playlist) => playlist.Tracks.Any(track => track.IsStream());

        public static bool IsLineIn(this Playlist playlist) => playlist.Tracks.Any(track => track.IsLineIn());
    }
}
