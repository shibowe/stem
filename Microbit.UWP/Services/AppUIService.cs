using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microbit.UWP.Services
{
    public static class AppUIService
    {
        public static void SetWindowsMobileStatusBarColor(Color? backgroundColor, Color? foregroundColor)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = backgroundColor;
                statusBar.ForegroundColor = foregroundColor;
                statusBar.BackgroundOpacity = 1;
            }
        }
        public static async Task HideWindowsMobileStatusBarAsync()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }
        }
        public static async void ShowStatusBar()
        {
            // turn on SystemTray for mobile
            // don't forget to add a Reference to Windows Mobile Extensions For The UWP
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = ColorHelper.FromArgb(255, 70, 84, 158);
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Colors.White;

                await statusbar.ShowAsync();
            }
        }
    }
}
