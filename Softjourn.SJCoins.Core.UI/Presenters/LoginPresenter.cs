using System;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.Utils;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class LoginPresenter : BasePresenter<ILoginView>
    {
        public async void Login(string userName, string password)
        {
            if (!Validators.IsUserNameEmpty(userName) && !Validators.IsPasswordValid(password))
            {
                View.SetPasswordError(Resources.UiMessageResources.activity_login_invalid_password);
                View.SetUsernameError(Resources.UiMessageResources.activity_login_empty_username);

                return;
            }

            if (!Validators.IsUserNameEmpty(userName))
            {
                View.SetUsernameError(Resources.UiMessageResources.activity_login_empty_username);

                return;
            }

            if (!Validators.IsPasswordValid(password))
            {
                View.SetPasswordError(Resources.UiMessageResources.activity_login_invalid_password);

                return;
            }

            if (Validators.IsPasswordValid(password) && Validators.IsUserNameEmpty(userName))
            {
                if (NetworkUtils.IsConnected)
                {
                    View.ShowProgress(Resources.UiMessageResources.progress_authenticating);

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
                            Resources.UiMessageResources.server_error_bad_username_or_password,
                            Resources.UiMessageResources.btn_title_ok, null);
                    }
                    catch (Exception ex)
                    {
                        View.HideProgress();
                        AlertService.ShowToastMessage(ex.Message);
                    }
                }
                else
                {
                    AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
                }
            }
        }

        public void ToWelcomePage() => NavigationService.NavigateToAsRoot(NavigationPage.Welcome);
    }
}
