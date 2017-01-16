using System;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;

using Softjourn.SJCoins.iOS.Services;

namespace Softjourn.SJCoins.iOS.Bootstraper
{
	public class Bootstraper : BaseBootstrapper
	{
		protected override void RegisterUIDependencies(ContainerBuilder builder)
		{			       
			builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
			builder.RegisterType<AlertService>().As<IAlertService>().SingleInstance();
		}

		protected override void RegisterPlatformDependencies(ContainerBuilder builder)
		{

		}
	}
}
