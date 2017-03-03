
using System.Collections.Generic;
using System.Linq;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;

namespace Softjourn.SJCoins.Core.Managers
{
    public class TransactionsManager
    {
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }
        public bool IsInput { get; set; }
        public bool IsListAscending { get; set; }

        private List<Transaction> TransactionsList { get; set; }

        public TransactionsManager()
        {
            TransactionsList = new List<Transaction>();
            PagesCount = 0;
            CurrentPage = 0;
            IsListAscending = true;
            IsInput = true;
        }

        #region Public Methods
        //Setting all properties to defaults
        public void SetDefaults(string currentUser)
        {
            TransactionsList.Clear();
            PagesCount = 0;
            CurrentPage = 0;

            IsListAscending = true;
        }

        //Adding new Items to existe list of transactions
        public void AddTransactionsToExisted(List<Transaction> transactions)
        {
            TransactionsList.AddRange(transactions);
        }

        //Get All Transactions
        public List<Transaction> GetTransactions()
        {
            return IsListAscending ? GetAscendingList(null) : GetDescendingList();
        }

        //Get new portion of Transactions only
        public List<Transaction> GetTransactions(List<Transaction> transactions)
        {
            return IsListAscending ? GetAscendingList(transactions) : GetDescendingList();
        }
        #endregion

        #region Private Methods

        //Get Only ASCENDING Transactions
        //Depands on argument return transactions from whole list if argument is null
        //or return transaction filtered only from argument list if argument is not null
        private List<Transaction> GetAscendingList(List<Transaction> transactions)
        {
            return transactions == null ? TransactionsList : transactions;
        }

        //Get Only DESCENDING Transactions
        private List<Transaction> GetDescendingList()
        {
            var transactionsList = new List<Transaction>(GetAscendingList(null));
            transactionsList.Reverse();
            return transactionsList;

        }
        #endregion
    }
}
