using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Axis.AudioPlayer.Services
{
    public class PopupService : IPopupService
    {
        public Task<PopupAction> DisplayActionSheetAsync(string title, string message, IEnumerable<PopupAction> actions)
        {
            throw new NotImplementedException();
        }

        public Task<PopupAction> DisplayAlertAsync(string title, string message, IEnumerable<PopupAction> actions)
        {
            throw new NotImplementedException();
        }
    }
}
