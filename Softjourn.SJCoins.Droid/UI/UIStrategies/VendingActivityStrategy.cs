
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Droid.UI.Adapters;

namespace Softjourn.SJCoins.Droid.UI.UIStrategies
{
    public class VendingActivityStrategy : IProductsListFragmentStrategy
    {

        Activity _activity;
        string _productsCategory;
        RecyclerView _machineItems;
        Button _buttonSortByName;
        Button _buttonSortByPrice;
        TextView _noProductsInCategory;
        RecyclerView.LayoutManager _layoutManager;
        FeaturedProductItemsAdapter _productAdapter;

        public VendingActivityStrategy(Activity activity, string category)
        {
            _activity = activity;
            _productsCategory = category;
        }

        public View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_products_list, container, false);
            _machineItems = view.FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);

            _layoutManager = new LinearLayoutManager(_activity, LinearLayoutManager.Horizontal, false);
            _productAdapter = new FeaturedProductItemsAdapter(_productsCategory, null, _activity);

            return view;
        }

        public void OnChangeFavoriteIcon(string action)
        {

        }
    }
}