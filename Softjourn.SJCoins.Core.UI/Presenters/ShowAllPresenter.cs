using System;
using System.Collections.Generic;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
	public class ShowAllPresenter : BaseProductPresenter<IShowAllView>
	{
		public ShowAllPresenter()
		{
            MyBalance = DataManager.Profile.Amount;
        }

		public override void ChangeUserBalance(string balance)
		{
		}

	    public List<Product> GetProductList(string category)
	    {
	        return DataManager.GetProductListByGivenCategory(category);
	    } 
	}
}
