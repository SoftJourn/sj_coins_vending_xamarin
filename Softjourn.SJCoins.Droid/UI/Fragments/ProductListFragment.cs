using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Droid.UI.Adapters;
using Softjourn.SJCoins.Droid.UI.UIStrategies;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class ProductListFragment : Fragment
    {
        private string _productsCategory;

        protected bool _sortingByNameForward = false;
        protected bool _sortingByPriceForward = true;
        private static string TagProductsCategory = "PRODUCTS CATEGORY";

        //private VendingFragmentContract.Presenter mPresenter;
        private FeaturedProductItemsAdapter _productAdapter;
        private static RecyclerView.LayoutManager _layoutManager;
        private int _headers;
        private IProductsListFragmentStrategy _strategy;
        private List<Product> _productList;

        RecyclerView _machineItems;
        Button _buttonSortByName;
        Button _buttonSortByPrice;
        TextView _noProductsInCategory;


        public static ProductListFragment NewInstance(string category, int? headers, int? container, List<Product> productList)
        {
            var bundle = new Bundle();
            bundle.PutString(TagProductsCategory, category);
            bundle.PutString("HEADER", Java.Lang.String.ValueOf(headers));
            var fragment = new ProductListFragment { Arguments = bundle };
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _productsCategory = Arguments.GetString(TagProductsCategory);
            _headers = Integer.ParseInt(Arguments.GetString("HEADER"));
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (Regex.IsMatch(this.Activity.LocalClassName, ".*SeeAllActivity"))
            {
                _strategy = new ParentSeeAllActivityStrategy(Activity, _productsCategory, this);
                Activity.Title = _productsCategory;
            }
            else if (Regex.IsMatch(this.Activity.LocalClassName, ".*VendingActivity"))
            {
                _strategy = new VendingActivityStrategy(Activity, _productsCategory);
            }

            var view = _strategy.OnCreateView(inflater, container, savedInstanceState);

            _buttonSortByName.Click += OnClickSortByNameButton;
            _buttonSortByPrice.Click += OnClickSortByPriceButton;

            _machineItems.SetLayoutManager(_layoutManager);

            _machineItems.SetAdapter(_productAdapter);

            return view;
        }

        private void OnClickSortByPriceButton(object sender, EventArgs e)
        {
            if (_productList != null && _productList.Count > 0)
            {
                //SortByPrice(_sortingByPriceForward, _productsCategory, mPresenter, _buttonSortByName, _buttonSortByPrice);
            }
            SortByPrice(_sortingByPriceForward, _productsCategory, _buttonSortByName, _buttonSortByPrice);
        }

        private void OnClickSortByNameButton(object sender, EventArgs e)
        {
            if (_productList != null && _productList.Count > 0)
            {
                //SortByName(_sortingByNameForward, _productsCategory, mPresenter, _buttonSortByName, _buttonSortByPrice);
            }
            SortByName(_sortingByNameForward, _productsCategory, _buttonSortByName, _buttonSortByPrice);
        }


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            //mPresenter = new VendingFragmentPresenter(this);

            if (savedInstanceState == null)
            {
                GetLocalProductsList();
            }
        }

        /**
         * Retrieves chosen list of products from local storage
         * depends on mProductsCategory;
         */
        private void GetLocalProductsList()
        {
            //switch (_productsCategory)
            //{
            //    case Const.AllItems:
            //        //mPresenter.getLocalProductList();
            //        break;
            //    case Const.Favorites:
            //        _headers = Resource.Id.favoritesHeader;
            //        //mPresenter.getLocalFavorites();
            //        break;
            //    case Const.LastAdded:
            //        _headers = Resource.Id.newProductsHeader;
            //        //mPresenter.getLocalLastAddedProducts();
            //        break;
            //    case Const.BestSellers:
            //        _headers = Resource.Id.bestSellersHeader;
            //        //mPresenter.getLocalBestSellers();
            //        break;
            //    default:
            //        //mPresenter.getLocalCategoryProducts(_productsCategory);
            //        break;
            //}
        }

        public void SetFragmentFields(RecyclerView recyclerView, Button buttonSortName, Button buttonSortPrice,
            TextView noProducts)
        {
            _machineItems = recyclerView;
            _buttonSortByName = buttonSortName;
            _buttonSortByPrice = buttonSortPrice;
            _noProductsInCategory = noProducts;
        }

        /*      @Override
          public void setSortedData(List<Product> product)
              {
                  mProductAdapter.setData(product);
              }

              @Override
          public void changeFavoriteIcon(String action)
              {
                  _strategy.onChangeFavoriteIcon(action);
              }

              /**
               * Is used to hide category (Vending Activity case) when there is no products
               * and to show special message (See All Activity) when there is no products
               #1#
              @Override
          public void showDataAfterRemovingFavorites(List<Product> productsList)
              {
                  if (_mProductsCategory.equals(FAVORITES))
                  {

                      if (productsList != null && !productsList.isEmpty())
                      {
                          try
                          {
                              ((VendingActivity)getActivity()).showContainer(mHeaders, ((ViewGroup)getView().getParent()).getId());
                          }
                          catch (ClassCastException e)
                          {
                              e.printStackTrace();
                          }
                      }
                      else
                      {
                          hideContainer();
                      }
                  }
              }

              @Override
          public void loadData(List<Product> productsList)
              {
                  if (productsList != null && !productsList.isEmpty())
                  {
                      _productList = productsList;
                      mProductAdapter.setData(productsList);

                      try
                      {
                          ((VendingActivity)getActivity()).showContainer(mHeaders, ((ViewGroup)getView().getParent()).getId());
                      }
                      catch (ClassCastException e)
                      {
                          e.printStackTrace();
                      }
                  }
                  else
                  {
                      hideContainer();
                  }
              }*/


        public override void OnDestroy()
        {
            base.OnDestroy();
            // mPresenter.onDestroy();
        }

        /**
         * Hides Container and headers or shows special message
         * when there are no products to show in chosen category
         */

        private void HideContainer()
        {
            try
            {
                //    ((VendingActivity) Activity).HideContainer(_headers, ((ViewGroup) View.Parent).Id);
            }
            catch (ClassCastException e)
            {
                if (_noProductsInCategory != null)
                {
                    _noProductsInCategory.Visibility = ViewStates.Visible;
                }
                if (_buttonSortByName != null && _buttonSortByPrice != null)
                {
                    _buttonSortByName.Visibility = ViewStates.Gone;
                    _buttonSortByPrice.Visibility = ViewStates.Gone;
                }
            }
        }

        private void SortByName(bool isSortingForward, string productsCategory, Button buttonName, Button buttonPrice)
        {
            _sortingByPriceForward = true;
            buttonName.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.colorScreenBackground)));
            buttonPrice.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.transparent)));
            //presenter.sortByName(productsCategory, isSortingForward);
            _sortingByNameForward = !_sortingByNameForward;
        }

        private void SortByPrice(bool isSortingForward, string productsCategory, Button buttonName, Button buttonPrice)
        {
            _sortingByNameForward = true;
            buttonPrice.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.colorScreenBackground)));
            buttonName.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.transparent)));
            //presenter.sortByPrice(productsCategory, isSortingForward);
            _sortingByPriceForward = !_sortingByPriceForward;
        }
    }
}
