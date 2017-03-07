using System;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.UI.Interfaces;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
	public abstract class BaseProductPresenter<TView> : BasePresenter<TView> where TView : class, IBaseProductView
	{
		#region Properties
		private int _balance;
		public int MyBalance
		{
			get { return _balance; }
			set { _balance = value; }
		}
		#endregion

		#region Public methods
		//Trig adding or removing product from favorite category depends on current state of product.
		public async void OnFavoriteClick(Product product)
		{
			if (NetworkUtils.IsConnected)
			{
				try
				{
					if (product.IsProductFavorite)
					{
						// Execute remove favorite call
						await RestApiServise.RemoveProductFromFavorites(product.Id.ToString());
						// Remove favorite locally
						DataManager.RemoveProductFromFavorite(product);
                        // Trigg view that process success 
                        View.FavoriteChanged(DataManager.GetProductFromListById(product.Id).IsProductFavorite);
					}
					else
					{
						// Execute add favorite call
						await RestApiServise.AddProductToFavorites(product.Id.ToString());
						// Add favorite locally
						DataManager.AddProductToFavorite(product);
                        // Trigg view that process success 
                        View.FavoriteChanged(DataManager.GetProductFromListById(product.Id).IsProductFavorite);
                    }
				}
				catch (ApiNotAuthorizedException ex)
				{
					AlertService.ShowToastMessage(ex.Message);
					NavigationService.NavigateToAsRoot(NavigationPage.Login);
				}
				catch (Exception ex)
				{
					//AlertService.ShowToastMessage(ex.Message);
				}
			}
			else
			{
				AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
			}
		}

		// show purchase dialog with proposal to purchase product
		public void OnBuyProductClick(Product product)
		{
			// Create action with trigg OnPurchased method after it execution
			var onPurchaseAction = new Action<Product>(OnProductPurchased);
			// Show confirmation dialog on view
			AlertService.ShowPurchaseConfirmationDialod(product, onPurchaseAction);
		}

		public abstract void ChangeUserBalance(string balance);

        //Is called when user clicks on Product to show Detail page of chosen product.
        public void OnProductDetailsClick(int productID)
        {
            NavigationService.NavigateTo(NavigationPage.Detail, productID);
        }

        //public abstract void BuyingSuccess();

        //public abstract void FavoriteSuccess();

        #endregion

        #region Private methods
        // check is balance enough and make purchase
        private async void OnProductPurchased(Product product)
		{
			if (NetworkUtils.IsConnected)
			{
				if (MyBalance >= product.IntPrice)
				{
					View.ShowProgress(Resources.StringResources.progress_buying);
					try
					{
						var leftAmount = await RestApiServise.BuyProductById(product.Id.ToString());
						if (leftAmount != null) // them set new balance amount
						{
							MyBalance = int.Parse(leftAmount.Balance);
							ChangeUserBalance(MyBalance.ToString());
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
		#endregion
	}
}
