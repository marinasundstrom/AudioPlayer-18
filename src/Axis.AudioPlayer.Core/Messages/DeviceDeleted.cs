﻿using System;

namespace Axis.AudioPlayer.Messages
{
    public class DeviceDeleted
    {
        public DeviceDeleted(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; }
    }
}
