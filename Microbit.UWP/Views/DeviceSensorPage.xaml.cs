using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microbit.UWP.ViewModels;
using Windows.UI.Core;
using Windows.Devices.Enumeration;
using Microbit.UWP.Services;

namespace Microbit.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeviceSensorPage : Page
    {
        SensorsViewModel viewModel;
        public DeviceSensorPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += DevicePage_BackRequested;

            var device = (e.Parameter as DeviceInformationDisplay).DeviceInformation;

            viewModel = new SensorsViewModel(device);
            viewModel.StartReceivingData();


            this.DataContext = viewModel;
            base.OnNavigatedTo(e);
        }
        private void DevicePage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= DevicePage_BackRequested;
            viewModel.Dispose();
            base.OnNavigatedFrom(e);
        }
    }
}
