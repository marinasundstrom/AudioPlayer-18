using Xamarin.Forms;

namespace Axis.AudioPlayer.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        int count = 0;

        void Button_Clicked(object sender, System.EventArgs e)
        {
            Button.Text = $"{++count} times";
        }
    }
}
