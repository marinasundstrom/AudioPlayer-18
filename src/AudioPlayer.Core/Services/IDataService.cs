using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AudioPlayer.Services
{
    public interface IDataService
    {
        Task<Device> AddOrUpdateDeviceAsync(Device device);
        Task<Device> GetDeviceAsync(Guid id);
        Task<IEnumerable<Device>> GetDevicesAsync();
        Task RemoveAsync(Device device);
        Task InitializeAsync();
    }

    public class Device
    {
        [Key]
        public Guid Id { get; set; }
        public string DisplayName { get; set; }

        public string Product { get; set; }

        public string IPAddress { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
