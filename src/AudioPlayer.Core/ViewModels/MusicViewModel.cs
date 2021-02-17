using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AudioPlayer.Services;
using MvvmUtils;

namespace AudioPlayer.ViewModels
{
    public class MusicViewModel : AxisViewModelBase
    {
        private const string LineIn = "line-in";
        private const string Library = "library";
        private const string Streams = "streams";
        private const string Playlists = "playlists";

        private RelayCommand<Item> navigateToItemCommand;
        private IEnumerable<Item> items;

        public MusicViewModel(IAppContext context,
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

        public void Initialize()
        {
            UpdateList();
        }

        private void Context_DeviceSet(object sender, System.EventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            var list = new List<Item>
            {
                new Item {
                    Id = Library,
                    Name = "Library"
                },
                new Item {
                    Id = Playlists,
                    Name = "Playlists"
                },
                new Item
                {
                    Id = Streams,
                    Name = "Streams"
                }
            };

            if (HasLineIn())
            {
                list.Add(new Item
                {
                    Id = LineIn,
                    Name = "Line-in"
                });
            }

            Items = list;
        }

        private bool HasLineIn()
        {
            return Player.Playlists.Any(x => x.IsLineIn());
        }

        private bool HasStreams()
        {
            return Player.Playlists.Any(x => x.IsStream());
        }

        public IEnumerable<Item> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

		public IPlayerService Player { get; }

        public ICommand NavigateToItemCommand => navigateToItemCommand ?? (navigateToItemCommand = new RelayCommand<Item>(async (Item item) =>
        {
            switch (item.Id)
            {
                case Library:
					await Navigation.NavigateTo(Pages.Library);
                    break;

                case Playlists:
                    await Navigation.NavigateTo(Pages.Playlists);
                    break;

                case Streams:
                    await Navigation.NavigateTo(Pages.Streams);
                    break;

                case LineIn:
					MessageBus.Publish(new PlayTrack(Track.LINEIN_ID, Playlist.LineInPlaylistId));
					await Navigation.PushModal(Pages.NowPlaying);
                    break;
            }
        }));

        public IMusicNavigator Navigation { get; }
        public IPopupService PopupService { get; }

        public override void CleanUp()
        {
            base.CleanUp();

            Context.DeviceChanged -= Context_DeviceSet;
        }
    }

    public class Item
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
    }
}
