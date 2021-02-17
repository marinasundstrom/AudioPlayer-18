using System;
using AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace AudioPlayer.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await (BindingContext as MainViewModel).Initialize();
        }
    }
}
