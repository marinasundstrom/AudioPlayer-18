using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axis.AudioPlayer.Data;

namespace Axis.AudioPlayer.Services
{
    public interface IDataService
    {
        Task<Data.Device> AddOrUpdateDeviceAsync(Data.Device device);
        Task<Data.Device> GetDeviceAsync(Guid id);
        Task<IEnumerable<Data.Device>> GetDevicesAsync();
        Task RemoveAsync(Data.Device device);
        Task InitializeAsync();
    }
}
