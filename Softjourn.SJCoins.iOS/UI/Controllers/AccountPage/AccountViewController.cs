using System;
using System.Collections.Generic;
using CoreAnimation;
using Foundation;
using SDWebImage;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("AccountViewController")]
	public partial class AccountViewController : BaseViewController<AccountPresenter>, IAccountView
	{
		#region Properties
		private AccountSource _tableSource;

		UITapGestureRecognizer avatarImageTap; 
		#endregion

		#region Constructor
		public AccountViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigureTableView();
			ConfigureAvatarImage(AvatarImage);
			Presenter.OnStartLoadingPage();
		}
		#endregion

		#region BaseViewController
		public override void AttachEvents()
		{
			base.AttachEvents();
			DoneButton.Clicked += DoneButtonClickHandler;
			_tableSource.ItemSelected += TableSource_ItemClicked;
			// Add tap gesture to avatar image
			avatarImageTap = new UITapGestureRecognizer(AvatarImageTapHandler);
			AvatarImage.AddGestureRecognizer(avatarImageTap);
		}

		public override void DetachEvents()
		{
			DoneButton.Clicked -= DoneButtonClickHandler;
			_tableSource.ItemSelected -= TableSource_ItemClicked;
			// Remove tap gesture from avatar image
			AvatarImage.RemoveGestureRecognizer(avatarImageTap);
			base.DetachEvents();
		}
		#endregion

		#region IAccountView implementation
		public void SetAccountInfo(Account account)
		{
			if (account != null)
			{
				NameLabel.Text = account.Name + " " + account.Surname;
				AmountLabel.Text = account.Amount.ToString() + " coins";

				// TODO need change default avatar image
				AvatarImage.SetImage(url: new NSUrl("https://sjcoins-testing.softjourn.if.ua/vending/v1/products/100/image.jpeg"), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
			}
		}

		public void ImageAcquired(byte[] receipt)
		{
			// Set image to imageView
			AvatarImage.Image = UIImage.LoadFromData(NSData.FromArray(receipt));
		}

		public void ImageAcquired(string receipt)
		{
		}
		#endregion

		#region Private methods
		private void ConfigureTableView()
		{
			var options = Presenter.GetOptionsList();

			var optionsFirstSection = new List<AccountOption>(options);
			optionsFirstSection.RemoveAt(optionsFirstSection.Count - 1);

			var optionsSecondSection = new List<AccountOption>(options);
			optionsSecondSection.RemoveRange(0, optionsSecondSection.Count - 1);

			_tableSource = new AccountSource(optionsFirstSection, optionsSecondSection);
			TableView.Source = _tableSource;

			TableView.RegisterNibForCellReuse(ProductCell.Nib, ProductCell.Key);
		}

		private void ConfigureAvatarImage(UIImageView imageView)
		{
			// Make image rounded
			CALayer imageCircle = imageView.Layer;
			imageCircle.CornerRadius = 60;
			imageCircle.BorderWidth = 0.2f;
			imageCircle.MasksToBounds = true;
		}

		// -------------------- Event handlers --------------------
		private void TableSource_ItemClicked(object sender, AccountOption item)
		{
			Presenter.OnItemClick(item.OptionName);
		}

		private void DoneButtonClickHandler(object sender, EventArgs e)
		{
			DismissViewController(animated: true, completionHandler: null);
		}

		private void AvatarImageTapHandler(UITapGestureRecognizer gestureRecognizer)
		{
			Presenter.OnPhotoClicked();
		}
		// -------------------------------------------------------- 
		#endregion
	}

	#region UITableViewSource implementation
	public class AccountSource : UITableViewSource
	{
		private List<AccountOption> optionsFirstSection = new List<AccountOption>();
		private List<AccountOption> optionsSecondSection = new List<AccountOption>();

		public event EventHandler<AccountOption> ItemSelected;

		public AccountSource(List<AccountOption> first, List<AccountOption> second)
		{
			optionsFirstSection = first;
			optionsSecondSection = second;
		}

		public override nint NumberOfSections(UITableView tableView) => 2;

		public override nint RowsInSection(UITableView tableview, nint section) 
		{
			switch (section)
			{
				case 0:
					return optionsFirstSection.Count;
				case 1:
					return optionsSecondSection.Count;
				default:
					return 0;
			}
		}
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => (AccountCell)tableView.DequeueReusableCell(AccountCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var _cell = (AccountCell)cell;
            switch (indexPath.Section)
			{
				case 0:
					_cell.ConfigureWith(optionsFirstSection[indexPath.Row]);
					break;
				case 1:
					_cell.ConfigureWith(optionsSecondSection[indexPath.Row]);
					break;
			}
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			switch (indexPath.Section)
			{
				case 0:
					ItemSelected?.Invoke(this, optionsFirstSection[indexPath.Row]);
					break;
				case 1:
					ItemSelected?.Invoke(this, optionsSecondSection[indexPath.Row]);
					break;
			}
		}
	}
	#endregion
}
