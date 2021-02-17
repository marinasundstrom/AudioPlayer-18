using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AudioPlayer.Converters
{
    public class PlayButtonStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is PlayerState playState)
            {
                switch(playState)
                {
                    case PlayerState.Playing:
                        return "pause_50.png";

                    case PlayerState.Paused:
                        return "play_50.png";

                    case PlayerState.Stopped:
                        return "play_50.png";
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
