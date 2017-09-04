using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Sources;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("NewHomeViewController")]
    public partial class NewHomeViewController: BaseViewController<HomePresenter>, IHomeView, IDisposable
	{
		#region Properties
		public List<Categories> Categories { get; private set; }

		private bool pullToRefreshTrigged = false;
		private NewHomeViewSource tableSource = new NewHomeViewSource();
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
            TableView.Alpha = 0;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			RefreshFavoritesCell();
			Presenter.UpdateBalanceView();
		}
		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			AccountButton.Clicked += OnAccountClicked;
			tableSource.NewHomeViewSource_ItemSelected += OnItemSelected;
			tableSource.NewHomeViewSource_SeeAllClicked += OnSeeAllClicked;
			tableSource.NewHomeViewSource_BuyExecuted += OnBuyActionClicked;
			tableSource.NewHomeViewSource_AddDeleteFavoriteExecuted += OnFavoriteActionClicked;
		}

		public override void DetachEvents()
		{
			AccountButton.Clicked -= OnAccountClicked;
			tableSource.NewHomeViewSource_ItemSelected -= OnItemSelected;
			tableSource.NewHomeViewSource_SeeAllClicked -= OnSeeAllClicked;
			tableSource.NewHomeViewSource_BuyExecuted -= OnBuyActionClicked;
			tableSource.NewHomeViewSource_AddDeleteFavoriteExecuted -= OnFavoriteActionClicked;
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
			SetBalance(account.Amount.ToString());
		}

		public void SetUserBalance(string balance)
		{
			// Show user balance after buying
			SetBalance(balance);
		}

		public void SetMachineName(string name)
		{
			// Set chosenMachine name as title 
			MachineNameLabel.Text = name;
		}

		public void ShowProducts(List<Categories> listCategories)
		{
			Categories = listCategories;
			// Send downloaded data to dataSource and show them on view
			tableSource.Categories = Categories;
			TableView.ReloadData();
            UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveEaseIn, () => { TableView.Alpha = 1.0f; }, null);
		}

		public void LastUnavailableFavoriteRemoved(Product product)
		{
			RefreshFavoritesCell();
		}

		public void FavoriteChanged(Product product)
		{
			RefreshFavoritesCell();
		}
		#endregion

		#region Private methods
		private void ConfigurePage()
		{
			//Hide no items label
			NoItemsLabel.Hidden = true;
			MachineNameLabel.Text = "";
			MyBalanceLabel.Text = "";

			// Configure datasource and delegate
			TableView.Source = tableSource;
			TableView.AlwaysBounceVertical = true;
		}

		private void SetBalance(string balance)
		{
			MyBalanceLabel.Text = "Your balance: " + balance + " coins";		
		}

		private void RefreshFavoritesCell()
		{
			// TODO refactoring 
			if (Categories != null)
			{
				var newList = Presenter.GetCategoriesList();
				tableSource.Categories = newList;
				TableView.ReloadData();
			}
		}
		#endregion

		#region Event handlers
		public void OnAccountClicked(object sender, EventArgs e)
		{
			// Trigg presenter that user click on account
			Presenter.OnProfileButtonClicked();
		}

		public void OnItemSelected(object sender, Product product)
		{
			// Trigg presenter that user click on some product for showing details controllers
			Presenter.OnProductDetailsClick(product.Id);
		}

		public void OnSeeAllClicked(object sender, string categoryName)
		{
			// Trigg presenter that user click on SeeAll button 
			Presenter.OnShowAllClick(categoryName);
		}

		public void OnBuyActionClicked(object sender, Product product)
		{
			// Trigg presenter that user click Buy action on preview page 
			Presenter.OnBuyProductClick(product);
		}

		public void OnFavoriteActionClicked(object sender, Product product)
		{
			// Trigg presenter that user click Favorite action on preview page 
			Presenter.OnFavoriteClick(product);
		}
		#endregion

		#region Throw TableView to parent 
		protected override UIScrollView GetRefreshableScrollView() => TableView;

		protected override void PullToRefreshTriggered(object sender, System.EventArgs e)
		{
			StopRefreshing();
			pullToRefreshTrigged = true;
			Presenter.OnStartLoadingPage();
		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			System.Diagnostics.Debug.WriteLine(String.Format("{0} object disposed", this.GetType()));
			base.Dispose(disposing);
		}
	}
}
