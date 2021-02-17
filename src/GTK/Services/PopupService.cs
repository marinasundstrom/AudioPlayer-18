using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioPlayer.Services
{
    public class PopupService : IPopupService
    {
        public Task<PopupAction> DisplayActionSheetAsync(string title, string message, IEnumerable<PopupAction> actions)
            => Task.FromResult<PopupAction>(null);

        public Task<PopupAction> DisplayAlertAsync(string title, string message, IEnumerable<PopupAction> actions)
            => Task.FromResult<PopupAction>(null);
    }
}
