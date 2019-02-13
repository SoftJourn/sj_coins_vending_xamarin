using System;

namespace Softjourn.SJCoins.Core.Common.Exceptions
{
    public class JsonReaderExceptionCustom : Exception
    {
        public JsonReaderExceptionCustom(string message)
            : base(message)
        {
        }
    }
}
