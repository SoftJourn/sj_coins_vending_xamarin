﻿using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
	public class DetailPresenter : BaseProductPresenter<IDetailView>
	{
		public DetailPresenter()
		{
            MyBalance = DataManager.Profile.Amount;
        }

        public override void ChangeUserBalance(string balance) => DataManager.Profile.Amount = int.Parse(balance);

        public Product GetProduct(int productId) => DataManager.GetProductFromListById(productId);
    }
}
