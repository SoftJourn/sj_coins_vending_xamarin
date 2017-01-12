using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.Utils
{
    public class NetworkUtils
    {
        public static bool isConnected
        {
            get { return CrossConnectivity.Current.IsConnected; }
        }
    }
}
