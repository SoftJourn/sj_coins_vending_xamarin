using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.iOS.Services;
using Softjourn.SJCoins.iOS.UI.Services;

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
