using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;

namespace Microbit.UWP.ViewModels
{
    public class DevicePageViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private INavigationService _navigate;

        public DevicePageViewModel(IDialogService dialogService, INavigationService navigation)
        {
            _dialogService = dialogService;
            _navigate = navigation;
        }
    }
}
