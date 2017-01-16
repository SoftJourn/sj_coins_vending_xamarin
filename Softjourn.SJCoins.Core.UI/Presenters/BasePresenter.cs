using Softjourn.SJCoins.Core.UI.Managers;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class BasePresenter<TView> : IBasePresenter where TView : class, IBaseView
    {
        public IAlertManager _alertManager;
        public IAlertManager AlertManager
        {
            set { _alertManager = value; }
        }

        public BasePresenter()
        {
            
        }

        #region View

        protected TView View { get; set; }

        public void AttachView(IBaseView view)
        {
            View = view as TView;

            if (View != null)
                OnViewAttached();
            else
                throw new ArgumentException("View");
        }

        public void DetachView()
        {
            View = null;
            OnViewDetached();
        }

        public virtual void ViewShowed()
        {

        }
        public virtual void ViewHidden()
        {

        }

        // Init and load data
        protected virtual void OnViewAttached()
        {

        }

        // Deinit
        protected virtual void OnViewDetached()
        {

        }

        #endregion

        public void SetNavigationParams(string navigationData)
        {
            throw new NotImplementedException();
        }

    }
}
