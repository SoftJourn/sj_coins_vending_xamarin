using System.Collections.Generic;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.Models;
using Softjourn.SJCoins.Core.Models.AccountInfo;
using Softjourn.SJCoins.Core.Models.Machines;
using Softjourn.SJCoins.Core.Models.Products;
using Softjourn.SJCoins.Core.Models.TransactionReports;

namespace Softjourn.SJCoins.Core.Managers.Api
{
    public interface IApiService
    {
        #region OAuth Api calls

        Task<Session> MakeLoginRequestAsync(string userName, string password);
        Task<EmptyResponse> RevokeTokenAsync();

        #endregion;

        #region Machine Api calls

        Task<List<Machines>> GetMachinesListAsync();
        Task<Machines> GetMachineByIdAsync(string machineId);
        Task<Featured> GetFeaturedProductsAsync();
        Task<List<Product>> GetProductsList();
        Task<Amount> BuyProductById(string productId);
        Task<List<Product>> GetFavoritesList();
        Task<EmptyResponse> AddProductToFavorites(string productId);
        Task<EmptyResponse> RemoveProductFromFavorites(string productId);
        Task<List<History>> GetPurchaseHistory();

        #endregion

        #region Coins Api call

        Task<Account> GetUserAccountAsync();
        Task<Balance> GetBalanceAsync();
        Task<DepositeTransaction> GetOfflineCash(Cash scannedCode);
        Task<Report> GetTransactionReport(TransactionRequest transactionRequest);
        Task<Cash> WithdrawMoney(Amount amount);
        Task<byte[]> GetAvatarImage(string endpoint);
        Task<EmptyResponse> SetAvatarImage(byte[] image);

        #endregion
    }
}
