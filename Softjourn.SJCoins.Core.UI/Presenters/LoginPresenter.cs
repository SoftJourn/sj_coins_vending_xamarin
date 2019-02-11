using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;
using System;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.UI.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LoginPresenter : BasePresenter<ILoginView>
    {
        public async void Login(string userName, string password)
        {
            if (!Validators.IsUserNameEmpty(userName) && !Validators.IsPasswordValid(password))
            {
                View.SetPasswordError(Resources.StringResources.activity_login_invalid_password);
                View.SetUsernameError(Resources.StringResources.activity_login_empty_username);
                return;
            }

            if (!Validators.IsUserNameEmpty(userName))
            {
                View.SetUsernameError(Resources.StringResources.activity_login_empty_username);
                return;
            }

            if (!Validators.IsPasswordValid(password))
            {
                View.SetPasswordError(Resources.StringResources.activity_login_invalid_password);
                return;
            }

            if (Validators.IsPasswordValid(password) && Validators.IsUserNameEmpty(userName))
            {
                if (NetworkUtils.IsConnected)
                {
                    View.ShowProgress(Resources.StringResources.progress_authenticating);

                    try
                    {
                        await RestApiService.MakeLoginRequestAsync(userName, password);
                        NavigationService.NavigateToAsRoot(NavigationPage.SelectMachineFirstTime);
                        View.HideProgress();
                    }
                    catch (ApiBadRequestException)
                    {
                        View.HideProgress();
                        AlertService.ShowMessageWithUserInteraction(string.Empty,
                            Resources.StringResources.server_error_bad_username_or_password,
                            Resources.StringResources.btn_title_ok, null);
                    }
                    catch (Exception ex)
                    {
                        View.HideProgress();
                        AlertService.ShowToastMessage(ex.Message);
                    }
                }
                else
                {
                    AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
                }
            }
        }

        public void ToWelcomePage()
        {
            NavigationService.NavigateToAsRoot(NavigationPage.Welcome);
        }
    }
}
