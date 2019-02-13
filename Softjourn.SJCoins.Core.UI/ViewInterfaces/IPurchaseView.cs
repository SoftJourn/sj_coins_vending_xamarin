using System.Collections.Generic;
using Softjourn.SJCoins.Core.Models;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IPurchaseView : IBaseView
    {
        void SetData(List<History> purchaseList);

        void ShowEmptyView();
    }
}
