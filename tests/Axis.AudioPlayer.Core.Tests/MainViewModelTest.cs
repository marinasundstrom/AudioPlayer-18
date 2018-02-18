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

            var mainViewModel = new MainViewModel(resourceContainer);

            Assert.Equal("Click me!", mainViewModel.ButtonText);

            mainViewModel.ClickCommand.Execute(null);

            Assert.Equal("Once", mainViewModel.ButtonText);

            mainViewModel.ClickCommand.Execute(null);

            Assert.Equal("Twice", mainViewModel.ButtonText);

            mainViewModel.ClickCommand.Execute(null);

            Assert.Equal("Thrice", mainViewModel.ButtonText);

            mainViewModel.ClickCommand.Execute(null);

            Assert.Equal("4 times", mainViewModel.ButtonText);

            mainViewModel.ClickCommand.Execute(null);

            Assert.Equal("5 times", mainViewModel.ButtonText);

            mainViewModel.ClickCommand.Execute(null);

            Assert.Equal("6 times", mainViewModel.ButtonText);
        }
    }
}
