using System;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.Main
{
	[Register("AccountViewController")]
	public partial class AccountViewController : BaseViewController<AccountPresenter>, IAccountView
	{
		#region Properties
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
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			Presenter.OnStartLoadingPage();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);


		}
		#endregion

		#region IAccountView implementation
		public void SetAccountInfo(Account account)
		{
			Console.WriteLine("ololo");
			//AmountLabel.Text = account.Amount.ToString() + " coins";
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		#endregion
	}
}
