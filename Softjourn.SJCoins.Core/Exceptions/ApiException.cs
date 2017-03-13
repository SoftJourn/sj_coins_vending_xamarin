using System;

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
