﻿using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class HomePresenter : BasePresenter<IHomeView>
    {
        private int _balance;

        public HomePresenter()
        {

        }

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
                    dataManager.Profile = await RestApiServise.GetUserAccountAsync();
                    _balance = dataManager.Profile.Amount;
                    View.SetAccountInfo(dataManager.Profile);
                    View.SetMachineName(Settings.SelectedMachineName);
                    List<Product> favoritesList = await RestApiServise.GetFavoritesList();
                    List<Categories> productCategoriesList = new List<Categories>();

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
                    dataManager.ProductList = AddFavoriteFlagToProducts(productCategoriesList);
                    View.ShowProducts(dataManager.ProductList);
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

        // Called when Settings btn is clicked
        public void OnSettingsButtonClick()
        {
            NavigationService.NavigateTo(NavigationPage.Settings);
        }

        // show purchase dialog with proposal to purchase product
        public void OnProductClick(Product product)
        {
            Action<Product> OnPurchaseAction = new Action<Product>(OnProductPurchased);

            AlertService.ShowPurchaseConfirmationDialod(product, OnPurchaseAction);
        }

        //Trig adding or removing product from favorite category depends on current state of product.
        public async void OnFavoriteClick(Product product)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    if (product.IsProductFavorite)
                    {
                        await RestApiServise.AddProductToFavorites(product.Id.ToString());
                        dataManager.AddProductToFavorite(product);
                    }
                    else
                    {
                        await RestApiServise.RemoveProductFromFavorites(product.Id.ToString());
                        dataManager.RemoveProductFromFavorite(product);
                    }
                }
                catch (ApiNotAuthorizedException ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (Exception ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
        }

        //Is called when user click on Profile button (is using only for droid)
        public void OnProfileButtonClicked()
        {
            NavigationService.NavigateTo(NavigationPage.Profile);
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
                            product.IsProductFavorite = product.Id == favorite.Id;
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
                List<Categories> categoriesList = new List<Categories>(); //empty result list with Categories
                List<Categories> featuredCategoriesList = featuredProducts.Categories; // categories list with products from FeaturedProducts

                if (featuredCategoriesList != null && featuredCategoriesList.Count > 0) // if featuredCategoriesList ie empty we can't create any list
                {

                    // check and add to list last added products
                    List<int> lastAddedIdList = featuredProducts.LastAdded;
                    if (lastAddedIdList != null && lastAddedIdList.Count > 0)
                    {
                        Categories categoryLastAdded = new Categories();
                        categoryLastAdded.Name = "Last Added";
                        categoryLastAdded.Products = GetProductList(lastAddedIdList, featuredCategoriesList);
                        categoriesList.Add(categoryLastAdded);
                    }

                    // check and add to list best sellers products
                    List<int> bestSellersIdList = featuredProducts.BestSellers;
                    if (bestSellersIdList != null && bestSellersIdList.Count > 0)
                    {
                        Categories bestSellersCategory = new Categories();
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
            List<Product> list = new List<Product>();

            foreach (int id in idList)
            {
                foreach (var category in categoriesList)
                {
                    List<Product> productList = category.Products;
                    Product product = GetProductFromListById(id, productList);
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

        // check is balance enough and make purchase
        private async void OnProductPurchased(Product product)
        {
            if (NetworkUtils.IsConnected)
            {
                if (_balance >= product.IntPrice)
                {
                    View.ShowProgress(Resources.StringResources.progress_buying);
                    try
                    {
                        Amount leftAmount = await RestApiServise.BuyProductById(product.Id.ToString());
                        if (leftAmount != null) // them set new balance amount
                        {
                            _balance = int.Parse(leftAmount.Balance);
                            View.SetUserBalance(leftAmount.Balance);
                        }
                        View.HideProgress();
                        AlertService.ShowMessageWithUserInteraction("Purchase",
                            Resources.StringResources.activity_product_take_your_order_message,
                            Resources.StringResources.btn_title_ok, null);
                    }

                    catch (ApiNotAuthorizedException ex)
                    {
                        View.HideProgress();
                        AlertService.ShowToastMessage(ex.Message);
                        NavigationService.NavigateToAsRoot(NavigationPage.Login);
                    }
                    catch (ApiNotFoundException ex)
                    {
                        View.HideProgress();
                        AlertService.ShowMessageWithUserInteraction("Error", ex.Message,
                            Resources.StringResources.btn_title_ok, null);
                    }
                    catch (Exception ex)
                    {
                        View.HideProgress();
                        AlertService.ShowToastMessage(ex.Message);
                    }
                }
                else
                {
                    AlertService.ShowMessageWithUserInteraction("Error",
                        Resources.StringResources.error_not_enough_money, Resources.StringResources.btn_title_ok, null);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
        }
    }
}
