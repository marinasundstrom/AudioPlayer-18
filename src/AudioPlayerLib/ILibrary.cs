using System.Collections.Generic;
using System.Threading.Tasks;

namespace Axis.AudioPlayer
{
    public interface ILibrary : IEnumerable<Track>
    {
        IEnumerable<Track> Tracks { get; }
        IEnumerable<Playlist> Playlists { get; }
        Playlist LibraryPlaylist { get; }

        Playlist GetPlaylist(string id);
        Track GetTrack(string id);

        Task FetchAllAsync();
        Task FetchPlaylistsAsync();
        Task FetchPlaylistTracksAsync(Playlist p);

		Task SaveToFileAsync(string path);
		Task LoadFromFileAsync(string path);
    }
}