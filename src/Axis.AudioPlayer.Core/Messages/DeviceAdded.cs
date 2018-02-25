using System;

namespace Axis.AudioPlayer.Messages
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
