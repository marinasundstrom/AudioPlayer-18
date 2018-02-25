using System;
using System.Threading.Tasks;

namespace Axis.AudioPlayer.Services
{
    public interface IAppContext
    {
        Data.Device Device { get; }

		IPlayerService Player { get; }

        Task SetDevice(Data.Device device);

        event EventHandler DeviceChanged;

        Task Initialize(bool isResume = false);

        Task ForgetDevice();

        Task Save();
    }
}
