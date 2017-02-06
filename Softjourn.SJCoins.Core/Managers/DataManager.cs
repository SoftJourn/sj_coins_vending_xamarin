using System.Collections.Generic;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;
using Softjourn.SJCoins.Core.API.Model.Products;

namespace Softjourn.SJCoins.Core.Managers
{
	public class DataManager
	{
		#region Properties
		public Account Profile { get; set; }
	    public List<Categories> ProductList { get; set; }
	    #endregion
	}
}
