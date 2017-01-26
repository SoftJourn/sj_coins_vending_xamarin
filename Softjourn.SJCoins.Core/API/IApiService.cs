using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.API.Model.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softjourn.SJCoins.Core.API
{
    public interface IApiService
    {
        #region OAuth Api calls
        Task<Session> MakeLoginRequest(string userName, string password);
        Task<EmptyResponse> RevokeToken();
        #endregion;

        #region Machine Api calls
        Task<List<Machines>> GetMachinesList();
        Task<Machines> GetMachineById(string machineId);
        
        #endregion

        #region Coins Api call

        #endregion
    }
}
