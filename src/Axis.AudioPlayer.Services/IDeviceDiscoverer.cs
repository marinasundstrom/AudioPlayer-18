using System;

namespace Axis.AudioPlayer.Services
{
    public interface IDeviceDiscoverer
    {
        void Start();

        void Stop();

        event EventHandler<DeviceDiscoveryEventArgs> DeviceDiscovered;

        IObservable<DiscoveryDevice> WhenDeviceDiscovered { get; }
    }

    public class DeviceDiscoveryEventArgs : EventArgs
    {
        public DeviceDiscoveryEventArgs(DiscoveryDevice device)
        {
            Device = device;
        }

        public DiscoveryDevice Device { get; }
    }
}