using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbit.UWP.Services
{
    public static class SensorUUIDs
    {
        public static String UARTSERVICE_SERVICE_UUID = "6E400001B5A3F393E0A9E50E24DCCA9E";
        public static String UART_RX_CHARACTERISTIC_UUID = "6E400002B5A3F393E0A9E50E24DCCA9E";
        public static String UART_TX_CHARACTERISTIC_UUID = "6E400003B5A3F393E0A9E50E24DCCA9E";

        public const string UUID_ACC_SERV = "02366e80-cf3a-11e1-9ab4-0002a5d5c51b";
        public const string UUID_ACC_DATA = "340a1b80-cf4b-11e1-ac36-0002a5d5c51b";
        public const string UUID_ENV_SERV = "42821a40-e477-11e2-82d0-0002a5d5c51b";
        public const string UUID_ENV_TEMP = "a32e5520-e477-11e2-a9e3-0002a5d5c51b";
        public const string UUID_ENV_PRES = "cd20c480-e48b-11e2-840b-0002a5d5c51b";
    }
}
