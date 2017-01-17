using Softjourn.SJCoins.Core.API;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;
using System;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LoginPresenter : BasePresenter<ILoginView>
    {
        public ApiService RestApiServise
        {
            get; set;
        }

        public LoginPresenter()
        {

        }

        public enum ValidateCredentialsResult { FieldsAreAmpty, UserNameNotValid, PasswordNotValid, CredentialsAreValid }

        public void Login(string userName, string password)
        {

            switch (Utils.Validators.ValidateCredentials(userName, password))
            {
                case ValidateCredentialsResult.FieldsAreAmpty:
                    View.SetUsernameError(Resources.StringResources.activity_login_invalid_email);
                    break;
                case ValidateCredentialsResult.UserNameNotValid:
                    View.SetUsernameError(Resources.StringResources.activity_login_invalid_email);
                    break;
                case ValidateCredentialsResult.PasswordNotValid:
                    View.SetPasswordError(Resources.StringResources.activity_login_invalid_password);
                    break;
                default:
                    View.ShowProgress(Resources.StringResources.progress_authenticating);
                    
                    if (NetworkUtils.isConnected)
                        {
                            RestApiServise.MakeLoginRequest(userName, password, "password", new Action<Session>(OnLoginAction));
                        }
                        else
                        {
                            View.ShowNoInternetError(Resources.StringResources.internet_turned_off);
                        }
                        break;
            }
        }

        public void ToWelcomePage()
        {
            NavigationService.NavigateToAsRoot(NavigationPage.Main);
        }

        public void OnLoginAction(Session session)
        {
            AlertService.ShowToastMessage(session.AccessToken);
        }
    }
}
