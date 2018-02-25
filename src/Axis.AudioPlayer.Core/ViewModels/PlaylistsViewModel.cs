using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Axis.AudioPlayer.Services;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
	public class PlaylistsViewModel : AxisViewModelBase
    {
        private RelayCommand<Playlist> navigateToPlaylistCommand;
        private IEnumerable<Playlist> playlists;

		public PlaylistsViewModel(IAppContext context,
							 IMessageBus messageBus,
							 IMusicNavigator navigationService,
							 IPopupService popupService)
			: base(context, messageBus)
		{
			Navigation = navigationService;
			PopupService = popupService;
			Player = context.Player;

			Context.DeviceChanged += Context_DeviceSet;
		}

		private async void Context_DeviceSet(object sender, System.EventArgs e)
		{
			await Update();
		}

        public Task Initialize()
		{
			return Update();
		}

		private async Task Update()
		{
			try
			{
				await Player.UpdatePlaylistsAsync();

				var playlists = GetPlaylists();

				if (Playlists == null)
				{
					Playlists = playlists;
				}
				else
				{
					if (!Playlists.SequenceEqual(playlists))
					{
						Playlists = playlists;
					}
				}
			}
			catch (Exception)
			{
				// Show message: Update failed
				throw;
			}
		}

		private IEnumerable<Playlist> GetPlaylists() => Player.Playlists.GetAllProperPlaylists();

        public IEnumerable<Playlist> Playlists
        {
            get => playlists;
            set => SetProperty(ref playlists, value);
        }

		public IPlayerService Player { get; }

        public ICommand NavigateToPlaylistCommand => navigateToPlaylistCommand ?? (navigateToPlaylistCommand = RelayCommand.Create((Playlist playlist) => Navigation.NavigateTo(Pages.Playlist, playlist.Id)));

        public IMusicNavigator Navigation { get; }
        public IPopupService PopupService { get; }

		~PlaylistsViewModel() 
		{
			Context.DeviceChanged -= Context_DeviceSet;
		}
    }
}
