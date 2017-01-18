using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Coins;
using Softjourn.SJCoins.Core.API.OAuth;
using Softjourn.SJCoins.Core.API.Vending;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Helpers;

namespace Softjourn.SJCoins.Core.API
{

    public class ApiService
    {

        public ApiClient ApiClient
        {
            get; set;
        }
        
        public ApiService()
        {

        }

        public void MakeLoginRequest(string login, string password, string type, Action<Session> action)
        {
            ApiClient.MakeLoginRequest(login, password, type, new Action<Session>((session) => 
            {
                Settings.AccessToken = session.AccessToken;
                Settings.RefreshToken = session.RefreshToken;
                action(session);
            }));
            
        }
    }
}
