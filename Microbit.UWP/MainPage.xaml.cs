using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using Newtonsoft.Json;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

using Microbit.UWP.Services;
using Microsoft.Toolkit.Uwp;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Microbit.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DeviceWatcher deviceWatcher;
        StringBuilder strUUID = new StringBuilder();
        ObservableCollection<DeviceInformation> deviceList = new ObservableCollection<DeviceInformation>();

        ObservableBluetoothLEDevice device;

        private TypedEventHandler<DeviceWatcher, DeviceInformation> handlerAdded = null;
        private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> handlerUpdated = null;
        private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> handlerRemoved = null;
        private TypedEventHandler<DeviceWatcher, Object> handlerEnumCompleted = null;
        private TypedEventHandler<DeviceWatcher, Object> handlerStopped = null;

        public ObservableCollection<DeviceInformationDisplay> ResultCollection
        {
            get;
            private set;
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ResultCollection = new ObservableCollection<DeviceInformationDisplay>();

            selectorComboBox.ItemsSource = DeviceSelectorChoices.DeviceWatcherSelectors;
            selectorComboBox.SelectedIndex = 0;

            DataContext = this;
        }


        private void BtnDevicePicker_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Emotion API
        private async void BtnInvokeAPI_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (null != file)
            {

                using (var inputStream = await file.OpenSequentialReadAsync())
                {
                    var readStream = inputStream.AsStreamForRead();
                    byte[] byteArray = new byte[readStream.Length];

                    await readStream.ReadAsync(byteArray, 0, byteArray.Length);

                    MakeRequest(file.Path.Trim(), byteArray);
                }
            }
            else
            {
                this.tblResults.Text = "没有选择任何文件";
            }

        }

        private async void MakeRequest(string imagePath, byte[] byteData)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9c1e3b2860d8440c981ff1077c4cb6c2");

            string uri = "https://api.cognitive.azure.cn/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<List<Models.EmotionModel>>(responseContent);
                foreach (var item in result)
                {
                    this.tblResults.Text = "惊喜值:" + item.Scores.surprise.ToString();
                }
            }
        }
        #endregion


        #region Bluetooth LE Paire and Unpaire
        private async void resultsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resultsListView.IsEnabled = false;
            string.Format("Pairing started. Please wait...");

            DeviceInformationDisplay deviceInfoDisp = resultsListView.SelectedItem as DeviceInformationDisplay;

            DevicePairingResult dpr = await deviceInfoDisp.DeviceInformation.Pairing.PairAsync();
            device = new ObservableBluetoothLEDevice(deviceInfoDisp.DeviceInformation);
            await device.ConnectAsync();

            if (device.IsPaired)
            {
                var services = device.Services;

                Frame.Navigate(typeof(Views.DeviceSensorPage), e.AddedItems[0]);
            }

            string.Format("Pairing result = " + dpr.Status.ToString());
            resultsListView.IsEnabled = true;
        }

        private async void BtnUnpairDevice_Click(object sender, RoutedEventArgs e)
        {

            DeviceInformationDisplay deviceInfoDisp = resultsListView.SelectedItem as DeviceInformationDisplay;

            DeviceUnpairingResult dupr = await deviceInfoDisp.DeviceInformation.Pairing.UnpairAsync();

            this.tblResults.Text = string.Format("Unpairing result = " + dupr.Status.ToString());
        }
        #endregion

        private void selectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResultCollection.Clear();

            if (selectorComboBox.SelectedIndex == 0)
            {
                return;
            }

            DeviceSelectorInfo deviceSelectorInfo = (DeviceSelectorInfo)selectorComboBox.SelectedItem;

            if (null == deviceSelectorInfo.Selector)
            {
                // If the a pre-canned device class selector was chosen, call the DeviceClass overload
                deviceWatcher = DeviceInformation.CreateWatcher(deviceSelectorInfo.DeviceClassSelector);
            }
            else if (deviceSelectorInfo.Kind == DeviceInformationKind.Unknown)
            {
                // Use AQS string selector from dynamic call to a device api's GetDeviceSelector call
                // Kind will be determined by the selector
                deviceWatcher = DeviceInformation.CreateWatcher(
                    deviceSelectorInfo.Selector,
                    null // don't request additional properties for this sample
                    );
            }
            else
            {
                // Kind is specified in the selector info
                deviceWatcher = DeviceInformation.CreateWatcher(
                    deviceSelectorInfo.Selector,
                    null, // don't request additional properties for this sample
                    deviceSelectorInfo.Kind);
            }
            // Hook up handlers for the watcher events before starting the watcher

            handlerAdded = new TypedEventHandler<DeviceWatcher, DeviceInformation>(async (watcher, deviceInfo) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    ResultCollection.Add(new DeviceInformationDisplay(deviceInfo));

                    this.tblResults.Text = String.Format("{0} devices found.", ResultCollection.Count);
                });
            });
            deviceWatcher.Added += handlerAdded;

            handlerUpdated = new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(async (watcher, deviceInfoUpdate) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    // Find the corresponding updated DeviceInformation in the collection and pass the update object
                    // to the Update method of the existing DeviceInformation. This automatically updates the object
                    // for us.
                    foreach (DeviceInformationDisplay deviceInfoDisp in ResultCollection)
                    {
                        if (deviceInfoDisp.Id == deviceInfoUpdate.Id)
                        {
                            deviceInfoDisp.Update(deviceInfoUpdate);
                            break;
                        }
                    }
                });
            });
            deviceWatcher.Updated += handlerUpdated;

            handlerRemoved = new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(async (watcher, deviceInfoUpdate) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    // Find the corresponding DeviceInformation in the collection and remove it
                    foreach (DeviceInformationDisplay deviceInfoDisp in ResultCollection)
                    {
                        if (deviceInfoDisp.Id == deviceInfoUpdate.Id)
                        {
                            ResultCollection.Remove(deviceInfoDisp);
                            break;
                        }
                    }

                    this.tblResults.Text = String.Format("{0} devices found.", ResultCollection.Count);
                });
            });
            deviceWatcher.Removed += handlerRemoved;

            handlerEnumCompleted = new TypedEventHandler<DeviceWatcher, Object>(async (watcher, obj) =>
            {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    this.tblResults.Text =
                        String.Format("{0} devices found. Enumeration completed. Watching for updates...", ResultCollection.Count);
                });
            });
            deviceWatcher.EnumerationCompleted += handlerEnumCompleted;

            handlerStopped = new TypedEventHandler<DeviceWatcher, Object>(async (watcher, obj) =>
            {
                await this.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    this.tblResults.Text = String.Format("{0} devices found. Watcher {1}.",
                                ResultCollection.Count,
                                DeviceWatcherStatus.Aborted == watcher.Status ? "aborted" : "stopped");
                });
            });
            deviceWatcher.Stopped += handlerStopped;

            this.tblResults.Text = "Starting Watcher...";
            deviceWatcher.Start();
        }
    }
}
