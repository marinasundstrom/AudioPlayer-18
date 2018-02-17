using System;
using System.Threading.Tasks;
using Axis.AudioPlayer.Messages;
using Axis.AudioPlayer.Services;
using MvvmUtils;

namespace Axis.AudioPlayer
{
    public class AppContext
    {
        public AppContext(IMessageBus messageBus, IDataService dataService)
        {
            MessageBus = messageBus;
            DataService = dataService;
        }

        public async Task LoadContext()
        {
            await Task.Delay(2000);

            MessageBus.Publish(new LoadContextMessage());
        }

        public Task SaveContext()
        {
            return Task.CompletedTask;
        }

        public IMessageBus MessageBus { get; }
        public IDataService DataService { get; }
    }
}
