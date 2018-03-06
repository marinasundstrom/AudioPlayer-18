using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Axis.AudioPlayer.Data;
using Microsoft.EntityFrameworkCore;

namespace Axis.AudioPlayer.Services
{
    public class DataService : IDataService
    {
        private readonly AudioPlayerContext db;

        private const string fileName = "Data.db";

        public DataService()
        {
            var dbFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbFullPath = Path.Combine(dbFolder, fileName);
            db = new AudioPlayerContext(dbFullPath);
        }

        public async Task<IEnumerable<Data.Device>> GetDevicesAsync() => await db.Devices.ToArrayAsync();

        public async Task<Data.Device> AddOrUpdateDeviceAsync(Data.Device device)
        {
            var f = db.Update(device);
            await db.SaveChangesAsync();
            return f.Entity;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await db.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

		public Task<Data.Device> GetDeviceAsync(Guid id) => db.Devices.FirstOrDefaultAsync(x => x.Id == id);

        public Task RemoveAsync(Data.Device device)
        {
            db.Devices.Remove(device);
            return db.SaveChangesAsync();
        }
    }
}
