using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters.Interfaces
{
    public interface IBasePresenter
    {
        void AttachView(IBaseView view);

        void DetachView();

        //TODO refactor
        void ViewShowed();

        void ViewHidden();
    }
}
