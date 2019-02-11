namespace Softjourn.SJCoins.Core.Exceptions
{
    public sealed class ApiNotAuthorizedException : ApiException
    {
        public ApiNotAuthorizedException(string message)
            : base(message)
        {
        }
    }
}
