using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;
using CoreGraphics;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Services;
using Softjourn.SJCoins.iOS.General.Constants;
using System.Linq;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("HomeViewController")]
	public partial class HomeViewController : BaseViewController<HomePresenter>, IHomeView
	{
		#region Properties
		public List<Categories> Categories { get; private set; } = new List<Categories>();

		private HomeViewControllerDataSource _dataSource;
		private bool launched;
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
			// Attach
			AccountButton.Clicked += OnAccountClicked;
			RefreshFavoritesCell();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			// Detach
			AccountButton.Clicked -= OnAccountClicked;
			base.ViewWillDisappear(animated);
		}
		#endregion

		#region IHomeView implementation
		public void SetAccountInfo(Account account)
		{
			// Show user balance
			string balance = account.Amount.ToString();
			SetBalance(balance);
			BalanceLabel.Hidden = false;
		}

		public void SetUserBalance(string balance)
		{
			// Show user balance
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
			CollectionView.ReloadData();
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
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
			_dataSource = new HomeViewControllerDataSource(Categories);

			CollectionView.DataSource = _dataSource;
			CollectionView.Delegate = new HomeViewControllerDelegateFlowLayout(this);
			CollectionView.AlwaysBounceVertical = true;
		}

		private void RefreshFavoritesCell()
		{
			if (Categories.Count != 0)
			{
				var newFavorites = Presenter.GetProductListForGivenCategory(Const.FavoritesCategory);
				_dataSource.RefreshFavorites(newFavorites);
				CollectionView.ReloadData();
				//var firstCell = CollectionView.VisibleCells[0];
				//var indexPath = CollectionView.IndexPathForCell(firstCell);
				//CollectionView.ReloadItems(new NSIndexPath[] { indexPath });
			}
		}

		public void FavoriteChanged(bool isFavorite)
		{
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
		// --------------------------------------------------------

		// Throw CollectionView to parent
		protected override UIScrollView GetRefreshableScrollView() => CollectionView;
		#endregion
	}

	#region UICollectionViewSource implementation
	public class HomeViewControllerDataSource : UICollectionViewDataSource
	{
		private List<Categories> categories;

		public HomeViewControllerDataSource(List<Categories> categories)
		{
			this.categories = new List<Categories>(categories.ToList());
		}

		public void SetCategories(List<Categories> categories)
		{
			this.categories = categories;
		}

		public void RefreshFavorites(List<Product> favorites)
		{
			foreach (var category in categories)
			{
				if (category.Name == Const.FavoritesCategory)
				{
					category.Products = favorites;
					if (category.Products.Count == 0)
					{
						// Remove Favorites category
						categories.RemoveAt(0);
					}
					else {
						// Create and insert Favorites Category
						var favoriteCategory = new Categories()
						{
							Name = Const.FavoritesCategory,
							Products = favorites
						};
						categories.Insert(0, favoriteCategory);
					}
					break;
				}
			}
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => categories.Count;

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => (UICollectionViewCell)collectionView.DequeueReusableCell(HomeCell.Key, indexPath);
	}
	#endregion

	#region UICollectionViewDelegateFlowLayout implementation
	public class HomeViewControllerDelegateFlowLayout : UICollectionViewDelegateFlowLayout
	{
		private const int cellHeight = 180;
		private HomeViewController parent;

		public HomeViewControllerDelegateFlowLayout(HomeViewController parent)
		{
			this.parent = parent;
		}

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath) => new CGSize(collectionView.Bounds.Width, cellHeight);

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (HomeCell)cell;
			var category = parent.Categories[indexPath.Row];

			// Create delegate object with event and throw it to cell
			var _delegate = new HomeCellDelegate(category.Products);
			_delegate.ItemSelectedEvent -= parent.OnItemSelected;
			_delegate.ItemSelectedEvent += parent.OnItemSelected;

			// Add seeAll event
			_cell.SeeAllClickedEvent -= parent.OnSeeAllClicked;
			_cell.SeeAllClickedEvent += parent.OnSeeAllClicked;

			// TODO Add functionality with unavailable product

			_cell.ConfigureWith(category, _delegate);
		}
	}
	#endregion
}
