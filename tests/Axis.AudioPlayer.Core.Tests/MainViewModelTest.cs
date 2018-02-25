using System.Reflection;
using System.Resources;
using Axis.AudioPlayer.Resources;
using Axis.AudioPlayer.ViewModels;
using Xunit;

namespace Axis.AudioPlayer.Core.Tests
{

    public class MainViewModelTest
    {
        [Fact]
        public void ExecuteClickCommand()
        {
            var resourceContainer = new ResourceContainer(new ResourceManager(ResourceContainer.ResourceId, typeof(AppResources).GetTypeInfo().Assembly), new Localize());

            // var mainViewModel = new MainViewModel(resourceContainer);
        }
    }
}
