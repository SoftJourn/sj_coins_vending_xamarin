using System;

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
