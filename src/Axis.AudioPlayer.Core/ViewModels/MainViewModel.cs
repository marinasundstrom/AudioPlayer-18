using System.Windows.Input;
using MvvmUtils;

namespace Axis.AudioPlayer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string buttonText;
        private RelayCommand clickCommand;
        private int count;

        public MainViewModel(IResourceContainer resourceContainer)
        {
            ResourceContainer = resourceContainer;

            ButtonText = ResourceContainer.GetString("ClickMe");
        }

        public IResourceContainer ResourceContainer { get; }

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
                    ButtonText = ResourceContainer.GetString("Once");
                    break;

                case 2:
                    ButtonText = ResourceContainer.GetString("Twice");
                    break;

                case 3:
                    ButtonText = ResourceContainer.GetString("Thrice");
                    break;

                default:
                    ButtonText = ResourceContainer.GetString("Times", count);
                    break;
            }
        }));
    }
}
