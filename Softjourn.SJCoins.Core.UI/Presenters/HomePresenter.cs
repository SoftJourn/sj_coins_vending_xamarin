using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class HomePresenter : BaseProductPresenter<IHomeView>
    {
        public void OnRefresh()
        {
            OnStartLoadingPage();
        }

        public async void OnStartLoadingPage()
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.StringResources.progress_loading);
                    DataManager.Profile = await RestApiServise.GetUserAccountAsync();
                    MyBalance = DataManager.Profile.Amount;
                    View.SetAccountInfo(DataManager.Profile);
                    View.SetMachineName(Settings.SelectedMachineName);
                    var favoritesList = await RestApiServise.GetFavoritesList();
                    var productCategoriesList = new List<Categories>();

                    // add favorites category to result list if favorites exists
                    if (favoritesList != null && favoritesList.Count > 0)
                    {
                        var favoriteCategory = new Categories();
                        favoriteCategory.Name = "Favorites";
                        favoriteCategory.Products = favoritesList;
                        productCategoriesList.Add(favoriteCategory);
                    }

                    var featuredProducts = await RestApiServise.GetFeaturedProductsAsync();

                    var featuredCategoriesList = GetCategoriesListFromFeaturedProduct(featuredProducts);

                    if (featuredCategoriesList != null && featuredCategoriesList.Count > 0)
                    {
                        // add to result array category which products list are not empty
                        foreach (var category in featuredCategoriesList)
                        {
                            if (category.Products != null && category.Products.Count > 0)
                            {
                                productCategoriesList.Add(category);
                            }
                        }
                    }
                    DataManager.ProductList = AddIsProductInCurrentMachineFlagToProducts(AddFavoriteFlagToProducts(productCategoriesList));
                    View.ShowProducts(DataManager.ProductList);
                    View.HideProgress();
                }
                catch (ApiNotAuthorizedException ex)
                {
                    View.HideProgress();
                    AlertService.ShowToastMessage(ex.Message);
                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (Exception ex)
                {
                    View.HideProgress();
                    AlertService.ShowToastMessage(ex.Message);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
        }

        public List<Product> GetProductListForGivenCategory(string category)
        {
            return DataManager.GetProductListByGivenCategory(category);
        }

        public void UpdateBalanceView()
        {
            if (DataManager.Profile != null)
            {
                View.SetUserBalance(DataManager.Profile.Amount.ToString());
            }
        }

        //Setting user's balance after buying ar grabbing new data
        public override void ChangeUserBalance(string balance)
        {
            View.SetUserBalance(balance);
        }

        //Is called when user click on Show All button for displaying another view only with product from category
        public void OnShowAllClick(string category)
        {
            NavigationService.NavigateTo(NavigationPage.ShowAll, category);
        }

        //Is called when user click on Profile button (is using only for droid)
        public void OnProfileButtonClicked()
        {
            if (DataManager.Profile != null)
            {
                NavigationService.NavigateTo(NavigationPage.Profile);
            }
        }

        //Adding Favorite flag to product before returning to View
        private List<Categories> AddFavoriteFlagToProducts(List<Categories> categoriesList)
        {
            var favorites = new List<Product>();
            foreach (var category in categoriesList)
            {
                if (category.Name != "Favorites") continue;
                favorites.AddRange(category.Products);
                break;
            }
            foreach (var category in categoriesList)
            {
                if (category.Name == "Favorites")
                {
                    foreach (var product in category.Products)
                    {
                        product.IsProductFavorite = true;
                    }
                }
                else
                {
                    foreach (var product in category.Products)
                    {
                        foreach (var favorite in favorites)
                        {
                            if (product.Id == favorite.Id)
                            {
                                product.IsProductFavorite = true;
                            }
                        }
                    }
                }
            }
            return categoriesList;
        }

        //Adding Flag IsProductInCurrentMachine to product
        private List<Categories> AddIsProductInCurrentMachineFlagToProducts(List<Categories> categoriesList)
        {
            foreach (var category in categoriesList)
            {
                if (category.Name != Const.Favorites) continue;
                foreach (var favorite in category.Products)
                {
                    foreach (var ctgr in categoriesList.Where(ctgr => ctgr.Name != Const.Favorites))
                    {
                        foreach (var product in ctgr.Products.Where(product => favorite.Id == product.Id))
                        {
                            favorite.IsProductInCurrentMachine = true;
                            break;
                        }
                    }
                }
            }
            return categoriesList;
        }

        // get list with all categories from featured product. 
        private List<Categories> GetCategoriesListFromFeaturedProduct(Featured featuredProducts)
        {
            if (featuredProducts != null)
            {
                var categoriesList = new List<Categories>(); //empty result list with Categories
                var featuredCategoriesList = featuredProducts.Categories; // categories list with products from FeaturedProducts

                if (featuredCategoriesList != null && featuredCategoriesList.Count > 0) // if featuredCategoriesList ie empty we can't create any list
                {

                    // check and add to list last added products
                    var lastAddedIdList = featuredProducts.LastAdded;
                    if (lastAddedIdList != null && lastAddedIdList.Count > 0)
                    {
                        var categoryLastAdded = new Categories();
                        categoryLastAdded.Name = "Last Added";
                        categoryLastAdded.Products = GetProductList(lastAddedIdList, featuredCategoriesList);
                        categoriesList.Add(categoryLastAdded);
                    }

                    // check and add to list best sellers products
                    var bestSellersIdList = featuredProducts.BestSellers;
                    if (bestSellersIdList != null && bestSellersIdList.Count > 0)
                    {
                        var bestSellersCategory = new Categories();
                        bestSellersCategory.Name = "Best Sellers";
                        bestSellersCategory.Products = GetProductList(bestSellersIdList, featuredCategoriesList);
                        categoriesList.Add(bestSellersCategory);
                    }

                    // add to categoriesList all categories list from featuredProducts;
                    categoriesList.AddRange(featuredCategoriesList);
                    return categoriesList;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //return list with products if we just have list with int id. Is looking for products using list of categories
        private List<Product> GetProductList(List<int> idList, List<Categories> categoriesList)
        {
            var list = new List<Product>();

            foreach (int id in idList)
            {
                foreach (var category in categoriesList)
                {
                    var productList = category.Products;
                    var product = GetProductFromListById(id, productList);
                    if (product != null)
                    {
                        list.Add(product);
                        break;
                    }
                }
            }
            return list;
        }

        // return product from list by using it id while searching
        private Product GetProductFromListById(int productId, List<Product> productsList)
        {
            foreach (var product in productsList)
            {
                if (product.Id == productId)
                {
                    return product;
                }
            }
            return null;
        }

        public List<Categories> GetCategoriesList() => DataManager.ProductList;
    }
}
