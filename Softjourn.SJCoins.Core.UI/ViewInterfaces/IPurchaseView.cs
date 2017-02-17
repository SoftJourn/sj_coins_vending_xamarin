using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IPurchaseView : IBaseView
    {
        void SetData(List<History> purchaseList);
    }
}
