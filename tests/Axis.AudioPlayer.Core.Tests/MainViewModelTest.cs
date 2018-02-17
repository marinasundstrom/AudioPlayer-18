using System;
using Axis.AudioPlayer.ViewModels;
using Xunit;

namespace Axis.AudioPlayer.Core.Tests
{
    public class MainViewModelTest
    {
        [Fact]
        public void ExecuteClickCommand()
        {
            var mainViewModel = new MainViewModel();

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
