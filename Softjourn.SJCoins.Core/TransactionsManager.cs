using System.Collections.Generic;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Models.TransactionReports;

namespace Softjourn.SJCoins.Core.Managers
{
    public sealed class TransactionsManager
    {
        public int PagesCount { get; set; }
        public int CurrentPage { get; set; }
        public bool IsInput { get; set; }
        public bool IsListAscending { get; set; }

        private List<Transaction> TransactionsList { get; }

        public TransactionsManager()
        {
            TransactionsList = new List<Transaction>();
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
            PagesCount = Constant.Zero;
            CurrentPage = Constant.Zero;

            IsListAscending = true;
        }

        /// <summary>
        /// Adding new Items to exist list of transactions
        /// </summary>
        /// <param name="transactions"></param>
        public void AddTransactionsToExisted(List<Transaction> transactions) => TransactionsList.AddRange(transactions);

        /// <summary>
        /// Get All Transactions
        /// </summary>
        /// <returns></returns>
        public List<Transaction> GetTransactions()
        {
            List<Transaction> GetDescendingList()
            {
                var transactionsList = new List<Transaction>(TransactionsList);
                transactionsList.Reverse();

                return transactionsList;
            }

            return IsListAscending
                ? TransactionsList
                : GetDescendingList();
        }
    }
}
