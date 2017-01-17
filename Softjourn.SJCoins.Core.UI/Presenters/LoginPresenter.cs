
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LoginPresenter : BasePresenter<ILoginView>
    {

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
                        //    mModel.makeLoginCall(userName, password);
                        AlertService.ShowToastMessage("There should be call");
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
    }
}
