using System;
using System.Collections.Generic;
using Autofac;
using Softjourn.SJCoins.Core.Common;
using Softjourn.SJCoins.Core.Common.Exceptions;
using Softjourn.SJCoins.Core.Common.Utils;
using Softjourn.SJCoins.Core.Managers;
using Softjourn.SJCoins.Core.Models.TransactionReports;
using Softjourn.SJCoins.Core.Resources;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class TransactionReportPresenter : BasePresenter<ITransactionReportView>
    {
        private const string DefaultSortDirection = "desc";
        private const string DefaultProperty = "created";
        private const string DefaultDirection = "IN";
        private readonly TransactionsManager TransactionsManager;
        private bool _isLoading;

        private string _sortProperty;
        private string _sortDirection;
        private string _direction;

        public TransactionReportPresenter()
        {
            Scope = BaseBootstrapper.Container.BeginLifetimeScope();
            TransactionsManager = Scope.Resolve<TransactionsManager>();
            _sortProperty = DefaultProperty;
            _sortDirection = DefaultSortDirection;
            _direction = DefaultDirection;
            TransactionsManager.IsInput = true;
        }

        /// <summary>
        /// Get first page of Transaction
        /// Is called when View started
        /// </summary>
        public void OnStartLoadingPage()
        {
            TransactionsManager.SetDefaults($"{DataManager.Profile.Name} {DataManager.Profile.Surname}");
            GetReportTransactions(Constant.Zero, _direction, _sortDirection, _sortProperty);
        }

        /// <summary>
        /// Get next page of transactions if transactions are not loading in current moment
        /// </summary>
        public void GetNextPage()
        {
            if (_isLoading) return;

            if (TransactionsManager.PagesCount <= 1 || TransactionsManager.CurrentPage >= TransactionsManager.PagesCount - 1)
                return;

            _isLoading = true;

            //Get Transaction for next page 
            //where next page is Current page + 1
            GetReportTransactions(TransactionsManager.CurrentPage + 1, _direction, _sortDirection, _sortProperty);
        }

        /// <summary>
        /// Handle Click on Input button
        /// </summary>
        public void OnInputClicked()
        {
            if (_direction == DefaultDirection && TransactionsManager.IsListAscending)
            {
                TransactionsManager.IsListAscending = false;
                View.SetData(TransactionsManager.GetTransactions());

                //Handling buttons arrows
                View.SetCompoundDrawableOutput(null);
                View.SetCompoundDrawableInput(_sortProperty == DefaultProperty);

                return;
            }
            if (_direction == DefaultDirection && !TransactionsManager.IsListAscending)
            {
                TransactionsManager.IsListAscending = true;
                View.SetData(TransactionsManager.GetTransactions());

                View.SetCompoundDrawableOutput(null);
                View.SetCompoundDrawableInput(_sortProperty != DefaultProperty);

                return;
            }

            if (_direction == DefaultDirection) return;

            _direction = DefaultDirection;
            OnStartLoadingPage();
            View.SetCompoundDrawableOutput(null);
            View.SetCompoundDrawableInput(_sortProperty != DefaultProperty);
        }

        /// <summary>
        /// Handle Click on Output button
        /// </summary>
        public void OnOutputClicked()
        {
            if (_direction != DefaultDirection && TransactionsManager.IsListAscending)
            {
                TransactionsManager.IsListAscending = false;
                View.SetData(TransactionsManager.GetTransactions());

                //Handling buttons arrows
                View.SetCompoundDrawableOutput(_sortProperty == DefaultProperty);
                View.SetCompoundDrawableInput(null);

                return;
            }

            if (_direction != DefaultDirection && !TransactionsManager.IsListAscending)
            {
                TransactionsManager.IsListAscending = true;
                View.SetData(TransactionsManager.GetTransactions());

                //Handling buttons arrows
                View.SetCompoundDrawableOutput(_sortProperty != DefaultProperty);
                View.SetCompoundDrawableInput(null);

                return;
            }

            if (_direction != DefaultDirection) return;

            _direction = "OUT";
            OnStartLoadingPage();
            //Handling buttons arrows
            View.SetCompoundDrawableOutput(_sortProperty != DefaultProperty);
            View.SetCompoundDrawableInput(null);
        }

        public void OnOrderByAmountClick()
        {
            if (_sortProperty != DefaultProperty) return;

            _sortProperty = "amount";
            _sortDirection = "asc";

            TransactionsManager.SetDefaults($"{DataManager.Profile.Name} {DataManager.Profile.Surname}");
            GetReportTransactions(Constant.Zero, _direction, _sortDirection, _sortProperty);

            if (_direction == DefaultDirection)
            {
                View.SetCompoundDrawableOutput(null);
                View.SetCompoundDrawableInput(true);
            }
            else
            {
                View.SetCompoundDrawableOutput(true);
                View.SetCompoundDrawableInput(null);
            }
        }

        public void OnOrderByDateClick()
        {
            if (_sortProperty == DefaultProperty) return;

            _sortProperty = DefaultProperty;
            _sortDirection = DefaultSortDirection;

            OnStartLoadingPage();

            if (_direction == DefaultDirection)
            {
                View.SetCompoundDrawableOutput(null);
                View.SetCompoundDrawableInput(false);
            }
            else
            {
                View.SetCompoundDrawableOutput(false);
                View.SetCompoundDrawableInput(null);
            }
        }

        #region Private Methods

        /// <summary>
        /// Form TransactionRequest for REST call
        /// by setting page to be loaded and sort method (Property, Direction(Asc,Desc))
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="direction"></param>
        /// <param name="sortDirection"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static TransactionRequest FormRequestBody(int pageNumber, string direction, string sortDirection, string property)
        {
            var transactionRequest = new TransactionRequest
            {
                Size = 50,
                Page = pageNumber,
                Direction = direction

            };
            var sort = new Sort
            {
                Direction = sortDirection,
                Property = property
            };
            var sortList = new List<Sort> { sort };
            transactionRequest.Sort = sortList;

            return transactionRequest;
        }

        //REST call for transactions
        private async void GetReportTransactions(int pageNumber, string direction, string sortDirection, string property)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(UiMessageResources.progress_loading);

                    var transactionReport = await RestApiService.GetTransactionReport(FormRequestBody(pageNumber, direction, sortDirection, property));

                    //Converting DateTime in appropriate string for UI
                    foreach (var item in transactionReport.Content)
                    {
                        item.PrettyTime = TimeUtils.GetPrettyTime(item.Created);
                    }

                    View.HideProgress();

                    //If list is Empty Show empty View
                    //else show data
                    if (transactionReport.Content.Count == Constant.Zero)
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
                        if (transactionReport.Number == Constant.Zero)
                        {
                            View.SetData(transactionReport.Content);
                        }
                        //If not first page is loaded call view method
                        //to add new data to existed one.
                        //Setting Loading toggle to false to allow loading of the next page
                        else
                        {
                            if (TransactionsManager.IsListAscending)
                            {
                                _isLoading = false;
                                View.AddItemsToExistedList(transactionReport.Content);
                            }
                            else
                            {
                                _isLoading = false;
                                View.SetData(TransactionsManager.GetTransactions());
                            }
                        }
                    }
                }
                catch (ApiNotAuthorizedException)
                {
                    View.HideProgress();
                    DataManager.Profile = null;
                    Settings.ClearUserData();
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
                AlertService.ShowToastMessage(UiMessageResources.internet_turned_off);
            }
        }

        #endregion
    }
}
