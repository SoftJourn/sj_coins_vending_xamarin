using System;
using Android.Content;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Droid.UI.Activities;

namespace Softjourn.SJCoins.Droid.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateTo(NavigationPage page, Object obj = null)
        {
            try
            {
                Xamarin.Essentials.Platform.CurrentActivity.StartActivity(GetWithParams(page, obj));
            }
            catch { }
        }

        public void NavigateToAsRoot(NavigationPage page)
        {
            Xamarin.Essentials.Platform.CurrentActivity.StartActivity(GetWithParams(page));
            Xamarin.Essentials.Platform.CurrentActivity.Finish();
        }

        /**
         * Method for getting data from Intent if exists
         */
        private Intent GetWithParams(NavigationPage navigationParams, Object obj = null)
        {
            var view = GetView(navigationParams);

            var data = string.Empty;
            if (obj == null) return view;
            try
            {
                data = obj.ToString();
                view.PutExtra(Utils.Const.NavigationKey, data);
            }
            catch
            {
            }
            return view;
        }

        private Intent GetView(NavigationPage page)
        {
            Intent view = null;
            switch (page)
            {
                case NavigationPage.Login:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(LoginActivity));
                    break;
                case NavigationPage.Home:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(MainActivity));
                    view.AddFlags(ActivityFlags.ClearTask);
                    break;
                case NavigationPage.Welcome:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(WelcomeActivity));
                    break;
                case NavigationPage.SelectMachine:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(SelectMachineActivity));
                    break;
                case NavigationPage.SelectMachineFirstTime:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(SelectMachineActivity));
                    break;
                case NavigationPage.Profile:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(ProfileActivity));
                    view.AddFlags(ActivityFlags.ForwardResult);
                    break;
                case NavigationPage.ShowAll:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(ShowAllActivity));
                    view.AddFlags(ActivityFlags.NewTask);
                    break;
                case NavigationPage.Detail:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(DetailsActivity));
                    break;
                case NavigationPage.Purchase:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(PurchaseActivity));
                    break;
                case NavigationPage.Reports:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(ReportsActivity));
                    break;
                case NavigationPage.PrivacyTerms:
                    //view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(PrivaceTermsActivity));
                    break;
                case NavigationPage.Help:
                    //view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(HelpActivity));
                    break;
                case NavigationPage.ShareFuns:
                    view = new Intent(Xamarin.Essentials.Platform.AppContext, typeof(QrActivity));
                    break;
                default:
                    throw new ArgumentException("Not valid page");
            }
            return view;
        }
    }
}