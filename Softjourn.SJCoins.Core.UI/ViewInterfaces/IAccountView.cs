using System;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
	public interface IAccountView : IBaseView
	{
		void SetAccountInfo(Account account);

        void ImageAcquired(byte[] receipt);

        void ImageAcquired(string receipt);
    }
}
