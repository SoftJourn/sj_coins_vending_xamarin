using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper.Modules;

namespace Softjourn.SJCoins.Core.UI.Bootstrapper
{
    public abstract class BaseBootstrapper
    {
        public static IContainer Container { get; private set; }
        

        public void Init()
        {
            ConfigureCrashAnalitics();

            var builder = new ContainerBuilder();
            RegisterCoreDependencies(builder);
            RegisterUIDependencies(builder);
            RegisterPlatformDependencies(builder);
            Container = builder.Build();
            
        }

        protected abstract void RegisterPlatformDependencies(ContainerBuilder builder);

        protected abstract void RegisterUIDependencies(ContainerBuilder builder);

        protected abstract void ConfigureCrashAnalitics();

        private void RegisterCoreDependencies(ContainerBuilder builder)
        {
            builder.RegisterModule<ManagerModule>();
            builder.RegisterModule<ApiModule>();
            builder.RegisterModule<PresenterModule>();
        }
    }
}
