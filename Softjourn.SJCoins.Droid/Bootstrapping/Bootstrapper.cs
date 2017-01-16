using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Managers;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Droid.Managers;
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
            builder.RegisterType<AlertManager>().As<IAlertManager>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
        }
    }
}