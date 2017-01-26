using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;
using CoreGraphics;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("HomeViewController")]
	public partial class HomeViewController : UIViewController
	{
		private const int cellHeight = 180;

		#region Properties
		private List<Categories> categories;
		private List<Favorites> favorites;
		private Account account;
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

			//Presenter.StartLoading();
			ConfigureSettingButton();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// Set chosenMachine name as title to viewController 
			//NavigationItem.Title = 
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}
		#endregion

		#region IHomeView implementation
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
				Presenter.ToLoginScreen();
				Presenter.DisableWelcomePageOnLaunch();
			};
		}

		// Throw CollectionView to parent
		protected override UIScrollView GetRefreshableScrollView() => CollectionView;

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

		#region HomeViewControllerDelegate implementation
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
				if (parent.categories != null)
				{
					var category = parent.categories[indexPath.Row];
					if (category.Products != null || category.Products.Count != 0)
					{
						// set delegate to cell
						// if (category.name == favorite)
						// {
						//	configure cell with name items etc.	
						// }
					}
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
