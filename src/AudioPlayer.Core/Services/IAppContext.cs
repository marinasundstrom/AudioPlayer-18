using System;
using System.Threading.Tasks;

namespace AudioPlayer.Services
{
    public interface IAppContext
    {
        Device Device { get; }

		IPlayerService Player { get; }

        Task SetDevice(Device device);

        event EventHandler DeviceChanged;

        Task Initialize(bool isResume = false);

        Task ForgetDevice();

        Task Save();
    }
}
