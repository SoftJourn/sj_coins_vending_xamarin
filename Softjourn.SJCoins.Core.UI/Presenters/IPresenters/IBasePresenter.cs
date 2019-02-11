using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters.IPresenters
{
    public interface IBasePresenter
    {
        void AttachView(IBaseView view);
        void DetachView();
        void SetNavigationParams(string navigationData);

        //TODO refactor
        void ViewShowed();
        void ViewHidden();
    }
}
