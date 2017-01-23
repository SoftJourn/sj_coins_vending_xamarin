﻿using Softjourn.SJCoins.Core.API;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;
using System;
using System.Collections.Generic;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.UI.Utils;

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

        public async void Login(string userName, string password)
        {
            if (!Validators.IsUserNameEmpty(userName) && !Validators.IsPasswordValid(password))
            {
                View.SetPasswordError(Resources.StringResources.activity_login_invalid_password);
                View.SetUsernameError(Resources.StringResources.activity_login_empty_username);
                return;
            }

            if (!Validators.IsUserNameValid(userName))
            {
                View.SetUsernameError(Resources.StringResources.activity_login_invalid_username);
                return;
            }

            if (!Validators.IsPasswordValid(password))
            {
                View.SetPasswordError(Resources.StringResources.activity_login_invalid_password);
                return;
            }

            if (Validators.IsPasswordValid(password) && Validators.IsUserNameValid(userName) && Validators.IsUserNameEmpty(userName))
            {
                if (NetworkUtils.IsConnected)
                {
                    View.ShowProgress(Resources.StringResources.progress_authenticating);
                    
                    try
                    {
                        await RestApiServise.MakeLoginRequest(userName, password);
                        NavigationService.NavigateToAsRoot(NavigationPage.SelectMachine);
                        View.HideProgress();
                    }
                    catch (ApiBadRequestException ex)
                    {
                        View.HideProgress();
                        AlertService.ShowMessageWithUserInteraction("Server Error", Resources.StringResources.server_error_bad_username_or_password, Resources.StringResources.btn_title_ok, null);
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
         
        public void IsPasswordValid(string password)
        {
            if (!Validators.IsPasswordValid(password))
            {
                View.SetPasswordError(Resources.StringResources.activity_login_invalid_password);
            }
        }

        public void IsUserNameValid(string userName)
        {
            if (!Validators.IsUserNameEmpty(userName))
            {
                View.SetUsernameError(Resources.StringResources.activity_login_empty_username);
                return;
            }
            if (!Validators.IsUserNameValid(userName))
            {
                View.SetUsernameError(Resources.StringResources.activity_login_invalid_username);
                return;
            }
        }

        public void ToWelcomePage()
        {
            NavigationService.NavigateToAsRoot(NavigationPage.Welcome);
        }

    }
}
