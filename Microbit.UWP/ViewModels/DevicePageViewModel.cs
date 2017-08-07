using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

using Microbit.UWP.Models;
using System.Collections.ObjectModel;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System;

namespace Microbit.UWP.ViewModels
{
    public class DevicePageViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private INavigationService _navigate;

        private Microsoft.Toolkit.Uwp.ObservableGattDeviceService readGattServices;
        private Microsoft.Toolkit.Uwp.ObservableGattCharacteristics readGattCharacteristics;

        private ObservableCollection<Microsoft.Toolkit.Uwp.ObservableGattDeviceService> ServiceCollection = new ObservableCollection<Microsoft.Toolkit.Uwp.ObservableGattDeviceService>();
        private ObservableCollection<Microsoft.Toolkit.Uwp.ObservableGattCharacteristics> CharacteristicCollection = new ObservableCollection<Microsoft.Toolkit.Uwp.ObservableGattCharacteristics>();


        public DevicePageViewModel(IDialogService dialogService, INavigationService navigation)
        {
            _dialogService = dialogService;
            _navigate = navigation;

            Messenger.Default.Register<Microsoft.Toolkit.Uwp.ObservableBluetoothLEDevice>(this, async (obj) =>
            {
                Microsoft.Toolkit.Uwp.ObservableBluetoothLEDevice device = obj as Microsoft.Toolkit.Uwp.ObservableBluetoothLEDevice;

                if (device.IsPaired)
                {
                    await device.ConnectAsync();
                    ServiceCollection = device.Services;
                }
            });

        }

        #region Properties
        private string _deviceName;
        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                Set(ref _deviceName, value);
                RaisePropertyChanged(nameof(DeviceName));
            }
        }

        private string _statusContent = default(string);
        public string StatusContent
        {
            get { return _statusContent; }
            set
            {
                Set(ref _statusContent, value);
                RaisePropertyChanged(nameof(StatusContent));
            }
        }
        #endregion

        #region Enumerating services
        
        #endregion

    }
}
