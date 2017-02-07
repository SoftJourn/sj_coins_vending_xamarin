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
				var action1 = PreviewActionForTitle("Default Action");
				var action2 = PreviewActionForTitle("Destructive Action", UIPreviewActionStyle.Destructive);

				var subAction1 = PreviewActionForTitle("Sub Action 1");
				var subAction2 = PreviewActionForTitle("Sub Action 2");
				var groupedActions = UIPreviewActionGroup.Create("Sub Actions…", UIPreviewActionStyle.Default, new[] { subAction1, subAction2 });

				return new IUIPreviewActionItem[] { action1, action2, groupedActions };
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
