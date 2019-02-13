using System;

namespace Softjourn.SJCoins.Core.Common.Exceptions
{
    public class CameraException : Exception
    {
        public CameraException(string message)
            : base(message)
        {
        }
    }
}
