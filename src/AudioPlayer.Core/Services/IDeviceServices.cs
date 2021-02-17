using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.Services
{
    public interface IDeviceServices
    {
        void OpenBrowser(Uri uri);
    }
}
