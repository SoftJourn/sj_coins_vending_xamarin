using System.Collections.Generic;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;

namespace Softjourn.SJCoins.Core.Managers
{
    public class TransactionsManager
    {
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }
        public bool IsInput { get; set; }
        public bool IsListAscending { get; set; }

        private List<Transaction> TransactionsList { get; }

        public TransactionsManager()
        {
            TransactionsList = new List<Transaction>();
            PagesCount = 0;
            CurrentPage = 0;
            IsListAscending = true;
            IsInput = true;
        }

        /// <summary>
        /// Setting all properties to defaults
        /// </summary>
        /// <param name="currentUser"></param>
        public void SetDefaults(string currentUser)
        {
            TransactionsList.Clear();
            PagesCount = 0;
            CurrentPage = 0;

            IsListAscending = true;
        }

        /// <summary>
        /// Adding new Items to exist list of transactions
        /// </summary>
        /// <param name="transactions"></param>
        public void AddTransactionsToExisted(List<Transaction> transactions)
        {
            TransactionsList.AddRange(transactions);
        }

        /// <summary>
        /// Get All Transactions
        /// </summary>
        /// <returns></returns>
        public List<Transaction> GetTransactions()
        {
            return IsListAscending ? GetAscendingList(null) : GetDescendingList();
        }

        /// <summary>
        /// Get new portion of Transactions only
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public List<Transaction> GetTransactions(List<Transaction> transactions)
        {
            return IsListAscending ? GetAscendingList(transactions) : GetDescendingList();
        }

        #region Private Methods

        /// <summary>
        /// Get Only ASCENDING Transactions
        /// Depends on argument return transactions from whole list if argument is null
        /// or return transaction filtered only from argument list if argument is not null
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        private List<Transaction> GetAscendingList(List<Transaction> transactions)
        {
            return transactions ?? TransactionsList;
        }

        /// <summary>
        /// Get Only DESCENDING Transactions
        /// </summary>
        /// <returns></returns>
        private List<Transaction> GetDescendingList()
        {
            var transactionsList = new List<Transaction>(GetAscendingList(null));
            transactionsList.Reverse();

            return transactionsList;
        }

        #endregion
    }
}
