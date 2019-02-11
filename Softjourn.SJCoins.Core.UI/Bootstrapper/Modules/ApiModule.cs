using Autofac;
using Softjourn.SJCoins.Core.API;

namespace Softjourn.SJCoins.Core.UI.Bootstrapper.Modules
{
    internal class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiClient>().AsSelf();
            builder.RegisterType<ApiService>().AsSelf().PropertiesAutowired();
        }
    }
}
