
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LoginPresenter : BasePresenter, ILoginPresenter
    {
        private ILoginView _view;

        public LoginPresenter(ILoginView view)
        {
            _view = view;
        }

        public enum ValidateCredentialsResult { FieldsAreAmpty, UserNameNotValid, PasswordNotValid, CredentialsAreValid }

        public void Login(string userName, string password)
        {

            switch (Utils.Validators.ValidateCredentials(userName, password))
            {
                case ValidateCredentialsResult.FieldsAreAmpty:
                    _view.SetUsernameError(Resources.StringResources.activity_login_invalid_email);
                    break;
                case ValidateCredentialsResult.UserNameNotValid:
                    _view.SetUsernameError(Resources.StringResources.activity_login_invalid_email);
                    break;
                case ValidateCredentialsResult.PasswordNotValid:
                    _view.SetPasswordError(Resources.StringResources.activity_login_invalid_password);
                    break;
                default:
                    _view.ShowProgress(Resources.StringResources.progress_authenticating);
                    
                    if (NetworkUtils.isConnected)
                        {
                         //    mModel.makeLoginCall(userName, password);
                            _view.ShowMessage("There should be call");
                        }
                        else
                        {
                            _view.ShowNoInternetError(Resources.StringResources.internet_turned_off);
                        }
                        break;
            }
        }

        public void ToWelcomePage()
        {
            _view.ToWelcomePage();
        }
    }
}
