using Softjourn.SJCoins.Core.API;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;
using System;
using System.Collections.Generic;
using Softjourn.SJCoins.Core.Exceptions;

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

        public async void Login(string userName, string password)
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
                    if (NetworkUtils.IsConnected)
                    {
                        View.ShowProgress(Resources.StringResources.progress_authenticating);
                        Session session;
                        List<Machines> machinesList = new List<Machines>();
                        try
                        {
                            session = await RestApiServise.ApiClient.MakeLoginRequest(userName, password);
                            NavigationService.NavigateToAsRoot(NavigationPage.Main);
                        }
                        catch (ApiBadRequestException ex)
                        {
                            AlertService.ShowToastMessage("Bad Login or Password");
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
                    break;
            }
        }

        public void ToWelcomePage()
        {
            NavigationService.NavigateToAsRoot(NavigationPage.Welcome);
        }

    }
}
