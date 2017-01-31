using Softjourn.SJCoins.Core.API.Model.AccountInfo;
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
            try
            {
                View.ShowProgress(Resources.StringResources.progress_loading);
                Account userAccount = await RestApiServise.GetUserAccountAsync();
                _balance = userAccount.Amount;
                View.SetAccountInfo(userAccount);
                View.SetMachineName(Settings.SelectedMachineName);
                List<Product> favoritesList = await RestApiServise.GetFavoritesList();
                List<Categories> productCategoriesList = new List<Categories>();

                // add favorites category to result list if favorites exists
                if (favoritesList != null && favoritesList.Count > 0)
                {
                    Categories favoriteCategory = new Categories();
                    favoriteCategory.Name = "Favorites";
                    favoriteCategory.Products = favoritesList;
                    productCategoriesList.Add(favoriteCategory);
                }

                Featured featuredProducts = await RestApiServise.GetFeaturedProductsAsync();

                List<Categories> featuredCategoriesList = GetCategoriesListFromFeaturedProduct(featuredProducts);

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

                View.ShowProducts(productCategoriesList);
                View.HideProgress();
            } catch (ApiNotAuthorizedException ex)
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

        // Called when Settings btn is clicked
        public void OnSettingsButtonClick()
        {
            NavigationService.NavigateTo(NavigationPage.Settings);
        }

        public void OnProductClick(Product product)
        {
            Action<Product> OnPurchaseAction = new Action<Product>(OnProductPurchased);

            AlertService.ShowPurchaseConfirmationDialod(product, OnPurchaseAction);
        }

        private async void OnProductPurchased(Product product)
        {
            if (_balance >= product.IntPrice)
            {
                View.ShowProgress(Resources.StringResources.progress_buying);
                Amount leftAmount = await RestApiServise.BuyProductById(product.Id.ToString());
                if (leftAmount != null)
                {
                    _balance = int.Parse(leftAmount.Balance);
                    View.SetUserBalance(leftAmount.Balance);
                }
                View.HideProgress();
            } else
            {
                AlertService.ShowMessageWithUserInteraction("Error", Resources.StringResources.error_not_enough_money, Resources.StringResources.btn_title_ok, null);
            }
        }
    }
}
