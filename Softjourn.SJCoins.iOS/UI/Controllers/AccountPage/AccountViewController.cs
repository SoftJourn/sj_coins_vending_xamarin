using System;
using System.Collections.Generic;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using SDWebImage;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Cells;
using Softjourn.SJCoins.iOS.UI.Sources;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("AccountViewController")]
	public partial class AccountViewController : BaseViewController<AccountPresenter>, IAccountView
	{
		#region Properties
		private AccountViewSource _tableSource;

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
			Presenter.GetImageFromServer();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
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
			}
		}

		public void ImageAcquired(byte[] receipt)
		{
			var helper = new UIImageHelper();

			var imageData = NSData.FromArray(receipt);
			var image = UIImage.LoadFromData(imageData);


			// Resize image
			var scaledImage = helper.ScaleImage(image);
			// Set to imageView
			AvatarImage.Image = scaledImage;

			// Convert scaled image to byte
			var bytes = helper.BytesFromImage(scaledImage);
			// Send image to server
			if (bytes != null)
				Presenter.StoreAvatarOnServer(bytes);
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

			_tableSource = new AccountViewSource(optionsFirstSection, optionsSecondSection);
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
}
