using System;
using System.Threading.Tasks;
using AudioPlayer;
using Xunit;

namespace AudioPlayerLib.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task ConnectAndFetchLibrary()
        {
            var audioPlayer = new AudioPlayer();
            audioPlayer.Initialize(new Uri("http://192.168.1.77"), "root", "pass");

            Assert.True(await audioPlayer.TestConnectionAsync());

            Assert.NotNull(audioPlayer.Library.Playlists);

            await audioPlayer.Library.FetchAllAsync();

            Assert.NotEmpty(audioPlayer.Library.Playlists);

        }
    }
}
