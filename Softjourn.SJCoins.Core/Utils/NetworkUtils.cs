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

        public static void onConnectionChanged(INetworkConnection listener)
        {
            if (listener != null) {
                CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
                {
                    if (isConnected)
                    {
                        listener.OnInternetAppeared();
                    }
                    else
                    {
                        listener.OnInternetDismissed();
                    }
                };
            };
        }
}
}
