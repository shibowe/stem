using System;

namespace MicrobitCPA.MicrobitUtils.Services
{
	public interface IMicrobitServiceProvider
	{
		String ServiceName { get;}
		String ServiceDescription { get;}
		IMicrobitService GetServiceInstance();
	}
}
