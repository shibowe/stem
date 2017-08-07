using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

using Microbit.UWP.Models;
using System.Collections.ObjectModel;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;

namespace Microbit.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevicePage : Page
    {
        private ObservableCollection<BluetoothLEAttributeModel> ServiceCollection = new ObservableCollection<BluetoothLEAttributeModel>();
        private ObservableCollection<BluetoothLEAttributeModel> CharacteristicCollection = new ObservableCollection<BluetoothLEAttributeModel>();

        private BluetoothLEDevice bluetoothLeDevice = null;
        private GattCharacteristic selectedCharacteristic;
        private bool isValueChangedHandlerRegistered = false;
        private GattPresentationFormat presentationFormat;

        public DevicePage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += DevicePage_BackRequested;

            var deviceInfo = e.Parameter as DeviceModel;
            if (null != deviceInfo)
            {
                tblHeaderTitle.Text = deviceInfo.Name;
                InitConnectDevice(deviceInfo.Id);
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        private void DevicePage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            if (this.Frame.CanGoBack)
            {
                e.Handled = true;

                Frame.GoBack();
            }
        }
        public async void InitConnectDevice(string deviceId)
        {
            //ClearBluetoothLEDevice();
            try
            {
                if (!string.IsNullOrEmpty(deviceId))
                {
                    await DispatcherHelper.RunAsync(async () =>
                    {
                        // BT_Code: BluetoothLEDevice.FromIdAsync must be called from a UI thread because it may prompt for consent.
                        bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(deviceId);
                    });
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
                await DispatcherHelper.RunAsync(() =>
                {
                    StatusBlock.Text = " 连接到设备失败...";
                });
            }
        }
        private void ClearBluetoothLEDevice()
        {
            bluetoothLeDevice?.Dispose();
            bluetoothLeDevice = null;
        }
    }
}
