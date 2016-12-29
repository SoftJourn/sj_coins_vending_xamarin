using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ILaunchView
    {
        void NoInternet();

        void ToWelcomePage();

        void ToMainPage();

        void ToLoginPage();
    }
}
