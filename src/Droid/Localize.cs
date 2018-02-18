using System;
using System.Globalization;

namespace Axis.AudioPlayer
{
    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-"); // turns pt_BR into pt-BR
            return new CultureInfo(netLanguage);
        }

        public void SetLocale(CultureInfo ci)
        {
            throw new NotImplementedException();
        }
    }
}
