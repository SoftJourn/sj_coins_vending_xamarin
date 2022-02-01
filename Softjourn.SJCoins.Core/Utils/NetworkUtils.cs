using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Softjourn.SJCoins.Core.Utils
{
    public class NetworkUtils
    {
        public static bool IsConnected
        {
            get { return Connectivity.NetworkAccess == NetworkAccess.Internet; }
        }

        public static void OnConnectionChanged(INetworkConnection listener)
        {
            Connectivity.ConnectivityChanged += (sender, args) =>
                {
                    if (IsConnected)
                    {
                        listener?.OnInternetAppeared();
                    }
                    else
                    {
                        listener?.OnInternetDismissed();
                    }
                };

        }
    }
}
