using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;

namespace Softjourn.SJCoins.Core.Managers
{
    public class TransactionsManager
    {
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }

        private List<Transaction> TransactionsList { get; set; }

        public TransactionsManager()
        {
            TransactionsList = new List<Transaction>();
            PagesCount = 0;
            CurrentPage = 0;
        }

        public void AddTransactionsToExisted(List<Transaction> transactions)
        {
            TransactionsList.AddRange(transactions);
        }

        public List<Transaction> GetTransactions()
        {
            return TransactionsList;
        }
    }
}
