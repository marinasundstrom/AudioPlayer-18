using System.Windows.Input;
using Axis.AudioPlayer.Services;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string buttonText;
        private RelayCommand clickCommand;
        private int count;

        public MainViewModel()
        {
            ButtonText = "Click me!";
        }

        public string ButtonText
        {
            get => buttonText;
            set => SetProperty(ref buttonText, value);
        }

        public ICommand ClickCommand => clickCommand ?? (clickCommand = new RelayCommand(() =>
        {
            count++;

            switch (count)
            {
                case 1:
                    ButtonText = "Once";
                    break;

                case 2:
                    ButtonText = "Twice";
                    break;

                case 3:
                    ButtonText = "Thrice";
                    break;

                default:
                    ButtonText = $"{count} times";
                    break;
            }
        }));
    }
}
