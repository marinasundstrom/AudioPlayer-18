using System;

namespace AudioPlayer.Messages
{
    public class DeviceAdded
    {
        public DeviceAdded(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; }
    }
}
