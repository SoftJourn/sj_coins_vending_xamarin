using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class TransactionReportPresenter : BasePresenter<ITransactionReportView>
    {

        private const int DefaultPageNumber = 0;
        private const string DefaultDirection = "desc";
        private const string DefaultProperty = "created";
        private TransactionsManager TransactionsManager;
        private bool _isLoading = false;

        private string _sortProperty;
        private string _sortDirection;

        public TransactionReportPresenter()
        {
            _scope = BaseBootstrapper.Container.BeginLifetimeScope();
            TransactionsManager = _scope.Resolve<TransactionsManager>();
            _sortProperty = DefaultProperty;
            _sortDirection = DefaultDirection;
            TransactionsManager.IsInput = true;
        }

        #region Public Methods

        //Get first page of Transaction
        //Is called when View started
        public void OnStartLoadingPage()
        {
            TransactionsManager.SetDefaults(DataManager.Profile.Name+" "+DataManager.Profile.Surname);
            GetReportTransactions(DefaultPageNumber, DefaultDirection, DefaultProperty);
        }

        //Get next page of transactions if transactions are not loading in current moment
        public void GetNextPage()
        {
            if (_isLoading) return;
            if (TransactionsManager.PagesCount <= 1 || TransactionsManager.CurrentPage >= TransactionsManager.PagesCount)
                return;
            _isLoading = true;
            //Get Transaction for next page 
            //where next page is Curent page + 1
            GetReportTransactions(TransactionsManager.CurrentPage + 1, _sortDirection, _sortProperty);
        }

        //Handle Click on Input button
        public void OnInputClicked()
        {
            if (TransactionsManager.IsInput && TransactionsManager.IsListAscending)
            {
                TransactionsManager.IsListAscending = false;
                View.SetData(TransactionsManager.GetTransactions());
                return;
            }
            if (TransactionsManager.IsInput && !TransactionsManager.IsListAscending)
            {
                TransactionsManager.IsListAscending = true;
                View.SetData(TransactionsManager.GetTransactions());
                return;
            }
            if (!TransactionsManager.IsInput)
            {
                TransactionsManager.IsInput = true;
                TransactionsManager.IsListAscending = true;
                View.SetData(TransactionsManager.GetTransactions());
                return;
            }
        }

        //Handle Click on Output button
        public void OnOutputClicked()
        {
            if (!TransactionsManager.IsInput && TransactionsManager.IsListAscending)
            {
                TransactionsManager.IsListAscending = false;
                View.SetData(TransactionsManager.GetTransactions());
                return;
            }
            if (!TransactionsManager.IsInput && !TransactionsManager.IsListAscending)
            {
                TransactionsManager.IsListAscending = true;
                View.SetData(TransactionsManager.GetTransactions());
                return;
            }
            if (TransactionsManager.IsInput)
            {
                TransactionsManager.IsInput = false;
                TransactionsManager.IsListAscending = true;
                View.SetData(TransactionsManager.GetTransactions());
                return;
            }
        }

        public void OnOrderByAmountClick()
        {
            if (_sortProperty != DefaultProperty) return;

            _sortProperty = "amount";
            _sortDirection = "asc";

            TransactionsManager.SetDefaults(DataManager.Profile.Name + " " + DataManager.Profile.Surname);
            GetReportTransactions(DefaultPageNumber, _sortDirection, _sortProperty);
        }

        public void OnOrderByDateClick()
        {
            if (_sortProperty == DefaultProperty) return;

            _sortProperty = DefaultProperty;
            _sortDirection = DefaultDirection;

            OnStartLoadingPage();
        }
        #endregion

        #region Private Methods

        //Form TransactionRequest for REST call
        //by setting page to be loaded and sort method (Property, Direction(Asc,Desc))
        private TransactionRequest FormRequestBody(int pageNumber, string direction, string property)
        {
            var transactionRequest = new TransactionRequest
            {
                Size = 50,
                Page = pageNumber
            };
            var sort = new Sort
            {
                Direction = direction,
                Property = property
            };
            var sortList = new List<Sort>();
            sortList.Add(sort);
            transactionRequest.Sort = sortList;

            return transactionRequest;
        }

        //REST call for transactions
        private async void GetReportTransactions(int pageNumber, string direction, string property)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.StringResources.progress_loading);

                    var transactionReport = await RestApiServise.GetTransactionReport(FormRequestBody(pageNumber, direction, property));

                    //Converting DateTime in appropriate string for UI
                    foreach (var item in transactionReport.Content)
                    {
                        item.PrettyTime = TimeUtils.GetPrettyTime(item.Created);
                    }
                    View.HideProgress();

                    //If list is Empty Show empty View
                    //else show data
                    if (transactionReport.Content.Count == 0)
                    {
                        View.ShowEmptyView();
                    }
                    else
                    {
                        //Setting info about pages and Adding new transactions to existed list
                        //in TransactionsManager
                        TransactionsManager.CurrentPage = transactionReport.Number;
                        TransactionsManager.PagesCount = transactionReport.TotalPages;
                        TransactionsManager.AddTransactionsToExisted(transactionReport.Content);

                        //If first page is loaded call view method
                        //to set data.
                        if (transactionReport.Number == DefaultPageNumber)
                        {
                            View.SetData(TransactionsManager.GetTransactions());
                        }
                        //If not first page is loaded call view method
                        //to add new data to existed one.
                        //Setting Loading toggle to false to allow loading of the next page
                        else
                        {
                            if (TransactionsManager.IsListAscending)
                            {
                                _isLoading = false;
                                View.AddItemsToExistedList(TransactionsManager.GetTransactions(transactionReport.Content));
                            }
                            else
                            {
                                _isLoading = false;
                                View.SetData(TransactionsManager.GetTransactions(transactionReport.Content));
                            }
                        }
                    }
                }
                catch (ApiNotAuthorizedException ex)
                {
                    View.HideProgress();
                    AlertService.ShowToastMessage(ex.Message);
                    NavigationService.NavigateToAsRoot(NavigationPage.Login);
                }
                catch (Exception ex)
                {
                    View.HideProgress();
                    AlertService.ShowToastMessage(ex.Message);
                }
            }
            else
            {
                AlertService.ShowToastMessage(Resources.StringResources.internet_turned_off);
            }
        }
        #endregion
    }
}
