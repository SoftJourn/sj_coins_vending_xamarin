using Softjourn.SJCoins.Core.Models.Products;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
	public interface IBaseProductView : IBaseView
	{
	    void FavoriteChanged(Product product);
	    void LastUnavailableFavoriteRemoved(Product product);
	}
}
