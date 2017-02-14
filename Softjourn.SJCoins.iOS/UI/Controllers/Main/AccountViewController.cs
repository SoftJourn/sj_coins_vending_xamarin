using System;
using CoreAnimation;
using Foundation;
using SDWebImage;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("AccountViewController")]
	public partial class AccountViewController : BaseViewController<AccountPresenter>, IAccountView
	{
		#region Properties
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
			ConfigureAvatarImage(AvatarImage);
			Presenter.OnStartLoadingPage();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			// Attach 
			DoneButton.Clicked += DoneButtonClickHandler;
			// Add tap gesture to avatar image
			avatarImageTap = new UITapGestureRecognizer(AvatarImageTapHandler);
			AvatarImage.AddGestureRecognizer(avatarImageTap);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

		}

		public override void ViewWillDisappear(bool animated)
		{
			// Detach 
			DoneButton.Clicked -= DoneButtonClickHandler;
			// Remove tap gesture from avatar image
			AvatarImage.RemoveGestureRecognizer(avatarImageTap);
			base.ViewWillDisappear(animated);
		}
		#endregion

		#region IAccountView implementation
		public void SetAccountInfo(Account account)
		{
			if (account != null)
			{
				NameLabel.Text = account.Name + " " + account.Surname;
				AmountLabel.Text = account.Amount.ToString() + " coins";

				AvatarImage.SetImage(url: new NSUrl("https://sjcoins-testing.softjourn.if.ua/vending/v1/products/100/image.jpeg"), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
			}
		}

		public void ImageAcquired(byte[] receipt)
		{
			// Set image to imageView
			AvatarImage.Image = UIImage.LoadFromData(NSData.FromArray(receipt));
		}
		#endregion

		#region Private methods
		private void ConfigureAvatarImage(UIImageView imageView)
		{
			// Make image rounded
			CALayer imageCircle = imageView.Layer;
			imageCircle.CornerRadius = 60;
			imageCircle.BorderWidth = 0.2f;
			imageCircle.MasksToBounds = true;
		}

		private void DoneButtonClickHandler(object sender, EventArgs e)
		{
			DismissViewController(animated: true, completionHandler: null);
		}

		private void AvatarImageTapHandler(UITapGestureRecognizer gestureRecognizer)
		{
			Presenter.OnPhotoClicked();
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion
	}
}
