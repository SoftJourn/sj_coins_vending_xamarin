using System;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
	public class AccountPresenter : BasePresenter<IAccountView>
	{
		private int _balance;

		public AccountPresenter()
		{
		}

		public void OnStartLoadingPage()
		{
			// Display account information
			View.SetAccountInfo(dataManager.Profile);   
		}
	}
}
