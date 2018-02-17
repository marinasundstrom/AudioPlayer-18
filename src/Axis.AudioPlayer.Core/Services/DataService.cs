using System;
using System.IO;
using System.Threading.Tasks;
using Axis.AudioPlayer.Data;
using Microsoft.EntityFrameworkCore;

namespace Axis.AudioPlayer.Services
{
    public class DataService : IDataService
    {
        private readonly APContext context;

        private const string fileName = "Data.db";

        public DataService()
        {
            var dbFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbFullPath = Path.Combine(dbFolder, fileName);
            context = new APContext(dbFullPath);

            context.Database.MigrateAsync().Wait();
        }

        public async Task DoSomething()
        {
            context.Devices.Add(new Data.Models.Device() { DisplayName = "Foo" });
            await context.SaveChangesAsync();
        }
    }
}
