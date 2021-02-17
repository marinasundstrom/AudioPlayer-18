using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AudioPlayer.Converters
{
	class AlbumIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Track track)
			{
				if (track.IsStream())
				{
					return "online_100.png";
				}
				else if(track.IsLineIn()) 
				{
					return "microphone_100.png";
				} 
				else 
				{
					return "musical_notes_100.png";
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
