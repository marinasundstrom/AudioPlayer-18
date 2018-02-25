using System.Windows.Input;

namespace Axis.AudioPlayer.ViewModels
{
    public class MenuItem
    {
        public string Title { get; set; }

        public ICommand Command { get; set; }
    }
}
