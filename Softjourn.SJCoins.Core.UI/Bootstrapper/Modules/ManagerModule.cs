using Autofac;
using Softjourn.SJCoins.Core.Managers;

namespace Softjourn.SJCoins.Core.UI.Bootstrapper.Modules
{
    internal sealed class ManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
			builder.RegisterType<DataManager>().SingleInstance();
            builder.RegisterType<PhotoManager>().SingleInstance();
            builder.RegisterType<QrManager>().SingleInstance();
            builder.RegisterType<TransactionsManager>().SingleInstance();
        }
    }
}
