using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Products;
using UIKit;
using CoreGraphics;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("HomeViewController")]
	public partial class HomeViewController : UIViewController
	{
		private const int cellHeight = 180;

		#region Properties
		private List<Categories> categories;
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

		#region BaseViewController -> IBaseView implementation
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
