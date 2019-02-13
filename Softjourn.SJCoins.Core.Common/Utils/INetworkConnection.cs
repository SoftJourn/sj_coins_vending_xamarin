namespace Softjourn.SJCoins.Core.Common.Utils
{
    public interface INetworkConnection
    {
        void OnInternetAppeared();

        void OnInternetDismissed();
    }
}
