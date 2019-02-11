using System;

namespace Softjourn.SJCoins.Core.Exceptions
{
    public class CameraException : Exception
    {
        public CameraException(string message)
            : base(message)
        {
        }
    }
}
