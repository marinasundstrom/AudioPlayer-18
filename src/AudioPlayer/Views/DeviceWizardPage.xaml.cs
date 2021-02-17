using System;
using System.Collections.Generic;
using AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace AudioPlayer.Views
{
    public partial class DeviceWizardPage : NavigationPage
    {
        public DeviceWizardPage()
            : base(new DeviceDiscoveryPage())
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {

            }
        }
    }
}
