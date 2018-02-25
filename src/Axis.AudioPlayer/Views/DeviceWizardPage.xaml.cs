using System;
using System.Collections.Generic;
using Axis.AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Views
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
