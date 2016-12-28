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

        public void Login(string userName, string password)
        {
            const int fieldsAreAmpty = 1;
            const int userNameNotValid = 2;
            const int passwordNotValid = 3;

            switch (Utils.Validators.ValidateCredentials(userName, password))
            {
                case fieldsAreAmpty:
                    _view.SetUsernameError();
                    break;
                case userNameNotValid:
                    _view.SetUsernameError();
                    break;
                case passwordNotValid:
                    _view.SetPasswordError();
                    break;
                default:
                    _view.ShowProgress("Authentication...");
                    
                    if (CrossConnectivity.Current.IsConnected)
                        {
                         //    mModel.makeLoginCall(userName, password);
                            _view.ShowToastMessage("There should be call");
                        }
                        else
                        {
                            _view.ShowNoInternetError();
                        }
                        break;
            }
        }
    }
}
