using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ILaunchView
    {
        void ShowNoInternetError(string msg);

        void ToWelcomePage();

        void ToMainPage();

        void ToLoginPage();
    }
}
