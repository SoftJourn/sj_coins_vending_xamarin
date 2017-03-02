
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using Softjourn.SJCoins.Core.API;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.Managers;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class BasePresenter<TView> : IBasePresenter where TView : class, IBaseView
    {
		#region Properties
		protected ILifetimeScope _scope;
		protected DataManager DataManager;
        protected PhotoManager PhotoManager;


        public INavigationService NavigationService
        {
            get; set;
        }

        public IAlertService AlertService
        {
            get; set;
        }

        public ApiService RestApiServise
        {
            get; set;
        }
		#endregion

		#region Constructor
		public BasePresenter()
        {
			// Take container and resolve DataManager 
			_scope = BaseBootstrapper.Container.BeginLifetimeScope();
			DataManager = _scope.Resolve<DataManager>();
		    PhotoManager = _scope.Resolve<PhotoManager>();
        }
		#endregion

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
