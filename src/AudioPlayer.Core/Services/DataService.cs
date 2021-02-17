using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AudioPlayer.Services
{
    public class DataService : IDataService
    {
        private List<Device> devices = new List<Device>();
        private string dbFullPath;
        private const string fileName = "devices.json";

        public DataService()
        {
            var dbFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            dbFullPath = Path.Combine(dbFolder, fileName);
        }

        public Task<IEnumerable<Device>> GetDevicesAsync() => Task.Run(() => devices.AsEnumerable());

        public Task<Device> AddOrUpdateDeviceAsync(Device device)
        {
            return Task.Run(() =>
            {
                bool any = devices.Any(x => x.Id == device.Id);
                int index = devices.TakeWhile(x => x.Id != device.Id).Count();
                if (any && index > -1)
                {
                    devices[index] = device;
                }
                else
                {
                    device.Id = Guid.NewGuid();
                    devices.Add(device);
                }
                Save();
                return device;
            });
        }

        private Task Save()
        {
            return Task.Run(() =>
            {
                var text = JsonConvert.SerializeObject(devices);
                File.WriteAllText(dbFullPath, text);
            });
        }

        public Task InitializeAsync()
        {
            return Task.Run(() =>
            {
                if (File.Exists(dbFullPath))
                {
                    var text = File.ReadAllText(dbFullPath);
                    devices = JsonConvert.DeserializeObject<List<Device>>(text);
                }
                else
                {
                    devices = new List<Device>();
                }
            });
        }

        public Task<Device> GetDeviceAsync(Guid id) => Task.Run(() => devices.FirstOrDefault(x => x.Id == id));

        public Task RemoveAsync(Device device)
        {
            var d = devices.First(x => x.Id == device.Id);
            devices.Remove(d);
            return Save();
        }
    }
}
