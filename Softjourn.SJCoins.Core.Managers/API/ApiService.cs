using System.Collections.Generic;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.Common;
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

        #region OAuth Api calls

        public async Task<Session> MakeLoginRequestAsync(string userName, string password) => await ApiClient.MakeLoginRequestAsync(userName, password);

        public Task RevokeTokenAsync() => ApiClient.RevokeToken();

        #endregion

        #region Machine Api calls

        public async Task<List<Machines>> GetMachinesListAsync() => await ApiClient.GetMachinesListAsync();

        public async Task<Machines> GetMachineByIdAsync(string machineId) => await ApiClient.GetMachineByIdAsync(machineId);

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

        public async Task<List<Product>> GetFavoritesList() => await ApiClient.GetFavoritesListAsync();

        public Task AddProductToFavorites(string productId) => ApiClient.AddProductToFavoritesAsync(productId);

        public Task RemoveProductFromFavorites(string productId) => ApiClient.RemoveProductFromFavoritesAsync(productId);

        public async Task<List<History>> GetPurchaseHistory() => await ApiClient.GetPurchaseHistoryAsync();

        #endregion

        #region Coins Api call

        public async Task<Account> GetUserAccountAsync() => await ApiClient.GetUserAccountAsync();

        public async Task<Balance> GetBalanceAsync() => await ApiClient.GetBalanceAsync();

        public async Task<DepositeTransaction> GetOfflineCash(Cash scannedCode) => await ApiClient.GetOfflineMoney(scannedCode);

        public async Task<Cash> WithdrawMoney(Amount amount) => await ApiClient.WithdrawMoney(amount);

        public async Task<Report> GetTransactionReport(TransactionRequest transactionRequest) => await ApiClient.GetTransactionReport(transactionRequest);

        public async Task<byte[]> GetAvatarImage(string endpoint) => await ApiClient.GetAvatarImage(endpoint);

        public Task SetAvatarImage(byte[] image) => ApiClient.SetAvatarImage(image);

        #endregion
    }
}

