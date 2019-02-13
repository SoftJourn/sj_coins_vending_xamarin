namespace Softjourn.SJCoins.Core.Common.Exceptions
{
    public sealed class ApiNotAuthorizedException : ApiException
    {
        public ApiNotAuthorizedException(string message)
            : base(message)
        {
        }
    }
}
