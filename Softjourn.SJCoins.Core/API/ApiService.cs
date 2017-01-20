using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.API.Model.Machines;

namespace Softjourn.SJCoins.Core.API
{

    public class ApiService : IApiService
    {

        public ApiClient ApiClient 
        {
            get; set;
        }
        
        public ApiService()
        {

        }

        public async Task<Session> MakeLoginRequest(string userName, string password)
        {
            return await ApiClient.MakeLoginRequest(userName, password);
        }

        public void RevokeToken()
        {
            ApiClient.RevokeToken();
        }

        public async Task<List<Machines>> GetMachinesList()
        {
            return await ApiClient.GetMachinesList();
        }

        public async Task<Machines> GetMachineById(string machineId)
        {
            return await ApiClient.GetMachineById(machineId);
        }


    }
}
