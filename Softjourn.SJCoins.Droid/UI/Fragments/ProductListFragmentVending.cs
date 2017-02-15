
using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Droid.UI.Activities;
using Softjourn.SJCoins.Droid.UI.Adapters;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class ProductListFragmentVending : Fragment
    {
        public string ProductsCategory;
        private const string TagProductsCategory = "PRODUCTS CATEGORY";

        private FeaturedProductItemsAdapter _productAdapter;
        private static RecyclerView.LayoutManager _layoutManager;
        private List<Product> _productList;

        RecyclerView _machineItems;

        public static ProductListFragmentVending NewInstance(string category, int? headers, int? container, List<Product> productList)
        {
            Bundle bundle = new Bundle();
            bundle.PutString(TagProductsCategory, category);
            var fragment = new ProductListFragmentVending(productList) { Arguments = bundle };
            return fragment;
        }

        public ProductListFragmentVending(List<Product> productList)
        {
            _productList = productList;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ProductsCategory = Arguments.GetString(TagProductsCategory);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_products_list, container, false);
            _machineItems = view.FindViewById<RecyclerView>(Resource.Id.list_items_recycler_view);

            _layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            _productAdapter = new FeaturedProductItemsAdapter(ProductsCategory, null, Activity);

            _productAdapter.ProductSelected -= ProductSelected;
            _productAdapter.ProductSelected += ProductSelected;

            _productAdapter.ProductDetailsSelected -= ProductDetailsSelected;
            _productAdapter.ProductDetailsSelected += ProductDetailsSelected;

            _productAdapter.AddToFavorites -= AddProductToFavorite;
            _productAdapter.AddToFavorites += AddProductToFavorite;

            _productAdapter.RemoveFromFavorites -= RemoveProductFromFavorite;
            _productAdapter.RemoveFromFavorites += RemoveProductFromFavorite;

            _machineItems.SetLayoutManager(_layoutManager);

            _machineItems.SetAdapter(_productAdapter);

            return view;
        }

        private void RemoveProductFromFavorite(object sender, Product e)
        {
            ((MainActivity)Activity).TrigFavorite(e);
        }

        private void AddProductToFavorite(object sender, Product e)
        {
            ((MainActivity)Activity).TrigFavorite(e);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            if (savedInstanceState == null)
            {
                _productAdapter.SetData(_productList);
            }
        }

        public void SetFragmentFields(RecyclerView recyclerView, Button buttonSortName, Button buttonSortPrice,
            TextView noProducts)
        {
            _machineItems = recyclerView;
        }

        public void AttachEvents()
        {
            if (_productAdapter != null)
            {
                _productAdapter.ProductSelected += ProductSelected;
            }
        }

        public void DetachEvents()
        {
            if (_productAdapter != null)
            {
                _productAdapter.ProductSelected -= ProductSelected;
            }
        }

        public void ChangeFavorite(List<Product> list)
        {
            _productAdapter.SetData(list);
            _productAdapter.NotifyDataChanges();
        }

        private void ProductSelected(object sender, Product product)
        {
            ((MainActivity)Activity).ShowDetails(product);
        }

        private void ProductDetailsSelected(object sender, Product product)
        {
            ((MainActivity)Activity).ShowPreview(product);
        }
    }
}


