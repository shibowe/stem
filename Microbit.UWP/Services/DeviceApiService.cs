using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microbit.UWP.Models;

namespace Microbit.UWP.Services
{
    public class DeviceApiService
    {
        public List<DeviceModel> AllDeviceList { get; set; }
        public static DeviceApiService Instance { get; } = new DeviceApiService();

        private DeviceApiService()
        {
            AllDeviceList = new List<DeviceModel>();
        }

        
    }
}
