using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AudioPlayer.Api
{
	public class AudioControl : ApiBase
	{
		public AudioControl(Uri baseUri, NetworkCredential credentials)
		{
            Initialize(baseUri, credentials);

            RelativeApiUri = "/vapix/audiocontrol";
		}

		public async Task<Volume> GetVolumeAsync()
		{
			var result = await PostAsync<GetVolume>(RequestFactory.GetVolume);
			return result.Volume;
		}

        public Task SetVolumeAsync(int? foregroundVolume = null, string foregroundVolumeUnit = null, bool? foregroundVolumeMute = null, int? backgroundVolume = null, string backgroundVolumeUnit = null, bool? backgroundVolumeMute = null, CancellationToken cancellationToken = default) => PostAsync(RequestFactory.SetVolume(foregroundVolume, foregroundVolumeUnit, foregroundVolumeMute, backgroundVolume, backgroundVolumeUnit, backgroundVolumeMute), cancellationToken);

		#region Dtos

		public class GetVolume
   	 	{
			public Volume Volume { get; set; }
		}

		public class Volume
		{
			public bool BackgroundVolumeMute { get; set; }
			public double ForegroundVolume { get; set; }
			public double BackgroundVolume { get; set; }
			public string BackgroundVolumeUnit { get; set; }
			public bool ForegroundVolumeMute { get; set; }
			public string ForegroundVolumeUnit { get; set; }
		}

        #endregion

        private static class RequestFactory
		{
			public static string GetVolume = JsonConvert.SerializeObject(new
            {
                GetVolume = new {}
			});

            public static string SetVolume(int? foregroundVolume = null, string foregroundVolumeUnit = null, bool? foregroundVolumeMute = null, int? backgroundVolume = null, string backgroundVolumeUnit = null, bool? backgroundVolumeMute = null) => JsonConvert.SerializeObject(new {
				SetVolume = new {
					Volume = new {
						ForegroundVolume = foregroundVolume,
                        ForegroundVolumeUnit = foregroundVolume != null ? foregroundVolumeUnit ?? "dB" : null,
                        ForegroundVolumeMute = foregroundVolumeMute,
						BackgroundVolume = backgroundVolume,
                        BackgroundVolumeUnit = backgroundVolume != null ? backgroundVolumeUnit ?? "dB" : null,
                        BackgroundVolumeMute = backgroundVolumeMute
					}
				}
			}, new JsonSerializerSettings {
                NullValueHandling =  NullValueHandling.Ignore
            });
		}
	}
}
