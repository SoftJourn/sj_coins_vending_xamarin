using Plugin.Connectivity;

namespace Softjourn.SJCoins.Core.Common.Utils
{
    public class NetworkUtils
    {
        public static bool IsConnected => CrossConnectivity.Current.IsConnected;

        public static void OnConnectionChanged(INetworkConnection listener)
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
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
