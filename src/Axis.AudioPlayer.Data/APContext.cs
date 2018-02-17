using Axis.AudioPlayer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Axis.AudioPlayer.Data
{
    public class APContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        private string DatabasePath { get; set; }

        public APContext()
        {

        }

        public APContext(string databasePath)
        {
            DatabasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }
    }
}
