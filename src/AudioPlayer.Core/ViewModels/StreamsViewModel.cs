using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AudioPlayer.Services;
using MvvmUtils;

namespace AudioPlayer.ViewModels
{
    public class StreamsViewModel : AxisViewModelBase
    {
        private RelayCommand<Playlist> navigateToTrackCommand;
        private IEnumerable<Playlist> playlists;
        private Playlist selectedPlaylist;

        public StreamsViewModel(IAppContext context,
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

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        private RelayCommand refreshCommand;
        private bool isRefreshing;

        public ICommand RefreshCommand => refreshCommand ?? (refreshCommand = new RelayCommand(async () => {
            IsRefreshing = true;
            await Update();
            IsRefreshing = false;
        }));

        private async Task Update()
        {
            try
            {
                await Player.UpdatePlaylistsAsync();

                var playlists = GetStreamPlaylists();

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
                await PopupService.DisplayAlertAsync("Update failed", "Failed to reload streams.", new[] {
                            new PopupAction {
                                Text = "OK"
                            }
                        });
            }
        }

        private IEnumerable<Playlist> GetStreamPlaylists() =>
                Player.Playlists.GetStreamPlaylists();

        public IPlayerService Player { get; }

        public ICommand NavigateToTrackCommand => navigateToTrackCommand ?? (navigateToTrackCommand = RelayCommand.Create<Playlist>(async (Playlist playlist) =>
        {
            var track = playlist.Tracks[0];

            MessageBus.Publish(new PlayTrack(track.Id, playlist.Id));
            await Navigation.PushModal(Pages.NowPlaying);
        }));

        public IEnumerable<Playlist> Playlists
        {
            get => playlists;
            set => SetProperty(ref playlists, value);
        }

        public Playlist Playlist
        {
            get => selectedPlaylist;
            set => SetProperty(ref selectedPlaylist, value);
        }

        public IMusicNavigator Navigation { get; }
        public IPopupService PopupService { get; }
    }
}
