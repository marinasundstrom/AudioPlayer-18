using System;
using System.ComponentModel.DataAnnotations;

namespace Axis.AudioPlayer.Data.Models
{
    public class Device
    {
        [Key]
        public Guid Id { get; set; }

        public string Product { get; set; }

        public string DisplayName { get; set; }

        public string IPAddress { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
