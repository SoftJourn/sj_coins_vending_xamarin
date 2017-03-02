using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ITransactionReportView : IBaseView
    {
        void ShowEmptyView();

        void SetData(List<Transaction> transactionsList);

        void AddItemsToExistedList(List<Transaction> transactionsList);
    }
}
