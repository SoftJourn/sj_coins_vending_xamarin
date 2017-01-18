using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.Exceptions
{
    class ApiNotAuthorizedException : Exception
    {

            public ApiNotAuthorizedException(string message)
                : base(message)
            {
            }
        }
}
