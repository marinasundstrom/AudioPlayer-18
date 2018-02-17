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
            var viewModel = new MainViewModel();

            Assert.Equal(viewModel.ButtonText, "Click me!");

            viewModel.ClickCommand.Execute(null);

            Assert.Equal(viewModel.ButtonText, "Once");

            viewModel.ClickCommand.Execute(null);

            Assert.Equal(viewModel.ButtonText, "Twice");

            viewModel.ClickCommand.Execute(null);

            Assert.Equal(viewModel.ButtonText, "Thrice");

            viewModel.ClickCommand.Execute(null);

            Assert.Equal(viewModel.ButtonText, "4 times");

            viewModel.ClickCommand.Execute(null);

            Assert.Equal(viewModel.ButtonText, "5 times");

            viewModel.ClickCommand.Execute(null);

            Assert.Equal(viewModel.ButtonText, "6 times");
        }
    }
}
