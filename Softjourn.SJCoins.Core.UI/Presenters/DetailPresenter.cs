using System;
using System.Collections.Generic;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
	public class DetailPresenter : BaseProductPresenter<IDetailView>
	{
		public DetailPresenter()
		{
		}

		public override void ChangeUserBalance(string balance)
		{

		}

	    public Product GetProduct(int productID)
	    {
	        return DataManager.GetProductFromListById(productID);
	    }
    }
}
