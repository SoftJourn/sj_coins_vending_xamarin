using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ILaunchView : IBaseView
    {
        void ShowNoInternetError(string msg);

        void ToWelcomePage();

        void ToMainPage();

        void ToLoginPage();
    }
}
