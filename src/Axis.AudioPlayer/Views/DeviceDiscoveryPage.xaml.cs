﻿using System;
using System.Collections.Generic;
using Axis.AudioPlayer.Services;
using Axis.AudioPlayer.ViewModels;
using Xamarin.Forms;

namespace Axis.AudioPlayer.Views
{
    public partial class DeviceDiscoveryPage : ContentPage
    {
        public DeviceDiscoveryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await (BindingContext as DeviceWizardViewModel).Initialize();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            (BindingContext as DeviceWizardViewModel).Cleanup();
        }
    }
}
