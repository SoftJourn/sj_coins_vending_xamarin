using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public class Class1 : IErrorShow
    {
        public Class1() { }
        public void ShowDialog(string title, string msg, string btnName, Action<string, string> btnClicked)
        {
            //create dialog
            btnClicked("gfgd", "df");
        }
    }
}
