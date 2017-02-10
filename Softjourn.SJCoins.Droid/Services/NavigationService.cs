using System;
using System.Collections.Generic;
using Android.Content;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Droid.UI.Activities;

namespace Softjourn.SJCoins.Droid.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateTo(NavigationPage page, Object obj=null)
        {
            try
            {
                CrossCurrentActivity.Current.Activity.StartActivity(GetWithParams(page,obj));
            }
            catch { }
        }

        public void NavigateToAsRoot(NavigationPage page)
        {
            CrossCurrentActivity.Current.Activity.StartActivity(GetWithParams(page));
            CrossCurrentActivity.Current.Activity.Finish();
        }

        public void NavigationToShowAll(string category)
        {
            var view = new Intent(CrossCurrentActivity.Current.Activity, typeof(ShowAllActivity));
            view.PutExtra("PRODUCTS CATEGORY", category);
            CrossCurrentActivity.Current.Activity.StartActivity(view);
        }

        public void NavigationToDetails(Product product)
        {
            var view = new Intent(CrossCurrentActivity.Current.Activity, typeof(DetailsActivity));
            //view.PutExtra("PRODUCT", product);
            CrossCurrentActivity.Current.Activity.StartActivity(view);
        }

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
                case NavigationPage.Settings:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(SelectMachineActivity));
                    break;
                case NavigationPage.Profile:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(ProfileActivity));
                    break;
                case NavigationPage.ShowAll:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(ShowAllActivity));
                    break;
                case NavigationPage.Detail:
                    view = new Intent(CrossCurrentActivity.Current.Activity, typeof(DetailsActivity));
                    break;
                default:
                    throw new ArgumentException("Not valid page");
            }

            return view;
        }
    }
}