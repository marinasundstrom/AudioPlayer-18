using System;
namespace AudioPlayer
{
	public class PlayTrack
	{
		public PlayTrack(string trackId, string playlistId)
        {
            TrackId = trackId;
			PlaylistId = playlistId;
        }

		public string TrackId { get; }

		public string PlaylistId { get; }
	}
}
