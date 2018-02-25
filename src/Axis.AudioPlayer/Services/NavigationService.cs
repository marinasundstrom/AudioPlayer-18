using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Func<Page> resolver;
        private readonly bool resolveMainPageEachTime;
        private Page mainPage;

        private Dictionary<string, Type> pages { get; }
            = new Dictionary<string, Type>();

        public NavigationService(Func<Page> resolver, bool resolveMainPageEachTime = false)
        {
            this.resolver = resolver;
            this.resolveMainPageEachTime = resolveMainPageEachTime;
        }

        public Page MainPage => resolveMainPageEachTime ? resolver() : mainPage ?? (mainPage = resolver());

        public void SetMainPage(Page page) => mainPage = page;

        public void Configure(string pageKey, Type pageType)
        {
            pages.Add(pageKey, pageType);
        }

        public string CurrentPageKey => LookupPageKey(GetCurrentPageType());

        public string CurrentModalPageKey => LookupPageKey(GetCurrentModalPageType());

        public int StackCount => MainPage.Navigation.NavigationStack.Count;

        public int ModalStackCount => MainPage.Navigation.ModalStack.Count;

        public Task GoBack() => MainPage.Navigation.PopAsync();

        public Task Home() => MainPage.Navigation.PopToRootAsync();

        public Task NavigateTo(string pageKey) => NavigateTo(pageKey, null);

        public Task NavigateTo(string pageKey, object parameter) => NavigateTo(pageKey, parameter, HistoryBehavior.Default);

        public async Task NavigateTo(string pageKey, object parameter, HistoryBehavior historyBehavior)
        {
            if (pages.TryGetValue(pageKey, out var pageType))
            {
                var displayPage = (Page)Activator.CreateInstance(pageType);
                displayPage.SetNavigationArgs(parameter);

                if (historyBehavior == HistoryBehavior.ClearHistory)
                {
                    MainPage.Navigation.InsertPageBefore(displayPage,
                        MainPage.Navigation.NavigationStack[0]);

                    var existingPages = MainPage.Navigation.NavigationStack.ToList();
                    for (int i = 1; i < existingPages.Count; i++)
                        MainPage.Navigation.RemovePage(existingPages[i]);
                }
                else
                {
                    await MainPage.Navigation.PushAsync(displayPage);
                }
            }
            else
            {
                throw new ArgumentException($"No such page: {pageKey}.",
                    nameof(pageKey));
            }
        }

        public Task Pop() => MainPage.Navigation.PopAsync();

        public Task PopModal() => MainPage.Navigation.PopModalAsync();

        public Task PushModal(string pageKey)
        {
            return PushModal(pageKey, null);
        }

        public Task PushModal(string pageKey, object parameter)
        {
            if (pages.TryGetValue(pageKey, out var pageType))
            {
                var displayPage = (Page)Activator.CreateInstance(pageType);
                displayPage.SetNavigationArgs(parameter);

                return App.Current.MainPage.Navigation.PushModalAsync(displayPage);
            }
            else
            {
                throw new ArgumentException($"No such page: {pageKey}.",
                    nameof(pageKey));
            }
        }

        private string LookupPageKey(Type type) => pages.FirstOrDefault(x => x.Value == type).Key;

        private Type GetCurrentPageType() => MainPage.Navigation.NavigationStack.FirstOrDefault()?.GetType();

        private Type GetCurrentModalPageType() => MainPage.Navigation.ModalStack.FirstOrDefault()?.GetType();
    }
}
