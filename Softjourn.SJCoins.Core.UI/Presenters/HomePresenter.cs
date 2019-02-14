﻿using System;
using System.Collections.Generic;
using System.Linq;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class HomePresenter : BaseProductPresenter<IHomeView>
    {
        public void OnRefresh() => OnStartLoadingPage();

        public async void OnStartLoadingPage()
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.UiMessageResources.progress_loading);
                    DataManager.Profile = await RestApiService.GetUserAccountAsync();
                    MyBalance = DataManager.Profile.Amount;
                    GetAvatarImage(DataManager.Profile.Image);
                    View.SetAccountInfo(DataManager.Profile);
                    View.SetMachineName(Settings.SelectedMachineName);

                    var favoritesList = await RestApiService.GetFavoritesList();
                    var productCategoriesList = new List<Categories>();

                    // add favorites category to result list if favorites exists
                    if (favoritesList != null && favoritesList.Count > 0)
                    {
                        var favoriteCategory = new Categories { Name = "Favorites", Products = favoritesList };
                        productCategoriesList.Add(favoriteCategory);
                    }

                    var featuredProducts = await RestApiService.GetFeaturedProductsAsync();
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
                    DataManager.SetNutritionFactsInCorrectOrder();
                    View.ShowProducts(DataManager.ProductList);
                    View.HideProgress();
                }
                catch (ApiNotAuthorizedException)
                {
                    View.HideProgress();
                    DataManager.Profile = null;
                    Settings.ClearUserData();
                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (Exception ex)
                {
                    View.HideProgress();
                    View.ServiceNotAvailable();
                    AlertService.ShowToastMessage(ex.Message);
                }
            }
            else
            {
                View.HideProgress();
                AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
            }
        }

        public List<Product> GetProductListForGivenCategory(string category) =>
            DataManager.GetProductListByGivenCategory(category);

        public void UpdateBalanceView()
        {
            if (DataManager.Profile != null)
                View.SetUserBalance(DataManager.Profile.Amount.ToString());
        }

        /// <summary>
        /// Setting user's balance after buying ar grabbing new data
        /// </summary>
        /// <param name="balance"></param>
        public override void ChangeUserBalance(string balance) => View.SetUserBalance(balance);

        /// <summary>
        /// Is called when user click on Show All button for displaying another view only with product from category
        /// </summary>
        /// <param name="category"></param>
        public void OnShowAllClick(string category) => NavigationService.NavigateTo(NavigationPage.ShowAll, category);

        /// <summary>
        /// Is called when user click on Profile button (is using only for droid)
        /// </summary>
        public void OnProfileButtonClicked()
        {
            if (DataManager.Profile != null && NetworkUtils.IsConnected)
                NavigationService.NavigateTo(NavigationPage.Profile);
        }

        public void GetImageFromServer()
        {
            if (DataManager.Profile != null)
            {
                GetAvatarImage(DataManager.Profile.Image);
                View.SetAccountInfo(DataManager.Profile);
            }
        }

        /// <summary>
        /// Adding Favorite flag to product before returning to View
        /// </summary>
        /// <param name="categoriesList"></param>
        /// <returns></returns>
        private static List<Categories> AddFavoriteFlagToProducts(List<Categories> categoriesList)
        {
            var favorites = new List<Product>();
            foreach (var category in categoriesList)
            {
                if (category.Name != Constant.Favorites) continue;
                favorites.AddRange(category.Products);
                break;
            }

            foreach (var category in categoriesList)
            {
                if (category.Name == Constant.Favorites)
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

        /// <summary>
        /// Adding Flag IsProductInCurrentMachine to product
        /// </summary>
        /// <param name="categoriesList"></param>
        /// <returns></returns>
        private static List<Categories> AddIsProductInCurrentMachineFlagToProducts(List<Categories> categoriesList)
        {
            foreach (var category in categoriesList)
            {
                if (category.Name != Constant.Favorites) continue;
                foreach (var favorite in category.Products)
                {
                    foreach (var ctgr in categoriesList.Where(ctgr => ctgr.Name != Constant.Favorites))
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

        /// <summary>
        /// get list with all categories from featured product. 
        /// </summary>
        /// <param name="featuredProducts"></param>
        /// <returns></returns>
        private static List<Categories> GetCategoriesListFromFeaturedProduct(Featured featuredProducts)
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
                        var categoryLastAdded = new Categories
                        {
                            Name = "Last Added",
                            Products = GetProductList(lastAddedIdList, featuredCategoriesList)
                        };
                        categoriesList.Add(categoryLastAdded);
                    }

                    // check and add to list best sellers products
                    var bestSellersIdList = featuredProducts.BestSellers;
                    if (bestSellersIdList != null && bestSellersIdList.Count > 0)
                    {
                        var bestSellersCategory = new Categories
                        {
                            Name = "Best Sellers",
                            Products = GetProductList(bestSellersIdList, featuredCategoriesList)
                        };
                        categoriesList.Add(bestSellersCategory);
                    }

                    // add to categoriesList all categories list from featuredProducts;
                    categoriesList.AddRange(featuredCategoriesList);

                    return categoriesList;
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// return list with products if we just have list with int id. Is looking for products using list of categories
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="categoriesList"></param>
        /// <returns></returns>
        private static List<Product> GetProductList(IEnumerable<int> idList, IReadOnlyCollection<Categories> categoriesList)
        {
            var list = new List<Product>();

            foreach (var id in idList)
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

        /// <summary>
        /// return product from list by using it id while searching
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productsList"></param>
        /// <returns></returns>
        private static Product GetProductFromListById(int productId, IEnumerable<Product> productsList)
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

        protected override void AvatarImageAcquired(byte[] receipt) => View.ImageAcquired(receipt);
    }
}
