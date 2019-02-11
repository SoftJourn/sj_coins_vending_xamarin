using Autofac;
using Softjourn.SJCoins.Core.UI.Presenters;

namespace Softjourn.SJCoins.Core.UI.Bootstrapper.Modules
{
    internal class PresenterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LaunchPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<WelcomePresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<LoginPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<HomePresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<SelectMachinePresenter>().AsSelf().PropertiesAutowired();
			builder.RegisterType<AccountPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<DetailPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<ShowAllPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<PurchasePresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<QrPresenter>().AsSelf().PropertiesAutowired();
            builder.RegisterType<TransactionReportPresenter>().AsSelf().PropertiesAutowired();
        }
    }
}
