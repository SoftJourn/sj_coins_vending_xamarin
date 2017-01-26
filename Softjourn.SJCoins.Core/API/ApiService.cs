using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Helpers;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Core.API.Model.AccountInfo;

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

        public async Task<EmptyResponse> RevokeToken()
        {
            return await ApiClient.RevokeToken();
        }

        public async Task<List<Machines>> GetMachinesList()
        {
            return await ApiClient.GetMachinesList();
        }

        public async Task<Machines> GetMachineById(string machineId)
        {
            return await ApiClient.GetMachineById(machineId);
        }

        public async Task<Featured> GetFeaturesProducts()
        {
            string machineId = Settings.SelectedMachineId;
            return await ApiClient.GetFeaturedProducts(machineId);
        }

        public async Task<Featured> GetFeaturedProducts()
        {
            string machineId = Settings.SelectedMachineId;
            return await ApiClient.GetFeaturedProducts(machineId);
        }

        public async Task<List<Product>> GetProductsList()
        {
            string machineId = Settings.SelectedMachineId;
            return await ApiClient.GetProductsList(machineId);
        }

        public async Task<Amount> BuyProductById(string productId)
        {
            string machineId = Settings.SelectedMachineId;
            return await ApiClient.BuyProductById(machineId, productId);
        }

        public async Task<List<Favorites>> GetFavoritesList()
        {
            return await ApiClient.GetFavoritesList();
        }

        public async Task<EmptyResponse> AddProductToFavorites(string productId)
        {
            return await ApiClient.AddProductToFavorites(productId);
        }

        public async Task<EmptyResponse> RemoveProductFromFavorites(string productId)
        {
            return await ApiClient.RemoveProductFromFavorites(productId);
        }

        public async Task<List<History>> GetPurchaseHistory()
        {
            return await ApiClient.GetPurchaseHistory();
        }

        public async Task<Account> GetUserAccountAsync()
        {
            return await ApiClient.GetUserAccountAsync();
        }

        public async Task<Balance> GetBalanceAsync()
        {
            return await ApiClient.GetBalanceAsync();
        }
    }
}
