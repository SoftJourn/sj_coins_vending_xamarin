using MvvmCross.Platform.IoC;

namespace Softjourn.SJCoins.Core.UI
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

			RegisterAppStart<ViewModels.LoginViewModel>();
        }
    }
}
