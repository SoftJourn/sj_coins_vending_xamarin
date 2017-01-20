using Autofac;
using Softjourn.SJCoins.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Bootstrapper.Modules
{
    class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiClient>().AsSelf();
            builder.RegisterType<ApiService>().AsSelf().PropertiesAutowired();
        }

    }
}
