namespace Softjourn.SJCoins.Core.UI.Presenters.IPresenters
{
    public interface ILoginPresenter
    {
        void Login(string userName, string password);

        void ToWelcomePage();
    }
}
