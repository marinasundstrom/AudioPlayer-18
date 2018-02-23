using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Axis.AudioPlayer.Api.Vapix
{
    public class AudioRelay : ApiBase
    {
        public AudioRelay(Uri baseUri, NetworkCredential credentials)
        {
            Initialize(baseUri, credentials);

            RelativeApiUri = "/vapix/audiorelay";
        }

        public Task SetSoundConfigurationAsync(int masterVolume) => PostAsync(RequestFactory.SetSoundConfiguration(masterVolume));

        public async Task<Configuration> GetSoundConfigurationAsync()
		{
			var result = await PostAsync<GetSoundConfiguration>(RequestFactory.GetSoundConfiguration);
			return result.Configuration;
		}

		#region Dto

		public class GetSoundConfiguration
		{
			public Configuration Configuration { get; set; }
		}

		public class Configuration
		{
			public bool MasterVolumeMute { get; set; }
			public long MasterVolume { get; set; }
			public string MasterVolumeUnit { get; set; }
		}

		#endregion

        private static class RequestFactory
        {
            public static string GetSoundConfiguration = JsonConvert.SerializeObject(new
            {
                GetSoundConfiguration = new {}
			});

            public static string SetSoundConfiguration(int? masterVolume = 0, string masterVolumeUnit = null, bool? mute = null) => JsonConvert.SerializeObject(new
            {
                SetSoundConfiguration = new
                {
                    Configuration = new
                    {
                        MasterVolume = masterVolume,
                        MasterVolumeUnit = masterVolumeUnit ?? "dB",
                        MasterVolumeMute = mute
                    }
                }
            });
        }
    }
}
