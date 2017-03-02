
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

        private readonly DataManager _dataManager;

        private List<Transaction> TransactionsList { get; set; }
        private string _currentUser;

        public TransactionsManager()
        {
            TransactionsList = new List<Transaction>();
            PagesCount = 0;
            CurrentPage = 0;
            IsListAscending = true;
            IsInput = true;
            _dataManager = new DataManager();
        }

        #region Public Methods
        //Setting all properties to defaults
        public void SetDefaults(string currentUser)
        {
            TransactionsList = new List<Transaction>();
            PagesCount = 0;
            CurrentPage = 0;

            IsListAscending = true;
            _currentUser = currentUser;
        }

        //Adding new Items to existe list of transactions
        public void AddTransactionsToExisted(List<Transaction> transactions)
        {
            TransactionsList.AddRange(transactions);
        }

        //Get All Transactions
        public List<Transaction> GetTransactions()
        {
            return IsInput ? GetOnlyInput(null) : GetOnlyOutput(null);
        }

        //Get new portion of Transactions only
        public List<Transaction> GetTransactions(List<Transaction> transactions)
        {
            return IsInput ? GetOnlyInput(transactions) : GetOnlyOutput(transactions);
        }
        #endregion

        #region Private Methods
        //Return Input Transactions depends on IsListDescending
        private List<Transaction> GetOnlyInput(List<Transaction> transactions)
        {
            return IsListAscending ? GetAscendingInput(transactions) : GetDescendingInput(transactions);
        }

        //Return Output Transactions depends on IsListDescending
        private List<Transaction> GetOnlyOutput(List<Transaction> transactions)
        {
            return IsListAscending ? GetAscendingOutput(transactions) : GetDescendingOutput(transactions);
        }

        //Get Only Input ASCENDING Transactions (where Destination is current user)
        //Depands on argument return transactions from whole list if argument is null
        //or return transaction filtered only from argument list if argument is not null
        private List<Transaction> GetAscendingInput(List<Transaction> transactions)
        {
            return transactions == null ? TransactionsList.Where(transaction => transaction.Destination == _currentUser).ToList() :
                                          transactions.Where(transaction => transaction.Destination == _currentUser).ToList();
        }

        //Get Only Output ASCENDING Transactions (where Account is current user)
        //Depands on argument return transactions from whole list if argument is null
        //or return transaction filtered only from argument list if argument is not null
        private List<Transaction> GetAscendingOutput(List<Transaction> transactions)
        {
            return transactions == null ? TransactionsList.Where(transaction => transaction.Account ==_currentUser).ToList() :
                                          transactions.Where(transaction => transaction.Account == _currentUser).ToList();
        }

        //Get Only Input DESCENDING Transactions (where Destination is current user)
        //Depands on argument return transactions from whole list if argument is null
        //or return transaction filtered only from argument list if argument is not null
        private List<Transaction> GetDescendingInput(List<Transaction> transactions)
        {
            var transactionsList = GetAscendingInput(null);
            transactionsList.Reverse();
            return transactionsList;

        }

        //Get Only Output DESCENDING Transactions (where Account is current user)
        //Depands on argument return transactions from whole list if argument is null
        //or return transaction filtered only from argument list if argument is not null
        private List<Transaction> GetDescendingOutput(List<Transaction> transactions)
        {
            var transactionsList = GetAscendingOutput(null);
            transactionsList.Reverse();
            return transactionsList;
        }
        #endregion
    }
}
