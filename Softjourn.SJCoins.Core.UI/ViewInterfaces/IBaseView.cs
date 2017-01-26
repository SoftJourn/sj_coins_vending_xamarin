using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface IBaseView
    {
        void ShowProgress(string message);

        void HideProgress();
    }
}
