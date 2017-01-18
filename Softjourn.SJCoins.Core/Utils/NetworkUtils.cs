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
        public static bool IsConnected
        {
            get { return CrossConnectivity.Current.IsConnected; }
        }

        public static void OnConnectionChanged(INetworkConnection listener)
        {
                CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
                {
                    if (IsConnected)
                    {
                        if (listener != null)
                        {
                            listener.OnInternetAppeared();
                        }
                    }
                    else
                    {
                        if (listener != null)
                        {
                            listener.OnInternetDismissed();
                        }
                    }
                };

        }
}
}
