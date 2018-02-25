using Microsoft.EntityFrameworkCore;

namespace Axis.AudioPlayer.Data
{
    public class AudioPlayerContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        public DbSet<DeviceState> DeviceStates { get; set; }

        private string DatabasePath { get; }

        public AudioPlayerContext()
        {

        }

        public AudioPlayerContext(string databasePath)
        {
            DatabasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }
    }
}
