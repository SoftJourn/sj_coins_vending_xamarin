using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("PreViewController")]
	public partial class PreViewController : UIViewController
	{
		#region Properties
		public string DetailItemTitle { get; set; }

		public override IUIPreviewActionItem[] PreviewActionItems
		{
			get
			{
				return PreviewActions;
			}
		}

		IUIPreviewActionItem[] PreviewActions
		{
			get
			{
				var action1 = PreviewActionForTitle("Buy");
				var action2 = PreviewActionForTitle("Add to favorite");
				return new IUIPreviewActionItem[] { action1, action2 };
			}
		}
		#endregion

		#region Constructor
		public PreViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (!string.IsNullOrEmpty(DetailItemTitle))
				Title.Text = DetailItemTitle;
		}

		//protected override void Dispose(bool disposing)
		//{
		//	base.Dispose(disposing);

		//	Console.WriteLine("PreViewController disposed");
		//}

		static UIPreviewAction PreviewActionForTitle(string title, UIPreviewActionStyle style = UIPreviewActionStyle.Default)
		{
			return UIPreviewAction.Create(title, style, (action, previewViewController) =>
			{
				var previewController = (PreViewController)previewViewController;
				var item = previewController?.Title;

				Console.WriteLine("{0} triggered from `DetailViewController` for item: {1}", action.Title, item);
			});
		}
		#endregion
	}
}
