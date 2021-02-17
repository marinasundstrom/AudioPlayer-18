using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AudioPlayer.Api
{
    public class Player : ApiBase
	{
		public Player(Uri baseUri, NetworkCredential credentials)
		{
            Initialize(baseUri, credentials);

			RelativeApiUri = "/local/player/api";
		}

		public Task<Result> PlayAsync() => PostAsync<Result>(RequestFactory.Play());

		public Task<Result> PlayAsync(string playlistId, int trackIndex) => PostAsync<Result>(
            RequestFactory.PlayTrack(playlistId, trackIndex));

		public Task PauseAsync() => PostAsync(RequestFactory.Pause);

		public Task StopAsync() => PostAsync(RequestFactory.Stop);

		public Task<Result> PlayPreviousAsync() => PostAsync<Result>(RequestFactory.PlayPrevious);

		public Task<Result> PlayNextAsync() => PostAsync<Result>(RequestFactory.PlayNext);

		public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()
		{
			var result = await PostAsync<GetPlaylists>(RequestFactory.GetPlaylists);
			return result.Playlists;
		}

		public async Task<IEnumerable<Track>> GetTracksAsync(params string[] id)
		{
			var result = await PostAsync<GetTracks>(RequestFactory.GetTracks(id));
			return result.Tracks;
		}

		public Task<GetPlayerStatus> GetPlayerStatusAsync() => PostAsync<GetPlayerStatus>(
            RequestFactory.GetPlayerStatus);

        #region Dto

        public class Result
		{
			public NowPlaying NowPlaying { get; set; }
			public long TrackIndex { get; set; }
		}

		public class NowPlaying
		{
			public string Genre { get; set; }
			public string Artist { get; set; }
			public string Album { get; set; }
			public string Condition { get; set; }
			public string Title { get; set; }
			public string URI { get; set; }
			public string Id { get; set; }
			public string Type { get; set; }
			public string Year { get; set; }
		}

		public class GetPlayerStatus
		{
			public string PlaylistId { get; set; }
			public string TrackId { get; set; }
			public string Mode { get; set; }
			public object[] Storage { get; set; }
			public long TrackIndex { get; set; }
			public string Status { get; set; }
		}

		public class GetPlaylists
		{
			public Playlist[] Playlists { get; set; }
		}

		public class Playlist
		{
			public string Name { get; set; }
			public string Id { get; set; }
			public string[] ScheduleIds { get; set; }
			public string[] TrackIds { get; set; }
		}

		public class GetTracks
		{
			public Track[] Tracks { get; set; }
		}

		public class Track
		{
			public string Genre { get; set; }
			public string Artist { get; set; }
			public string Album { get; set; }
			public string Condition { get; set; }
			public string Title { get; set; }
			public string URI { get; set; }
			public string Id { get; set; }
			public string Type { get; set; }
			public string Year { get; set; }
		}

		#endregion

		private static class RequestFactory
		{
			private static string play;
			private static string stop;
			private static string getPlaylists;
			private static string getPlayerStatus;
			private static string playPrevious;
			private static string playNext;
			private static string pause;

			public static string Play() => play ?? (play = JsonConvert.SerializeObject(new
			{
				Play = new
				{

				}
			}));

			public static string PlayTrack(string playlistId, int trackIndex) => JsonConvert.SerializeObject(new
			{
				Play = new
				{
					PlaylistId = playlistId,
					TrackIndex = trackIndex
				}
			});

			public static string Stop = stop ?? (stop = JsonConvert.SerializeObject(new
			{
				Stop = new
				{

				}
			}));

			public static string Pause = pause ?? (pause = JsonConvert.SerializeObject(new
			{
				Pause = new
				{

				}
			}));

			public static string PlayPrevious = playPrevious ?? (playPrevious = JsonConvert.SerializeObject(new
			{
				PlayPrevious = new
				{

				}
			}));

			public static string PlayNext = playNext ?? (playNext = JsonConvert.SerializeObject(new
			{
				PlayNext = new
				{

				}
			}));

			public static string GetPlayerStatus = getPlayerStatus ?? (getPlayerStatus = JsonConvert.SerializeObject(new
			{
				GetPlayerStatus = new { }
			}));

			public static string GetPlaylists = getPlaylists ?? (getPlaylists = JsonConvert.SerializeObject(new
			{
				GetPlaylists = new { }
			}));

			public static string GetTracks(params string[] trackIds) => JsonConvert.SerializeObject(new
			{
				GetTracks = new
				{
					Ids = trackIds
				}
			});
		}
	}
}
