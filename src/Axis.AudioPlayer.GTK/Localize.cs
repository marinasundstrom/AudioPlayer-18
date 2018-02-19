using System;
using System.Globalization;
using System.Threading;

namespace Axis.AudioPlayer
{
    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
