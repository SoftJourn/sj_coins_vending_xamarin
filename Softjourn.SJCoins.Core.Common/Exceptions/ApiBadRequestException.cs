namespace Softjourn.SJCoins.Core.Common.Exceptions
{
    public sealed class ApiBadRequestException : ApiException
    {
        public ApiBadRequestException(string message)
            : base(message)
        {
        }
    }
}
