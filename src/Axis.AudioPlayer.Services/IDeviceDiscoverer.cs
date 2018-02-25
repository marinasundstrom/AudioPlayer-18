using System;

namespace Axis.AudioPlayer.Services
{
    public interface IDeviceDiscoverer
    {
        void Start();

        void Stop();

        event EventHandler<DeviceDiscoveryEventArgs> DeviceDiscovered;

		IObservable<Device> WhenDeviceDiscovered { get; }
    }

    public class DeviceDiscoveryEventArgs : EventArgs
    {
        public DeviceDiscoveryEventArgs(Device device)
        {
            Device = device;
        }

        public Device Device { get; }
    }
}