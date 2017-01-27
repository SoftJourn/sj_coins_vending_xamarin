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

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("HomeViewController")]
	public partial class HomeViewController : BaseViewController<HomePresenter>, IHomeView
	{
		private const int cellHeight = 180;
		private const string confirmTitle = "Confirm Purchase";

		#region Properties
		private List<Categories> categories;
		private Account _account;
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

			CollectionView.Source = new HomeViewControllerDataSource(this);
			CollectionView.Delegate = new HomeViewControllerDelegate(this);
			CollectionView.AlwaysBounceVertical = true;

			Presenter.OnStartLoadingPage();
			ConfigureSettingButton();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}
		#endregion

		#region IHomeView implementation
		public void SetAccountInfo(Account account)
		{
			// Show user balance
			string balance = account.Amount.ToString();
			SetBalance(balance);
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
			categories = listCategories;
			CollectionView.ReloadData();
		}

		public void showPurchaseConfirmationDialog(Product product)
		{
			string price = product.Price.ToString();
			string confirmMessage = "Buy" + product.Name + "for the" + price + "coins";
			new AlertService().ShowConfirmationDialog(confirmTitle, confirmMessage, null, null);
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		public override void SetUIAppearance()
		{
		}

		public override void AttachEvents()
		{
		}

		public override void DetachEvents()
		{
		}
		#endregion

		#region Private methods
		private void SetBalance(string balance)
		{
			BalanceLabel.Text = "Your balance is" + balance + "coins";
		}

		private UIView ConfigureVendingMachinesHeader()
		{
			UIView view = new UIView();
			UILabel label = new UILabel(frame: new CGRect(x: 25, y: 15, width: 300, height: 20));
			label.TextAlignment = UITextAlignment.Left;
			label.Text = "Vending Machines";
			label.TextColor = UIColor.Gray;
			view.Add(label);
			return view;
		}

		private void ConfigureSettingButton()
		{
			// Add click event to button
			SettingButton.Clicked += (sender, e) =>
			{
				// Show SettingViewController
				Presenter.OnSettingsButtonClick(); 
			};
		}

		// Throw CollectionView to parent
		protected override UIScrollView GetRefreshableScrollView() => CollectionView;

		// Trigg presenter that user click on some product 
		private static void OnItemSelected(object sender, Product product)
		{
			//Presenter.OnProductClick(product);
		}
		#endregion

		#region UICollectionViewSource implementation
		private class HomeViewControllerDataSource : UICollectionViewSource
		{
			private HomeViewController parent;

			public HomeViewControllerDataSource(HomeViewController parent)
			{
				this.parent = parent;
			}

			public override nint NumberOfSections(UICollectionView collectionView) => parent.categories == null ? 0 : parent.categories.Count;

			public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) => collectionView.DequeueReusableCell(HomeCell.Key, indexPath) as UICollectionViewCell;
		}

		#endregion

		#region UICollectionViewDelegate implementation
		private class HomeViewControllerDelegate : UICollectionViewDelegate
		{
			private HomeViewController parent;

			public HomeViewControllerDelegate(HomeViewController parent)
			{
				this.parent = parent;
			}

			public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
			{
				var _cell = cell as HomeCell;
				var category = parent.categories[indexPath.Row];
					{
						// if (category.name == favorite)
						// {
						//	configure cell with name unavailable items etc.	
						// }
						_cell.ConfigureWith(category);
						_cell.ItemSelected -= OnItemSelected;
						_cell.ItemSelected += OnItemSelected;
					}
			}
		}
		#endregion

		#region UICollectionViewDelegateFlowLayout implementation
		private class HomeViewControllerDelegateFlowLayout : UICollectionViewDelegateFlowLayout
		{
			private HomeViewController parent;

			public HomeViewControllerDelegateFlowLayout(HomeViewController parent)
			{
				this.parent = parent;
			}

			public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath) => new CGSize(collectionView.Bounds.Width, cellHeight);
		}
		#endregion
	}
}
