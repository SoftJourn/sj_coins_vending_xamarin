
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using System;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using Softjourn.SJCoins.Core.API;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.Utils;
using Softjourn.SJCoins.Core.Exceptions;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class BasePresenter<TView> : IBasePresenter where TView : class, IBaseView
    {
        protected const int oneMb = 1048576;

		#region Properties
		protected ILifetimeScope Scope;
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
			Scope = BaseBootstrapper.Container.BeginLifetimeScope();
			DataManager = Scope.Resolve<DataManager>();
		    PhotoManager = Scope.Resolve<PhotoManager>();
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

        protected virtual void AvatarImageAcquired(byte[] receipt)
        {

        }

        #endregion

        public void SetNavigationParams(string navigationData)
        {
            throw new NotImplementedException();
        }

        public async void GetAvatarImage(string endpoint)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    byte[] image;
                    if (DataManager.Avatar == null)
                    {
                        // Avatar not stored
                        image = await RestApiServise.GetAvatarImage(endpoint.Substring(1));

                        if (image.Length < oneMb)
                        {
                            // Store in data manager
                            DataManager.Avatar = image;
                        }
                    }
                    else
                        // Avatar stored
                        image = DataManager.Avatar;

                    AvatarImageAcquired(image);
                }
                catch (ApiBadRequestException)
                {
                    AlertService.ShowMessageWithUserInteraction("", Resources.StringResources.server_error_bad_username_or_password, Resources.StringResources.btn_title_ok, null);
                }
                catch (Exception ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
        }
    }
}
