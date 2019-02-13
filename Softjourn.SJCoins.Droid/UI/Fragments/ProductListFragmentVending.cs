using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Droid.UI.Activities;
using Softjourn.SJCoins.Droid.UI.Adapters;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class ProductListFragmentVending : Fragment
    {
        public string ProductsCategory;

        private const string TagProductsCategory = "PRODUCTS CATEGORY";
        private const string ArgProductsList = "PRODUCTS LIST";

        private FeaturedProductItemsAdapter _productAdapter;
        private RecyclerView.LayoutManager _layoutManager;
        private List<Product> _productList;

        private RecyclerView _machineItems;

        public static ProductListFragmentVending NewInstance(string category, int? headers, int? container, List<Product> productList)
        {
            var bundle = new Bundle();
            bundle.PutString(TagProductsCategory, category);
            string serializedList = Newtonsoft.Json.JsonConvert.SerializeObject(productList);
            bundle.PutString(ArgProductsList, serializedList);
            var fragment = new ProductListFragmentVending();
            fragment.Arguments = bundle;
            return fragment;
        }

        #region Fragment Standart Methods

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_products_list, container, false);
            _machineItems = view.FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);

            ProductsCategory = Arguments.GetString(TagProductsCategory);
            var serializedList = Arguments.GetString(ArgProductsList);

            _productList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(serializedList);
            _layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            _productAdapter = new FeaturedProductItemsAdapter(ProductsCategory, null, Activity);

            DetachEvents();
            AttachEvents();

            _machineItems.SetLayoutManager(_layoutManager);

            _machineItems.SetAdapter(_productAdapter);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            if (savedInstanceState == null)
                _productAdapter.SetData(_productList);
        }

        #endregion

        /**
         * Make adapter redraw items according to new list of Products
         * Is called by Activity only if Category is Favorites
         */
        public void ChangeFavorite(List<Product> list)
        {
            _productAdapter.SetData(list);
            _productAdapter.ChangeFavoriteIcon();
        }

        #region PrivateMethods

        /**
        * Calls TrigFavorite method on Activity fragment is attached to
        */
        private void RemoveProductFromFavorite(object sender, Product e)
        {
            ((MainActivity)Activity).TrigFavorite(e);
        }

        /**
         * Calls TrigFavorite method on Activity fragment is attached to
         */
        private void AddProductToFavorite(object sender, Product e)
        {
            ((MainActivity)Activity).TrigFavorite(e);
        }

        /**
         * OnClick on Product
         * Calls to Open Details screen
         */
        private void ProductSelected(object sender, Product product)
        {
            ((MainActivity)Activity).ShowDetails(product);
        }

        /**
         * OnLongClick on Product
         * Calls to Open Preview screen
         */
        private void ProductDetailsSelected(object sender, Product product)
        {
            ((MainActivity)Activity).ShowPreview(product);
        }

        private void AttachEvents()
        {
            if (_productAdapter == null) return;
            _productAdapter.ProductSelected += ProductSelected;
            _productAdapter.ProductDetailsSelected += ProductDetailsSelected;
            _productAdapter.AddToFavorites += AddProductToFavorite;
            _productAdapter.RemoveFromFavorites += RemoveProductFromFavorite;
        }

        private void DetachEvents()
        {
            if (_productAdapter == null) return;
            _productAdapter.ProductSelected -= ProductSelected;
            _productAdapter.ProductDetailsSelected -= ProductDetailsSelected;
            _productAdapter.AddToFavorites -= AddProductToFavorite;
            _productAdapter.RemoveFromFavorites -= RemoveProductFromFavorite;
        }
    }

    #endregion
}


