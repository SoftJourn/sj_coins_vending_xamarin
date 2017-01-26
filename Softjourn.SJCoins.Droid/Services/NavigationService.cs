using System;

using Android.Content;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Droid.UI.Activities;

namespace Softjourn.SJCoins.Droid.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateTo(NavigationPage page)
        {
            try
            {
                CrossCurrentActivity.Current.Activity.StartActivity(GetWithParams(page));
            }
            catch { }
        }

        public void NavigateToAsRoot(NavigationPage page)
        {
            CrossCurrentActivity.Current.Activity.StartActivity(GetWithParams(page));
            CrossCurrentActivity.Current.Activity.Finish();
        }

        private Intent GetWithParams(NavigationPage navigationParams)
        {
            var view = GetView(navigationParams);

            var data = string.Empty;
            try
            {
                data = JsonConvert.SerializeObject(navigationParams);
                view.PutExtra(utils.Const.NavigationKey, data);
            }
            catch { }
            return view;
        }

        private Intent GetView(NavigationPage page)
        {
            Intent view = null;
            switch (page)
            {
                case NavigationPage.Login:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(LoginActivity));
                    break;
                case NavigationPage.Home:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(MainActivity));
                    break;
                case NavigationPage.Welcome:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(WelcomeActivity));
                    break;
                case NavigationPage.SelectMachine:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(SelectMachineActivity));
                    break;
                default:
                    throw new ArgumentException("Not valid page");
            }

            return view;
        }
    }
}