using Softjourn.SJCoins.Core.Models.AccountInfo;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
	public interface IAccountView : IBaseView
	{
		void SetAccountInfo(Account account); 

        void ImageAcquired(byte[] receipt);

		void ImageAcquiredPlugin(byte[] receipt); // IOS

        void ImageAcquired(string receipt); // Android
    }
}
