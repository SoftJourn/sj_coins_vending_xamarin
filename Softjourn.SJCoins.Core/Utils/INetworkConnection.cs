using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.Utils
{
    public interface INetworkConnection
    {
        void OnInternetAppeared();
        void OnInternetDismissed();
    }
}
