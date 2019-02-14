using System;
using System.Diagnostics;
using Foundation;
using SDWebImage;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.iOS.General.Constants;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.HomePage
{
    [Register("PreViewController")]
    public partial class PreViewController : UIViewController
    {
        private Product CurrentProduct { get; set; }

        public event EventHandler<Product> PreViewController_BuyActionExecuted;
        public event EventHandler<Product> PreViewController_FavoriteActionExecuted;

        public override IUIPreviewActionItem[] PreviewActionItems => PreviewActions;

        private IUIPreviewActionItem[] PreviewActions
        {
            get
            {
                var action1 = PreviewActionForTitle("Buy", UIPreviewActionStyle.Default, BuyActionClicked);
                var action2 = PreviewActionForTitle(ConfigureFavoriteAction(), UIPreviewActionStyle.Default, FavoriteActionClicked);

                return new IUIPreviewActionItem[] { action1, action2 };
            }
        }

        public PreViewController(IntPtr handle) : base(handle)
        {
        }

        public void SetItem(Product item) => CurrentProduct = item;

        #region Controller Life cycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigurePageWith(CurrentProduct);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Debug.WriteLine(string.Format("{0} controller disposed", GetType()));
        }

        #endregion

        #region Private methods

        private void ConfigurePageWith(Product product)
        {
            if (product != null)
            {
                NameLabel.Text = product.Name;
                PriceLabel.Text = $"Price: {product.Price} Coins";

                if (string.IsNullOrEmpty(product.Description))
                {
                    DescriptionLabel.Text = product.Description;
                }
                else
                {
                    DescriptionLabel.TextColor = UIColor.Gray;
                    DescriptionLabel.Text = Const.defaultDescription;
                }

                Logo.SetImage(url: new NSUrl(product.ImageFullUrl), placeholder: UIImage.FromBundle(ImageConstants.Placeholder));
            }
        }

        private string ConfigureFavoriteAction() =>
            CurrentProduct.IsProductFavorite ? "Remove from favorites" : "Add to favorites";

        // -------------------- Action handlers --------------------
        private void BuyActionClicked() => PreViewController_BuyActionExecuted?.Invoke(this, CurrentProduct);

        private void FavoriteActionClicked() =>
            PreViewController_FavoriteActionExecuted?.Invoke(this, CurrentProduct);
        // --------------------------------------------------------

        #endregion

        private static UIPreviewAction PreviewActionForTitle(string title,
            UIPreviewActionStyle style = UIPreviewActionStyle.Default, Action handler = null) =>
            UIPreviewAction.Create(title, style, (action, previewViewController) => { handler?.Invoke(); });
    }
}
