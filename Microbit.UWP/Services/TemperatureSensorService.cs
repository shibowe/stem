﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Microbit.UWP.Services
{
    public class TemperatureSensorService : SensorBase
    {
        public TemperatureSensorService(GattDeviceService dataService) :
        base(dataService, SensorUUIDs.UUID_ENV_TEMP)
        { }
        public async Task<double> GetTemperature()
        {
            byte[] data = await ReadValue();
            return ((double)BitConverter.ToInt16(data, 0)) / 10;
        }
    }
}
