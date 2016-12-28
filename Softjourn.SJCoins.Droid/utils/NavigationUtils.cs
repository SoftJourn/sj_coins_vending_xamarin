using System;

using Android.App;
using Android.Content;
using Softjourn.SJCoins.Droid.ui.activities;

namespace TrololoWorld.utils
{
    public class NavigationUtils
    {
        //public static void GoToNoInternetScreen(Context context)
        //{
        //    var intent = new Intent(context, typeof(NoInternetActivity));
        //    context.StartActivity(intent);
        //}

        //public static void GoToVendingActivity(Context context)
        //{
        //    var intent = new Intent(context, typeof(VendingActivity));
        //    intent.AddFlags(ActivityFlags.ClearTop);
        //    context.StartActivity(intent);
        //}

        public static void GoToLoginActivity(Context context)
        {
            var intent = new Intent(context, typeof(LoginActivity));
            intent.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }

        //public static void GoToProfileActivity(Context context)
        //{
        //    var intent = new Intent(context, typeof(ProfileActivity));
        //    intent.AddFlags(ActivityFlags.LaunchedFromHistory);
        //    context.StartActivity(intent);
        //}

        //public static void GoToSeeAllActivity(Context context, String category)
        //{
        //    var intent = new Intent(context, typeof(SeeAllActivity));
        //    intent.PutExtra(Const.ExtrasCategory, category);
        //    context.StartActivity(intent);
        //}

        //public static void GoToWelcomeActivity(Context context)
        //{
        //    var intent = new Intent(context, typeof(WelcomeActivity));
        //    context.StartActivity(intent);
        //}

        /**
         * Is used to hadle Navigation in NavBar menu.
         * Is loading needed fragment to the SeeAll Activity.
         *
         * @param position = position of item in NavBar Menu.
         * @param activity = Activity where fragment sould be displayed
         * @param category = Category name to be displayed in case chosen item
         *                 is for dynamic category.
         */
        //public static void NavigationOnCategoriesSeeAll(int position, Activity activity, string category)
        //{
        //    switch (position)
        //    {
        //        case 0:
        //            activity.FragmentManager.BeginTransaction()
        //                    .Replace(Resource.Id.container_for_see_all_products, ProductListFragment.NewInstance(Const.AllItems, 0, 0), Const.TagAllProductsFragment)
        //                    .Commit();
        //            break;
        //        case 1:
        //            activity.FragmentManager.BeginTransaction()
        //                    .Replace(Resource.Id.container_for_see_all_products, ProductListFragment.NewInstance(Const.Favorites, 0, 0), Const.TagFavoritesFragment)
        //                    .Commit();
        //            break;
        //        case 2:
        //            activity.FragmentManager.BeginTransaction()
        //                    .Replace(Resource.Id.container_for_see_all_products, ProductListFragment.NewInstance(Const.LastAdded, 0, 0), Const.TagProductsLastAddedFragment)
        //                    .Commit();
        //            break;
        //        case 3:
        //            activity.FragmentManager.BeginTransaction()
        //                    .Replace(Resource.Id.container_for_see_all_products, ProductListFragment.NewInstance(Const.BestSellers, 0, 0), Const.TagProductsBestSellersFragment)
        //                    .Commit();
        //            break;
        //        default:
        //            activity.FragmentManager.BeginTransaction()
        //                    .Replace(Resource.Id.container_for_see_all_products, ProductListFragment.NewInstance(category, 0, 0), category)
        //                    .Commit();
        //            break;
        //    }
        //}
    }
}