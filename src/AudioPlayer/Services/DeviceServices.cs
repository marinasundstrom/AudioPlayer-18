using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.Services
{
    public class DeviceServices : IDeviceServices
    {
        public void OpenBrowser(Uri uri)
        {
            Xamarin.Forms.Device.OpenUri(uri);
        }
    }
}
