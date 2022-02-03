using Autofac;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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

        protected override void ConfigureCrashAnalitics()
        {
            AppCenter.Start("71335be2-60a6-45ed-9fde-f83b31693577",
                typeof(Analytics), typeof(Crashes));
        }
    }
}