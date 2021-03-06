using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Plugin.CurrentActivity;

namespace AudioPlayer.Services
{
    public class PopupService : IPopupService
    {
        public Task<PopupAction> DisplayActionSheetAsync(string title, string message, IEnumerable<PopupAction> actions)
        {
            throw new NotImplementedException();
        }

        public Task<PopupAction> DisplayAlertAsync(string title, string message, IEnumerable<PopupAction> actions)
        {
            var tcs = new TaskCompletionSource<PopupAction>();
            var currentActivity = CrossCurrentActivity.Current.Activity;
            var builder = new AlertDialog.Builder(currentActivity);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetCancelable(actions.Any(action => action.IsCancel));
            int index = 0;
            foreach (var action in actions)
            {
                if (action.IsDefault && action.IsCancel)
                    throw new InvalidOperationException();

                if (index == 0 && !action.IsCancel)
                {
                    builder.SetPositiveButton(action.Text, (s, arg) =>
                    {
                        try
                        {
                            action?.Command?.Execute(action.CommandParameter);
                            tcs.TrySetResult(action);
                        }
                        catch (Exception e)
                        {
                            tcs.SetException(e);
                        }
                    });
                }
                else if (index == 1 && !action.IsCancel)
                {
                    builder.SetNegativeButton(action.Text, (s, arg) =>
                    {
                        try
                        {
                            action?.Command?.Execute(action.CommandParameter);
                            tcs.TrySetResult(action);
                        }
                        catch (Exception e)
                        {
                            tcs.SetException(e);
                        }
                    });
                }
                else if (action.IsCancel)
                {
                    builder.SetNeutralButton(action.Text, (s, arg) =>
                    {       
                        try
                        {
                            action?.Command?.Execute(action.CommandParameter);
                            tcs.TrySetResult(action);
                        }
                        catch (Exception e)
                        {
                            tcs.SetException(e);
                        }
                    });
                }
                index++;
            }
            builder.Show();
            return tcs.Task;
        }
    }
}
