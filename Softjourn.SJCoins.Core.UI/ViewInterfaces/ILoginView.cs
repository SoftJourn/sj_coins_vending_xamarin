using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ILoginView
    {
        void SetUsernameError();

        void SetPasswordError();

        void NavigateToMain();

        void NavigateToWelcome();

        void ShowProgress(string message);

        void HideProgress();

        void ShowToastMessage(string message);

        void ShowNoInternetError();
    }
}
