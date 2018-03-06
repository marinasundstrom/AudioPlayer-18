using System;
using System.Threading.Tasks;
using Axis.AudioPlayer.Services;

namespace Axis.AudioPlayer
{
    internal class MyDispatcher : Dispatcher
    {
        public override Task BeginInvokeOnMainThread(Func<Task> action)
		{
            var tcs = new TaskCompletionSource<object>();
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => {
                try
                {
                    await action();
                    tcs.SetResult(null);
                } 
                catch (Exception e) 
                {
                    tcs.SetException(e);
                }
            });
            return tcs.Task;
		}
	}
}