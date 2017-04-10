
using System;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
	public class DetailPresenter : BaseProductPresenter<IDetailView>
	{
		public DetailPresenter()
		{
            MyBalance = DataManager.Profile.Amount;
        }

		public override void ChangeUserBalance(string balance)
		{
			DataManager.Profile.Amount = Int32.Parse(balance);
		}

	    public Product GetProduct(int productID)
	    {
	        return DataManager.GetProductFromListById(productID);
	    }
    }
}
