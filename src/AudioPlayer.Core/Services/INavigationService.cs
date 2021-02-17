using System;
using System.Threading.Tasks;

namespace AudioPlayer.Services
{
	public interface INavigationService
    {
        void Configure(string pageKey, Type pageType);

        string CurrentPageKey { get; }
        int StackCount { get; }

        string CurrentModalPageKey { get; }
        int ModalStackCount { get; }

        Task NavigateTo(string pageKey);
        Task NavigateTo(string pageKey, object parameter);
        Task NavigateTo(string pageKey, object parameter, HistoryBehavior historyBehavior);
        Task Pop();

        Task GoBack();
        Task Home();

        Task PushModal(string pageKey);
        Task PushModal(string pageKey, object parameter);
        Task PopModal();
    }
}
