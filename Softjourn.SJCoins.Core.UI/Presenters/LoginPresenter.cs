using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LoginPresenter : ILoginPresenter
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
                    _view.SetUsernameError();
                    break;
                case ValidateCredentialsResult.UserNameNotValid:
                    _view.SetUsernameError();
                    break;
                case ValidateCredentialsResult.PasswordNotValid:
                    _view.SetPasswordError();
                    break;
                default:
                    _view.ShowProgress("Authentication...");
                    
                    if (CrossConnectivity.Current.IsConnected)
                        {
                         //    mModel.makeLoginCall(userName, password);
                            _view.ShowMessage("There should be call");
                        }
                        else
                        {
                            _view.ShowNoInternetError();
                        }
                        break;
            }
        }

        public void ToWelcomePage()
        {
            _view.NavigateToWelcome();
        }
    }
}
