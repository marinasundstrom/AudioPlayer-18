using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Converters
{
    class MusicButtonIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string id)
            {
                switch (id)
                {
                    case "library":
                        return "music_library_30.png";

                    case "playlists":
                        return "playlist_30.png";

                    case "streams":
                        return "online_30.png";

                    case "line-in":
                        return "microphone_30.png";
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
