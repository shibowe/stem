using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml.Media.Imaging;

namespace Microbit.UWP.Models
{
    public class DeviceModel : INotifyPropertyChanged
    {
        public string ConnectStatus { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public DeviceModel(DeviceInformation deviceInfoIn)
        {
            DeviceInformation = deviceInfoIn;
        }
        public string Sensor { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Pressure { get; set; }
        public string Battery { get; set; }
        public string PM2_5 { get; set; }
        public string PM10 { get; set; }
        public DateTime Time { get; set; }

        public DeviceInformation DeviceInformation { get; private set; }

        public string Id => DeviceInformation.Id;
        public string Name => DeviceInformation.Name;
        public string IsPaired => DeviceInformation.Pairing.IsPaired ? "已配对," : "未配对,";
        public string IsConnected => ((bool?)DeviceInformation.Properties["System.Devices.Aep.IsConnected"] == true) ? "已连接" : "未连接";
        public IReadOnlyDictionary<string, object> Properties => DeviceInformation.Properties;

        public string ModuleNumber = "werwesdfadfdfa";
        public string FirmwareNumber = "fgadf2324323";
        public string SerialNumber = "23432423423423";

        public void Update(DeviceInformationUpdate deviceInfoUpdate)
        {
            DeviceInformation.Update(deviceInfoUpdate);

            OnPropertyChanged("Id");
            OnPropertyChanged("Name");
            OnPropertyChanged("DeviceInformation");
            OnPropertyChanged("IsPaired");
            OnPropertyChanged("IsConnected");
            OnPropertyChanged("Properties");
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
