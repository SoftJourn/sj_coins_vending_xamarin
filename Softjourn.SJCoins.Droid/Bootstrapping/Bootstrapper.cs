using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Droid.Services;

namespace Softjourn.SJCoins.Droid.Bootstrapping
{
    public class Bootstrapper : BaseBootstrapper
    {
        protected override void RegisterPlatformDependencies(ContainerBuilder builder)
        {
        }

        protected override void RegisterUIDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<AlertService>().As<IAlertService>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
        }
    }
}