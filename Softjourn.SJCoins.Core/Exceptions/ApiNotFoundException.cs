
namespace Softjourn.SJCoins.Core.Exceptions
{
    public class ApiNotFoundException : ApiException
    {        
        public ApiNotFoundException(string message)
                : base(message)
        {
        }
    }
}
