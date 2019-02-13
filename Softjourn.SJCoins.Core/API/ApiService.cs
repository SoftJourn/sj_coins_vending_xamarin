using System.Collections.Generic;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.Core.Models.AccountInfo;
using Softjourn.SJCoins.Core.Models.Machines;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.Models.TransactionReports;

namespace Softjourn.SJCoins.Core.Managers.Api
{
    public sealed class ApiService : IApiService
    {
        public ApiClient ApiClient
        {
            get; set;
        }

        public ApiService() { }

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
            var machineId = Settings.SelectedMachineId;

            return await ApiClient.GetFeaturedProductsAsync(machineId);
        }

        public async Task<List<Product>> GetProductsList()
        {
            var machineId = Settings.SelectedMachineId;

            return await ApiClient.GetProductsListAsync(machineId);
        }

        public async Task<Amount> BuyProductById(string productId)
        {
            var machineId = Settings.SelectedMachineId;

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

        public async Task<Cash> WithdrawMoney(Amount amount)
        {
            return await ApiClient.WithdrawMoney(amount);
        }

        public async Task<Report> GetTransactionReport(TransactionRequest transactionRequest)
        {
            return await ApiClient.GetTransactionReport(transactionRequest);
        }

        public async Task<byte[]> GetAvatarImage(string endpoint)
        {
            return await ApiClient.GetAvatarImage(endpoint);
        }

        public async Task<EmptyResponse> SetAvatarImage(byte[] image)
        {
            return await ApiClient.SetAvatarImage(image);
        }
    }
}

