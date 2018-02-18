using System;
using System.Globalization;

namespace Axis.AudioPlayer
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}
