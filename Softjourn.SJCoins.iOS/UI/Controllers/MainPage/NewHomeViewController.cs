using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Sources;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("NewHomeViewController")]
	public partial class NewHomeViewController: BaseViewController<HomePresenter>, IHomeView
	{
		#region Properties
		private bool pullToRefreshTrigged = false;
		private NewHomeViewSource tableSource;
		#endregion
	
		#region Constructor
		public NewHomeViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigurePage();
			Presenter.OnStartLoadingPage();
		}
		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			//AccountButton.Clicked += OnAccountClicked;
			//_delegate.HomeViewControllerDelegateFlowLayout_ItemSelected += OnItemSelected;
			//_delegate.HomeViewControllerDelegateFlowLayout_SeeAllClicked += OnSeeAllClicked;
			//_delegate.HomeViewControllerDelegateFlowLayout_BuyExecuted += OnBuyActionClicked;
			//_delegate.HomeViewControllerDelegateFlowLayout_AddDeleteFavoriteExecuted += OnFavoriteActionClicked;
		}

		public override void DetachEvents()
		{
			//AccountButton.Clicked -= OnAccountClicked;
			//_delegate.HomeViewControllerDelegateFlowLayout_ItemSelected -= OnItemSelected;
			//_delegate.HomeViewControllerDelegateFlowLayout_SeeAllClicked -= OnSeeAllClicked;
			//_delegate.HomeViewControllerDelegateFlowLayout_BuyExecuted -= OnBuyActionClicked;
			//_delegate.HomeViewControllerDelegateFlowLayout_AddDeleteFavoriteExecuted -= OnFavoriteActionClicked;
			base.DetachEvents();
		}

		public override void ShowProgress(string message)
		{
			if (!pullToRefreshTrigged)
				base.ShowProgress(message);
		}

		public override void HideProgress()
		{
			if (!pullToRefreshTrigged)
				base.HideProgress();

			pullToRefreshTrigged = false;
		}
		#endregion

		#region IHomeView implementation
		public void SetAccountInfo(Account account)
		{
			// Show user balance on start
			//SetBalance(account.Amount.ToString());
		}

		public void SetUserBalance(string balance)
		{
			// Show user balance after buying
			//SetBalance(balance);
		}

		public void SetMachineName(string name)
		{
			// Set chosenMachine name as title 
			//MachineNameLabel.Text = name;
		}

		public void ShowProducts(List<Categories> listCategories)
		{
			// Send downloaded data to dataSource and show them on view
			tableSource.Categories = listCategories;
			TableView.ReloadData();
		}

		public void LastUnavailableFavoriteRemoved(Product product)
		{
			//RefreshFavoritesCell();
		}

		public void FavoriteChanged(Product product)
		{
			//RefreshFavoritesCell();
		}
		#endregion

		#region Private methods
		private void ConfigurePage()
		{
			//Hide no items label
			NoItemsLabel.Hidden = true;
			//MachineNameLabel.Text = "";
			//MyBalanceLabel.Text = "";

			// Configure datasource and delegate
			tableSource = new NewHomeViewSource();
			tableSource.Categories = new List<Categories>();
			TableView.Source = tableSource;
			TableView.AlwaysBounceVertical = true;
		}
		#endregion

	}
}
