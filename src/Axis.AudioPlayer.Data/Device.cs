using System;
using System.ComponentModel.DataAnnotations;

namespace Axis.AudioPlayer.Data
{
    public class Device
    {
        [Key]
        public Guid Id { get; set; }
        public string DisplayName { get; set; }

        public string Product { get; set; }

        public string IPAddress { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public virtual DeviceState State { get; set; }
    }

    public class DeviceState
    {
        [Key]
        public Guid Id { get; set; }
        public double ForegroundVolume { get; set; }
        public double BackgroundVolume { get; set; }
        public double MusicVome { get; set; }
    }
}
