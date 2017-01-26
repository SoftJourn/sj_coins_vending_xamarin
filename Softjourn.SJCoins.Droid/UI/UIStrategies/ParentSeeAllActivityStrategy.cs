using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Droid.UI.Adapters;
using Softjourn.SJCoins.Droid.UI.Fragments;

namespace Softjourn.SJCoins.Droid.UI.UIStrategies
{
    /**
          * Strategy implementation for case of SeeAll activity should be as container for fragment
          */
    public class ParentSeeAllActivityStrategy : IProductsListFragmentStrategy
    {
        Activity _activity;
        string _productsCategory;
        RecyclerView _machineItems;
        Button _buttonSortByName;
        Button _buttonSortByPrice;
        TextView _noProductsInCategory;
        RecyclerView.LayoutManager _layoutManager;
        FeaturedProductItemsAdapter _productAdapter;
        private Fragment _fragment;

        public ParentSeeAllActivityStrategy(Activity activity, string category, Fragment fragment)
        {
            _activity = activity;
            _productsCategory = category;
            _fragment = fragment;
        }

        public View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_see_all_snacks_drinks, container, false);
            _machineItems = view.FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);
            _buttonSortByName = view.FindViewById<Button>(Resource.Id.button_sort_name);
            _buttonSortByPrice = view.FindViewById<Button>(Resource.Id.button_sort_price);
            _noProductsInCategory = view.FindViewById<TextView>(Resource.Id.textViewNoProductsInCategory);


            _layoutManager = new LinearLayoutManager(_activity, LinearLayoutManager.Vertical, false);
            //_productAdapter = new FeaturedProductItemsAdapter(_productsCategory, Const.SeeAllSnacksDrinksRecyclerView, _activity);

            //if (_productsCategory == Const.Favorites)
            {
                if (_buttonSortByName != null)
                {
                    _buttonSortByName.Visibility = ViewStates.Gone;
                }
                if (_buttonSortByPrice != null)
                {
                    _buttonSortByPrice.Visibility = ViewStates.Gone;
                }
            }
            //((SeeAllActivity)_activity).ProductsList(_productAdapter, _productsCategory);
            //((SeeAllActivity)_activity).SetButtons(_buttonSortByName, _buttonSortByPrice);
            //((ProductListFragment) _fragment).SetFragmentFields(_machineItems, _buttonSortByName, _buttonSortByPrice,
            //    _noProductsInCategory);
            return view;
        }


        public void OnChangeFavoriteIcon(String action)
        {
            //((SeeAllActivity)_activity).HideProgress();
            //if (action == Const.ActionAddFavorite || (action != Const.ActionAddFavorite && _productsCategory == Const.Favorites))
            {
                _productAdapter.NotifyDataChanges();
            }
        }
    }
}