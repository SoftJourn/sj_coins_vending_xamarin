using Autofac;
using Softjourn.SJCoins.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.Managers;

namespace Softjourn.SJCoins.Core.UI.Bootstrapper.Modules
{
    class ManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			builder.RegisterType<DataManager>().SingleInstance();
        }
    }
}
