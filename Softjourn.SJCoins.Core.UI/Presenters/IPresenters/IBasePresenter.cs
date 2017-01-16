using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Presenters.IPresenters
{
    public interface IBasePresenter<TView> where TView : class, IBaseView
    {
        void AttachView(IBaseView view);
        void DetachView();
        void SetNavigationParams(string navigationData);

        //TODO refactor
        void ViewShowed();
        void ViewHidden();
    }
}
