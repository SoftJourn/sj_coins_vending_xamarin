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
            builder.RegisterType<BasePresenter>().AsSelf();
            builder.RegisterType<LaunchPresenter>().AsSelf();
            builder.RegisterType<WelcomePresenter>().AsSelf();
            builder.RegisterType<LoginPresenter>().AsSelf();
        }
    }
}
