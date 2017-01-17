using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model;

namespace Softjourn.SJCoins.Core.API.OAuth
{
    public class OAuthApiClient : IOAuthApiProvider
    {
        public OAuthApiClient()
        {

        }

        public void MakeLoginRequest(string email, string password, string type, Action<Session> callback)
        {
            throw new NotImplementedException();
        }

        public void MakeRefreshToken(string email, string password, string type, Action<Session> callback)
        {
            throw new NotImplementedException();
        }
    }
}
