using System.Linq;
using Xamarin.Forms;
using AudioPlayer.Views;

namespace AudioPlayer.Services
{
	public class MusicNavigator : NavigationService, IMusicNavigator
	{
		public MusicNavigator()
			: base(() => ((Application.Current.MainPage as MasterDetailPage)
							.Detail as TabbedPage)
							.Children
						   .OfType<NavigationPage>()
						   .First(x => x.Title == "Music"), true)
		{
			Configure(Pages.NowPlaying, typeof(NowPlayingPage));
			Configure(Pages.Playlist, typeof(PlaylistPage));
			Configure(Pages.Playlists, typeof(PlaylistsPage));
			Configure(Pages.Music, typeof(MusicPage));
			Configure(Pages.Library, typeof(LibraryPage));
			Configure(Pages.Streams, typeof(StreamsPage));
			Configure(Pages.DeviceWizard, typeof(DeviceWizardPage));
		}
	}
}
