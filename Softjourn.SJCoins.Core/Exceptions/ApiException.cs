using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.Exceptions
{
    public class ApiException : Exception
    {
        public int ErrorCode
        {
            get; set;
        }

        public ApiException(string message)
            : base(message)
        {
        }

        public ApiException(int code, string message)
            : base(message)
        {
            ErrorCode = code;
        }

    }

}
