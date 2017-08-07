


using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;

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

        private ObservableCollection<BluetoothLEAttributeModel> ServiceCollection = new ObservableCollection<BluetoothLEAttributeModel>();
        private ObservableCollection<BluetoothLEAttributeModel> CharacteristicCollection = new ObservableCollection<BluetoothLEAttributeModel>();

        private BluetoothLEDevice bluetoothLeDevice = null;
        private GattCharacteristic selectedCharacteristic;
        private bool isValueChangedHandlerRegistered = false;
        private GattPresentationFormat presentationFormat;

        public DevicePageViewModel(IDialogService dialogService, INavigationService navigation)
        {
            _dialogService = dialogService;
            _navigate = navigation;

            Messenger.Default.Register<DeviceModel>(this, (obj) =>
            {
                DeviceName = obj.Name;
                InitConnectDevice(obj.Id);
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
        public async void InitConnectDevice(string deviceId)
        {
            ClearBluetoothLEDevice();
            try
            {
                if (!string.IsNullOrEmpty(deviceId))
                {
                    // BT_Code: BluetoothLEDevice.FromIdAsync must be called from a UI thread because it may prompt for consent.
                    bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(deviceId);
                }
            }
            catch (Exception ex) when ((uint)ex.HResult == 0x800710df)
            {
                // ERROR_DEVICE_NOT_AVAILABLE because the Bluetooth radio is not on.
            }

            if (bluetoothLeDevice != null)
            {
                // BT_Code: GattServices returns a list of all the supported services of the device.
                // If the services supported by the device are expected to change
                // during BT usage, subscribe to the GattServicesChanged event.
                var gatt = await bluetoothLeDevice.GetGattServicesAsync();
                foreach (var service in gatt.Services)
                {
                    ServiceCollection.Add(new BluetoothLEAttributeModel(service));
                }
            }
            else
            {
                ClearBluetoothLEDevice();
                StatusContent = " 连接到设备失败...";
            }
        }
        private void ClearBluetoothLEDevice()
        {
            bluetoothLeDevice?.Dispose();
            bluetoothLeDevice = null;
        }
        #endregion

    }
}
