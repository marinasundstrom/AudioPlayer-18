using System;
using System.Linq;
using System.Reactive.Linq;
using Zeroconf;

namespace AudioPlayer.Services
{
	public class DeviceDiscoverer : IDeviceDiscoverer, IDisposable
	{
		private const string PROTOCOL = "_axis-video._tcp.local.";

		private ZeroconfResolver.ResolverListener listener;

		public event EventHandler<DeviceDiscoveryEventArgs> DeviceDiscovered;

        public IObservable<DiscoveryDevice> WhenDeviceDiscovered 
			=> Observable
			.FromEventPattern<EventHandler<DeviceDiscoveryEventArgs>, DeviceDiscoveryEventArgs>(add => DeviceDiscovered += add, remove => DeviceDiscovered -= remove)
			.Select(x => x.EventArgs.Device);
		
        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            listener = ZeroconfResolver.CreateListener(new[] { PROTOCOL });
            listener.ServiceFound += Listener_ServiceFound;
        }

        public void Stop()
        {
            listener.Dispose();
        }

        private void Listener_ServiceFound(object sender, IZeroconfHost e)
        {
            foreach(var service in e.Services.Select(x => x.Value))
            {
                var properties = service.Properties;
                if(properties.SelectMany(x => x).Any(prop => prop.Key == "AudioRelay"))
                {
                    var device = new DiscoveryDevice()
                    {
                        DisplayName = e.DisplayName,
                        IPAddress = e.IPAddresses.Last(),
                        Product = properties[0]["product"]
                    };
                    DeviceDiscovered?.Invoke(this, new DeviceDiscoveryEventArgs(device));
                }
            }
        }
    }
}
