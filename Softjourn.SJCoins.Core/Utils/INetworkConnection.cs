namespace Softjourn.SJCoins.Core.Utils
{
    public interface INetworkConnection
    {
        void OnInternetAppeared();
        void OnInternetDismissed();
    }
}
