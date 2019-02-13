using System;
using Android.Content;
using Plugin.CurrentActivity;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Droid.UI.Activities;

namespace Softjourn.SJCoins.Droid.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateTo(NavigationPage page, object obj = null)
        {
            try
            {
                CrossCurrentActivity.Current.Activity.StartActivity(GetWithParams(page, obj));
            }
            catch { }
        }

        public void NavigateToAsRoot(NavigationPage page)
        {
            CrossCurrentActivity.Current.Activity.StartActivity(GetWithParams(page));
            CrossCurrentActivity.Current.Activity.Finish();
        }

        /**
         * Method for getting data from Intent if exists
         */
        private static Intent GetWithParams(NavigationPage navigationParams, object obj = null)
        {
            var view = GetView(navigationParams);

            var data = string.Empty;
            if (obj == null) return view;
            try
            {
                data = obj.ToString();
                view.PutExtra(Constant.NavigationKey, data);
            }
            catch
            {
            }

            return view;
        }

        private static Intent GetView(NavigationPage page)
        {
            Intent view = null;
            switch (page)
            {
                case NavigationPage.Login:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(LoginActivity));
                    break;
                case NavigationPage.Home:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(MainActivity));
                    view.AddFlags(ActivityFlags.ClearTask);
                    break;
                case NavigationPage.Welcome:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(WelcomeActivity));
                    break;
                case NavigationPage.SelectMachine:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(SelectMachineActivity));
                    break;
                case NavigationPage.SelectMachineFirstTime:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(SelectMachineActivity));
                    break;
                case NavigationPage.Profile:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(ProfileActivity));
                    view.AddFlags(ActivityFlags.ForwardResult);
                    break;
                case NavigationPage.ShowAll:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(ShowAllActivity));
                    view.AddFlags(ActivityFlags.NewTask);
                    break;
                case NavigationPage.Detail:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(DetailsActivity));
                    break;
                case NavigationPage.Purchase:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(PurchaseActivity));
                    break;
                case NavigationPage.Reports:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(ReportsActivity));
                    break;
                case NavigationPage.PrivacyTerms:
                    //view = new Intent(CrossCurrentActivity.Current.Activity, typeof(PrivaceTermsActivity));
                    break;
                case NavigationPage.Help:
                    //view = new Intent(CrossCurrentActivity.Current.Activity, typeof(HelpActivity));
                    break;
                case NavigationPage.ShareFuns:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(QrActivity));
                    break;
                default:
                    throw new ArgumentException("Not valid page");
            }

            return view;
        }
    }
}