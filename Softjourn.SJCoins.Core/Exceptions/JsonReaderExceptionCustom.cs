using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.Exceptions
{
    public class JsonReaderExceptionCustom : Exception
    {
        public JsonReaderExceptionCustom(string message)
            : base(message)
        {
        }
    }
}
