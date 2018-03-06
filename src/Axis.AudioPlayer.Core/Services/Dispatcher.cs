using System;
using System.Threading.Tasks;

namespace Axis.AudioPlayer.Services
{
    public abstract class Dispatcher
    {
        public static Dispatcher Current { get; set; }

        public abstract Task BeginInvokeOnMainThread(Func<Task> action);
    }
}
