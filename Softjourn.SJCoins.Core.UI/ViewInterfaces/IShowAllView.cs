using System.Collections.Generic;
using Softjourn.SJCoins.Core.Models.Products;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
	public interface IShowAllView : IBaseProductView
	{
	    void ShowSortedList(List<Product> products);

        void SetCompoundDrawableName(bool? isAsc);

        void SetCompoundDrawablePrice(bool? isAsc);
    }
}
