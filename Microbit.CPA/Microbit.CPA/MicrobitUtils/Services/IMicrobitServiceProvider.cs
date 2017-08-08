using System;

namespace Microbit.CPA.MicrobitUtils.Services
{
	public interface IMicrobitServiceProvider
	{
		String ServiceName { get;}
		String ServiceDescription { get;}
		IMicrobitService GetServiceInstance();
	}
}
