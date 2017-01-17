using Softjourn.SJCoins.Core.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API.OAuth
{
    public interface IOAuthApiProvider
    {
        void MakeLoginRequest(string email, string password, string type, Action<Session> callback);
        void MakeRefreshToken(string email, string password, string type, Action<Session> callback);
    }
}
