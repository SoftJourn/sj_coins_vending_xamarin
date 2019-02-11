using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Interfaces
{
	public interface IBaseProductView : IBaseView
	{
	    void FavoriteChanged(Product product);
	    void LastUnavailableFavoriteRemoved(Product product);
	}
}
