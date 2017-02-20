
using System.Collections.Generic;
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

        public async Task<Session> MakeLoginRequestAsync(string userName, string password)
        {
            return await ApiClient.MakeLoginRequestAsync(userName, password);
        }

        public async Task<EmptyResponse> RevokeTokenAsync()
        {
            return await ApiClient.RevokeToken();
        }

        public async Task<List<Machines>> GetMachinesListAsync()
        {
            return await ApiClient.GetMachinesListAsync();
        }

        public async Task<Machines> GetMachineByIdAsync(string machineId)
        {
            return await ApiClient.GetMachineByIdAsync(machineId);
        }

        public async Task<Featured> GetFeaturedProductsAsync()
        {
            string machineId = Settings.SelectedMachineId;
            return await ApiClient.GetFeaturedProductsAsync(machineId);
        }

        public async Task<List<Product>> GetProductsList()
        {
            string machineId = Settings.SelectedMachineId;
            return await ApiClient.GetProductsListAsync(machineId);
        }

        public async Task<Amount> BuyProductById(string productId)
        {
            string machineId = Settings.SelectedMachineId;
            return await ApiClient.BuyProductByIdAsync(machineId, productId);
        }

        public async Task<List<Product>> GetFavoritesList()
        {
            return await ApiClient.GetFavoritesListAsync();
        }

        public async Task<EmptyResponse> AddProductToFavorites(string productId)
        {
            return await ApiClient.AddProductToFavoritesAsync(productId);
        }

        public async Task<EmptyResponse> RemoveProductFromFavorites(string productId)
        {
            return await ApiClient.RemoveProductFromFavoritesAsync(productId);
        }

        public async Task<List<History>> GetPurchaseHistory()
        {
            return await ApiClient.GetPurchaseHistoryAsync();
        }

        public async Task<Account> GetUserAccountAsync()
        {
            return await ApiClient.GetUserAccountAsync();
        }

        public async Task<Balance> GetBalanceAsync()
        {
            return await ApiClient.GetBalanceAsync();
        }

        public async Task<DepositeTransaction> GetOfflineCash(Cash scannedCode)
        {
            return await ApiClient.GetOfflineMoney(scannedCode);
        }
    }
}
