namespace Softjourn.SJCoins.Core.Exceptions
{
    public sealed class ApiBadRequestException : ApiException
    {
        public ApiBadRequestException(string message)
            : base(message)
        {
        }
    }
}
