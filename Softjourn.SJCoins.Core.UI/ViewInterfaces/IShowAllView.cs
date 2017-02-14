using System;
using System.Collections.Generic;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.Interfaces;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
	public interface IShowAllView : IBaseProductView
	{
	    void ShowSortedList(List<Product> products);
	}
}
