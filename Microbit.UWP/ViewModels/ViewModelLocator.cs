using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

using Microbit.UWP.Services;

namespace Microbit.UWP.ViewModels
{
    public class ViewModelLocator
    {
        public const string DevicePageKey = "DevicePage";
        public const string ConnectPageKey = "ConnectPage";
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            //set navigation service for each page
            var nav = new NavigationService();
            nav.Configure(DevicePageKey, typeof(Views.DevicePage));
            nav.Configure(ConnectPageKey, typeof(Views.ConnectPage));



            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // to do something
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            //set viewmodels for each page
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<DevicePageViewModel>();

        }
        //define the viewmodel as properties for each page
        public DevicePageViewModel DPVM => ServiceLocator.Current.GetInstance<DevicePageViewModel>();
        public MainPageViewModel MPVM => ServiceLocator.Current.GetInstance<MainPageViewModel>();

    }
}
