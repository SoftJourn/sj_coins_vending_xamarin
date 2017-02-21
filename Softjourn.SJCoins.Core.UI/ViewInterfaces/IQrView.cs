using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IQrView : IBaseView
    {
        void UpdateBalance(string remain);

        void ShowSuccessFunding();

        void SetEditFieldError(string message);

        void ShowImage(byte[] image);
    }
}
