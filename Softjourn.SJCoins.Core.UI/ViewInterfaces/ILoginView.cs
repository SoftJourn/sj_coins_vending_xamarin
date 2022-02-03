namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ILoginView : IBaseView
    {
        void SetUsernameError(string message);

        void SetPasswordError(string message);
    }
}
