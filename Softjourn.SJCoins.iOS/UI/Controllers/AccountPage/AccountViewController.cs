using System;
using Foundation;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.Core.Models.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.General.Helper;
using Softjourn.SJCoins.iOS.UI.Sources.AccountPage;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("AccountViewController")]
	public partial class AccountViewController : BaseViewController<AccountPresenter>, IAccountView
	{
		private AccountViewSource tableSource;
		private readonly Lazy<UIImageHelper> helper = new Lazy<UIImageHelper>(() => new UIImageHelper());
		private UIImageHelper ImageHelper => helper.Value;

        private UITapGestureRecognizer avatarImageTap; 

		public AccountViewController(IntPtr handle) : base(handle)
		{
		}

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
            MakeNavigationBarTransparent();
		}

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            MakeNavigationBarDefault();
        }

		#endregion

		#region BaseViewController

		public override void AttachEvents()
		{
			base.AttachEvents();
            DoneButton.Clicked += DoneButtonClickHandler;
			tableSource.ItemSelected += TableSource_ItemClicked;
			// Add tap gesture to avatar image
			avatarImageTap = new UITapGestureRecognizer(AvatarImageTapHandler);
			AvatarImage.AddGestureRecognizer(avatarImageTap);
		}

		public override void DetachEvents()
		{
            DoneButton.Clicked -= DoneButtonClickHandler;
			tableSource.ItemSelected -= TableSource_ItemClicked;
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
				AmountLabel.Text = account.Amount.ToString();
			}
		}

		public void ImageAcquired(byte[] receipt)
		{
            // Method triggered when data taken from server or dataManager
            var image = UIImage.LoadFromData(NSData.FromArray(receipt));

			// Set image
			if (AvatarImage.Hidden)
				AvatarImage.Hidden = false;

			AvatarImage.Image = image;
		}

		public void ImageAcquiredPlugin(byte[] receipt)
		{
			// Method trigged when data taken from plugin (camera or library)
			var image = UIImage.LoadFromData(NSData.FromArray(receipt));
			// Resize image
			var scaledRotatedImage = ImageHelper.ScaleAndRotateImage(image, image.Orientation);

			// Set image
			if (AvatarImage.Hidden)
				AvatarImage.Hidden = false;
			
			AvatarImage.Image = scaledRotatedImage;

			// Convert scaled image to byte
			var bytes = ImageHelper.BytesFromImage(scaledRotatedImage);
			// Send image to server
			if (bytes != null)
				Presenter.StoreAvatarOnServer(bytes);
		}

		//Android
		public void ImageAcquired(string receipt) { }

		#endregion

		#region Private methods

		private void ConfigureTableView()
		{
			var options = Presenter.GetOptionsList();
			tableSource = new AccountViewSource(options);
            TableView.Source = tableSource;
		}

		private static void ConfigureAvatarImage(UIImageView imageView)
		{
            // Make image rounded
			var imageCircle = imageView.Layer;
			imageCircle.CornerRadius = imageView.Frame.Height / 2;
            imageCircle.BorderWidth = 0.3f;
            imageCircle.BorderColor = UIColorConstants.ProductImageBorderColor.CGColor;
			imageCircle.MasksToBounds = true;
		}

		#endregion

        #region Event handlers

		private void TableSource_ItemClicked(object sender, AccountOption item)
		{
			Presenter.OnItemClick(item.OptionName);
		}

		private void DoneButtonClickHandler(object sender, EventArgs e)
		{
			DismissViewController(true, null);
		}

		private void AvatarImageTapHandler(UITapGestureRecognizer gestureRecognizer)
		{
			Presenter.OnPhotoClicked();
		}

		#endregion
	}
}
