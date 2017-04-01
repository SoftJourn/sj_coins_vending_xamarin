using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;
using CoreGraphics;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("HomeViewController")]
	public partial class HomeViewController : BaseViewController<HomePresenter>, IHomeView
	{
		#region Properties
		public List<Categories> Categories { get; private set; }

		private HomeViewControllerDataSource _dataSource;
		private HomeViewControllerDelegateFlowLayout _delegate;
		#endregion

		#region Constructor
		public HomeViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigurePage();
			ConfigureCollectionView();
			Presenter.OnStartLoadingPage();
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
			_delegate.HomeViewControllerDelegateFlowLayout_ItemSelected += OnItemSelected;
			_delegate.HomeViewControllerDelegateFlowLayout_SeeAllClicked += OnSeeAllClicked;
			_delegate.HomeViewControllerDelegateFlowLayout_BuyExecuted += OnBuyActionClicked;
			_delegate.HomeViewControllerDelegateFlowLayout_AddDeleteFavoriteExecuted += OnFavoriteActionClicked;
		}

		public override void DetachEvents()
		{
			AccountButton.Clicked -= OnAccountClicked;
			_delegate.HomeViewControllerDelegateFlowLayout_ItemSelected -= OnItemSelected;
			_delegate.HomeViewControllerDelegateFlowLayout_SeeAllClicked -= OnSeeAllClicked;
			_delegate.HomeViewControllerDelegateFlowLayout_BuyExecuted -= OnBuyActionClicked;
			_delegate.HomeViewControllerDelegateFlowLayout_AddDeleteFavoriteExecuted -= OnFavoriteActionClicked;
			base.DetachEvents();
		}

		public override void ShowProgress(string message)
		{
		}

		public override void HideProgress()
		{
		}
		#endregion

		#region IHomeView implementation
		public void SetAccountInfo(Account account)
		{
			// Show user balance on start
			string balance = account.Amount.ToString();
			SetBalance(balance);
			BalanceLabel.Hidden = false;
		}

		public void SetUserBalance(string balance)
		{
			// Show user balance after buying
			SetBalance(balance);
		}

		public void SetMachineName(string name)
		{
			// Set chosenMachine name as title to viewController 
			NavigationItem.Title = name;
		}

		public void ShowProducts(List<Categories> listCategories)
		{
			// Save downloaded data and show them on view
			Categories = listCategories;
			_dataSource.SetCategories(listCategories);
			_delegate.SetCategories(listCategories);
			CollectionView.ReloadData();
		}

		public void LastUnavailableFavoriteRemoved(Product product)
		{
		}

		public void FavoriteChanged(Product product)
		{
			RefreshFavoritesCell();
		}
		#endregion

		#region Private methods
		private void SetBalance(string balance)
		{
			BalanceLabel.Text = "Your balance is " + balance + " coins";
		}

		private UIView ConfigureVendingMachinesHeader()
		{
			// Customize header view
			UIView view = new UIView();
			UILabel label = new UILabel(frame: new CGRect(x: 25, y: 15, width: 300, height: 20));
			label.TextAlignment = UITextAlignment.Left;
			label.Text = "Vending Machines";
			label.TextColor = UIColor.Gray;
			view.Add(label);
			return view;
		}

		private void ConfigurePage()
		{
			//Hide no items label
			NoItemsLabel.Hidden = true;
			BalanceLabel.Hidden = true;
		}

		private void ConfigureCollectionView()
		{
			// Configure datasource and delegate
			_dataSource = new HomeViewControllerDataSource();
			_delegate = new HomeViewControllerDelegateFlowLayout();

			CollectionView.DataSource = _dataSource;
			CollectionView.Delegate = _delegate;
			CollectionView.AlwaysBounceVertical = true;
		}

		private void RefreshFavoritesCell()
		{
			// TODO refactoring
			if (Categories != null)
			{
				var newList = Presenter.GetCategoriesList();
				_dataSource.SetCategories(newList);
				CollectionView.ReloadData();
				//var firstCell = CollectionView.VisibleCells[0];
				//var indexPath = CollectionView.IndexPathForCell(firstCell);
				//CollectionView.ReloadItems(new NSIndexPath[] { indexPath });
			}
		}

		// -------------------- Event handlers --------------------
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
		// --------------------------------------------------------

		// Throw CollectionView to parent
		protected override UIScrollView GetRefreshableScrollView() => CollectionView;

		protected override void PullToRefreshTriggered(object sender, System.EventArgs e)
		{
			StopRefreshing();
			Presenter.OnStartLoadingPage();
		}
		#endregion
	}

	#region UICollectionViewSource implementation
	public class HomeViewControllerDataSource : UICollectionViewDataSource
	{
		private List<Categories> categories = new List<Categories>();

		public void SetCategories(List<Categories> categories)
		{
			this.categories = categories;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => categories.Count;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => (UICollectionViewCell)collectionView.DequeueReusableCell(HomeCell.Key, indexPath);
	}
	#endregion

	#region UICollectionViewDelegateFlowLayout implementation
	public class HomeViewControllerDelegateFlowLayout : UICollectionViewDelegateFlowLayout
	{
		public event EventHandler<Product> HomeViewControllerDelegateFlowLayout_ItemSelected;
		public event EventHandler<string> HomeViewControllerDelegateFlowLayout_SeeAllClicked;
		public event EventHandler<Product> HomeViewControllerDelegateFlowLayout_BuyExecuted;
		public event EventHandler<Product> HomeViewControllerDelegateFlowLayout_AddDeleteFavoriteExecuted;

		private List<Categories> categories = new List<Categories>();
		private const int cellHeight = 180;

		public void SetCategories(List<Categories> categories)
		{
			this.categories = categories;
		}

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath) => new CGSize(collectionView.Bounds.Width, cellHeight);

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			if (categories.Count > 0)
			{
				var _cell = (HomeCell)cell;
				var category = categories[indexPath.Row];

				_cell.ConfigureWith(category);

				_cell.HomeCell_ItemSelected -= HomeViewControllerDelegateFlowLayout_ItemSelected;
				_cell.HomeCell_ItemSelected += HomeViewControllerDelegateFlowLayout_ItemSelected;

				_cell.HomeCell_SeeAllClicked -= HomeViewControllerDelegateFlowLayout_SeeAllClicked;
				_cell.HomeCell_SeeAllClicked += HomeViewControllerDelegateFlowLayout_SeeAllClicked;

				_cell.HomeCell_BuyActionExecuted -= HomeViewControllerDelegateFlowLayout_BuyExecuted;
				_cell.HomeCell_BuyActionExecuted += HomeViewControllerDelegateFlowLayout_BuyExecuted;

				_cell.HomeCell_FavoriteActionExecuted -= HomeViewControllerDelegateFlowLayout_AddDeleteFavoriteExecuted;
				_cell.HomeCell_FavoriteActionExecuted += HomeViewControllerDelegateFlowLayout_AddDeleteFavoriteExecuted;
			}
		}

		public override void CellDisplayingEnded(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (HomeCell)cell;

			_cell.HomeCell_ItemSelected -= HomeViewControllerDelegateFlowLayout_ItemSelected;
			_cell.HomeCell_SeeAllClicked -= HomeViewControllerDelegateFlowLayout_SeeAllClicked;
			_cell.HomeCell_BuyActionExecuted -= HomeViewControllerDelegateFlowLayout_BuyExecuted;
			_cell.HomeCell_FavoriteActionExecuted -= HomeViewControllerDelegateFlowLayout_AddDeleteFavoriteExecuted;
		}
	}
	#endregion
}
