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


        public DeviceInformation DeviceInformation { get; private set; }

        public string Id => DeviceInformation.Id;
        public string Name => DeviceInformation.Name;
        public string IsPaired => ConvertPaired(DeviceInformation.Pairing.IsPaired);
        public string IsConnected => ConvertConnected((bool?)DeviceInformation.Properties["System.Devices.Aep.IsConnected"] == true);
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
        private string ConvertPaired(bool isPaired)
        {
            if (isPaired)
            {
                return "已配对,";
            }
            else
            {
                return "未配对,";
            }
        }
        private string ConvertConnected(bool convert)
        {
            if (convert)
            {
                return "已连接设备";
            }
            else
            {
                return "未连接设备";
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
