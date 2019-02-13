using System;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public abstract class BaseProductPresenter<TView> : BasePresenter<TView> where TView : class, IBaseProductView
    {
        public int MyBalance { get; set; }

        protected override void AvatarImageAcquired(byte[] receipt) { }

        /// <summary>
        /// Trig adding or removing product from favorite category depends on current state of product.
        /// </summary>
        /// <param name="product"></param>
        public async void OnFavoriteClick(Product product)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    if (product.IsProductFavorite)
                    {
                        // Execute remove favorite call
                        await RestApiService.RemoveProductFromFavorites(product.Id.ToString());
                        // Remove favorite locally
                        DataManager.RemoveProductFromFavorite(product);
                        product.IsProductFavorite = false;
                        // Trigg view that process success
                        if (DataManager.GetProductFromListById(product.Id) != null)
                        {
                            View.FavoriteChanged(DataManager.GetProductFromListById(product.Id));
                        }
                        else
                        {
                            View.LastUnavailableFavoriteRemoved(product);
                        }
                    }
                    else
                    {
                        // Execute add favorite call
                        await RestApiService.AddProductToFavorites(product.Id.ToString());
                        // Add favorite locally
                        DataManager.AddProductToFavorite(product);
                        // Trigg view that process success 
                        var prod = DataManager.GetProductFromListById(product.Id);

                        View.FavoriteChanged(prod);
                    }
                }
                catch (ApiNotAuthorizedException)
                {
                    //AlertService.ShowToastMessage(ex.Message);
                    DataManager.Profile = null;
                    Settings.ClearUserData();
                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (ApiNotFoundException ex)
                {
                    AlertService.ShowToastMessage(ex.Message);
                    View.FavoriteChanged(DataManager.ChangeProductsFavoriteStatus(product));
                }
                catch (NotImplementedException)
                {

                }
                catch (NullReferenceException)
                {

                }
                catch (Exception e)
                {
                    AlertService.ShowToastMessage(e.Message);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
                View.FavoriteChanged(DataManager.GetProductFromListById(product.Id));
            }
        }

        /// <summary>
        /// Show purchase dialog with proposal to purchase product
        /// </summary>
        /// <param name="product"></param>
        public void OnBuyProductClick(Product product)
        {
            // Create action with trig OnPurchased method after it execution
            var onPurchaseAction = new Action<Product>(OnProductPurchased);
            // Show confirmation dialog on view
            AlertService.ShowPurchaseConfirmationDialod(product, onPurchaseAction);
        }

        public abstract void ChangeUserBalance(string balance);

        /// <summary>
        /// Is called when user clicks on Product to show Detail page of chosen product.
        /// </summary>
        /// <param name="productId"></param>
        public void OnProductDetailsClick(int productId)
        {
            NavigationService.NavigateTo(NavigationPage.Detail, productId);
        }

        #region Private methods

        /// <summary>
        /// Check is balance enough and make purchase
        /// </summary>
        /// <param name="product"></param>
        private async void OnProductPurchased(Product product)
        {
            if (NetworkUtils.IsConnected)
            {
                if (MyBalance >= product.IntPrice)
                {
                    View.ShowProgress(Resources.UiMessageResources.progress_buying);
                    try
                    {
                        var leftAmount = await RestApiService.BuyProductById(product.Id.ToString());
                        if (leftAmount != null) // them set new balance amount
                        {
                            MyBalance = int.Parse(leftAmount.Balance);
                            ChangeUserBalance(MyBalance.ToString());
                        }
                        View.HideProgress();
                        AlertService.ShowMessageWithUserInteraction("Purchase",
                            Resources.UiMessageResources.activity_product_take_your_order_message,
                            Resources.UiMessageResources.btn_title_ok, null);
                    }

                    catch (ApiNotAuthorizedException ex)
                    {
                        View.HideProgress();
                        //AlertService.ShowToastMessage(ex.Message);
                        DataManager.Profile = null;
                        Settings.ClearUserData();
                        NavigationService.NavigateToAsRoot(NavigationPage.Login);
                    }
                    catch (ApiNotFoundException ex)
                    {
                        View.HideProgress();
                        AlertService.ShowMessageWithUserInteraction("Error", ex.Message,
                            Resources.UiMessageResources.btn_title_ok, null);
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
                    Resources.UiMessageResources.error_not_enough_money, Resources.UiMessageResources.btn_title_ok, null);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.UiMessageResources.internet_turned_off);
            }
        }

        #endregion
    }
}
