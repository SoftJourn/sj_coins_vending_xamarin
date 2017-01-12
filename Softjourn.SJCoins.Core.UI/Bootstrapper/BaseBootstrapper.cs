using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Bootstrapper
{
    public abstract class BaseBootstrapper
    {
        public static IContainer Container { get; private set; }

        //protected ApplicationStartParameters ApplicationStartParameters { get; set; }


        public void Init()
        {
            var builder = new ContainerBuilder();

            RegisterCoreDependencies(builder);
            RegisterUIDependencies(builder);
            RegisterPlatformDependencies(builder);
            Container = builder.Build();
        }

        protected abstract void RegisterPlatformDependencies(ContainerBuilder builder);

        protected abstract void RegisterUIDependencies(ContainerBuilder builder);

        private void RegisterCoreDependencies(ContainerBuilder builder)
        {
            builder.RegisterModule<ManagerModule>();
            builder.RegisterModule<ApiModule>();
            builder.RegisterModule<PresenterModule>();
        }
    }
}
