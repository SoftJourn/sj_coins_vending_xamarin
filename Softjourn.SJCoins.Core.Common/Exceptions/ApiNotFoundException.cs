namespace Softjourn.SJCoins.Core.Common.Exceptions
{
    public sealed class ApiNotFoundException : ApiException
    {
        public ApiNotFoundException(string message)
                : base(message)
        {
        }
    }
}
