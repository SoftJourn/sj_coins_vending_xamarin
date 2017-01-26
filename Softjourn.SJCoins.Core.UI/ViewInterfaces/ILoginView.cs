using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ILoginView : IBaseView
    {
        void SetUsernameError(string message);

        void SetPasswordError(string message);
    }
}
