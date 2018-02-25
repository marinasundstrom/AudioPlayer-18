using System;
using System.IO;
using System.Threading.Tasks;
using Axis.AudioPlayer.ViewModels;
using MvvmUtils;
using Newtonsoft.Json;

namespace Axis.AudioPlayer.Services
{
    public class AppContext : IAppContext
    {
		public AppContext(IMessageBus messageBus, 
		                  IPlayerService player, 
		                  IDataService dataService)
        {
			MessageBus = messageBus;
			Player = player;
            DataService = dataService;
        }

		public IMessageBus MessageBus { get; private set; }

        public Data.Device Device { get; private set; }

		public IPlayerService Player { get; }

        public IDataService DataService { get; }

        public AppParams Parameters { get; private set; }

        public event EventHandler DeviceChanged;

        public async Task ForgetDevice()
        {
            await DataService.RemoveAsync(Device);
            Device = null;
        }

		public async Task Initialize(bool isResume = false)
        {
			await DataService.InitializeAsync();
			await Player.InitializeAsync();

            LoadParameters();
            if (Parameters?.Device != null)
            {
                var device = await DataService.GetDeviceAsync((Guid)Parameters.Device);
				if (device != null)
				{
					await SetDevice(device);
					if (isResume) 
					{
						// Is there a better way?
						MessageBus.Publish(new UpdatePlayer() { Fetch = true });
					}
				}
            }
        }

        private const string FILENAME = "appState.json";

        private string FileName
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(path, FILENAME);
            }
        }

        private void LoadParameters()
        {
            if (File.Exists(FileName))
            {
                var text = File.ReadAllText(FileName);
                Parameters = JsonConvert.DeserializeObject<AppParams>(text);
            }
            else
            {
                Parameters = new AppParams();
            }
        }

        public async Task SetDevice(Data.Device device)
        {
			if (device != null)
			{
				if (Device != null && device.Id == Device.Id)
					return;

				Device = device;
				try
				{
					await InitializePlayer(device);
					DeviceChanged?.Invoke(this, EventArgs.Empty);

					await Save();
				} 
				catch (Exception) {}
            }
        }

        public async Task Save()
        {
            Parameters.Device = Device?.Id;

            var text = JsonConvert.SerializeObject(Parameters);
            File.WriteAllText(FileName, text);
        }

		private async Task InitializePlayer(Data.Device device)
		{
			await Player.Setup(new Uri($"http://{device.IPAddress}"), device.Username, device.Password);
			await Player.PreloadAsync();

			try
			{
				await Player.UpdateAsync();
				await Player.InitiateAsync();
			}
			catch(Exception) 
			{
				// Handle this with an alert in UI
				// Offline / Browsing mode
				throw;
			}
        }
    }

    public class AppParams
    {
        public Guid? Device { get; set; }
    }
}
