using System.Collections.Generic;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;

namespace Softjourn.SJCoins.Core.UI.ViewInterfaces
{
    public interface ITransactionReportView : IBaseView
    {
        void ShowEmptyView();

        void SetData(List<Transaction> transactionsList);

        void AddItemsToExistedList(List<Transaction> transactionsList);

        void SetCompoundDrawableInput(bool? isAsc);

        void SetCompoundDrawableOutput(bool? isAsc);
    }
}
