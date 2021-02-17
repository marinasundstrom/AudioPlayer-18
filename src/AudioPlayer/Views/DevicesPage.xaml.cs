using System;
using System.Collections.Generic;
using AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace AudioPlayer.Views
{
    public partial class DevicesPage : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public DevicesPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await (BindingContext as DevicesViewModel).InitializeAsync();
            }
            catch (Exception)
            {

            }
        }
    }
}
