using System;
using System.Collections.Generic;
using Plugin.BLE.Abstractions.Contracts;

namespace Microbit.CPA.MicrobitUtils.Services
{
	public static class IdToServiceProviderMappingProvider
	{
		private static Dictionary<Guid, Func<IService, IMicrobitServiceProvider>> _mapping = 
			new Dictionary<Guid, Func<IService, IMicrobitServiceProvider>>()
		{
			// Device Information Service
			{ServiceIds.DeviceInformationServiceId, (service) => new MicrobitServiceProvider(
				"设备信息服务",
				"获取设备名称、编码、厂商等",
				ServiceIds.DeviceInformationServiceId,
				service,
				DeviceInformationService.GetInstance)},

			// Temperature Service
			{ServiceIds.TemperatureServiceId, (service) => new MicrobitServiceProvider(
				"温度测量服务",
				"温度传感器数据",
				ServiceIds.TemperatureServiceId,
				service,
				TemperatureService.GetInstance)},

			// Accelerometer Service
			{ServiceIds.AccelerometerServiceId, (service) => new MicrobitServiceProvider(
				"加速度计服务",
				"X, Y, Z 加速度值",
				ServiceIds.AccelerometerServiceId,
				service,
				AccelerometerService.GetInstance)},

			// Button Service
			{ServiceIds.ButtonServiceId, (service) => new MicrobitServiceProvider(
				"按键服务",
				"A / B 按键的状态",
				ServiceIds.ButtonServiceId,
				service,
				ButtonService.GetInstance)},

			// Magnetometer Service
			{ServiceIds.MagnetometerServiceId, (service) => new MicrobitServiceProvider(
				"磁力仪服务",
				"罗盘数据",
				ServiceIds.MagnetometerServiceId,
				service,
				MagnetometerService.GetInstance)},
			
			// Led Service
			{ServiceIds.LedServiceId, (service) => new MicrobitServiceProvider(
				"LED显示服务",
				"屏幕上显示文本或图形",
				ServiceIds.LedServiceId,
				service,
				LedService.GetInstance)},
		};

		public static IMicrobitServiceProvider ServiceProvider(IService serviceInstance)
		{
			Func<IService, IMicrobitServiceProvider> provider;
			if (!_mapping.TryGetValue(serviceInstance.Id, out provider))
			{
				return null;
			}
			return provider(serviceInstance);
		}
	}
}

