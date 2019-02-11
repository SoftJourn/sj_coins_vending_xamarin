namespace Softjourn.SJCoins.Core.Exceptions
{
    public sealed class ApiNotFoundException : ApiException
    {
        public ApiNotFoundException(string message)
                : base(message)
        {
        }
    }
}
