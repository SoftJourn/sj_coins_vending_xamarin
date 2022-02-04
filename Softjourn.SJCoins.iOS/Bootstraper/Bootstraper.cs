using System;
using Autofac;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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


        protected override void ConfigureCrashAnalitics()
        {
            AppCenter.Start("20136dd6-fcb5-4c30-bb41-52b07afd66c3",
                typeof(Analytics), typeof(Crashes));
        }

    }
}
