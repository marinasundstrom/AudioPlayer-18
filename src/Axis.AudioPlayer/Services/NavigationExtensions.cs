using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Services
{
    public static class NavigationExtensions
    {
        private static readonly ConditionalWeakTable<Page, object> arguments
            = new ConditionalWeakTable<Page, object>();

        public static object GetNavigationArgs(this Page page)
        {
            arguments.TryGetValue(page, out var argument);
            return argument;
        }

        public static void SetNavigationArgs(this Page page, object args)
            => arguments.Add(page, args);
    }
}
