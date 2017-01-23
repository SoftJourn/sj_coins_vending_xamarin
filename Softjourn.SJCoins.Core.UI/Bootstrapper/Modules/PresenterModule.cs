using Autofac;
using Softjourn.SJCoins.Core.UI.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Bootstrapper.Modules
{
    class PresenterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LaunchPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<WelcomePresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<LoginPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<MainPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<SelectMachinePresenter>().AsSelf().PropertiesAutowired();
        }
    }
}
