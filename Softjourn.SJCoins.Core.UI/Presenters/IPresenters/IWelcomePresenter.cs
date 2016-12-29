using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.Presenters.IPresenters
{
    public interface IWelcomePresenter
    {
        void DisableWelcomePageOnLaunch();

        void ToLoginScreen();
    }
}
