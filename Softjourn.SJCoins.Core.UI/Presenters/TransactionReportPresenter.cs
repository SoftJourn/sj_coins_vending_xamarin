using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.UI.Services.Navigation;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.UI.Presenters
{
    public class TransactionReportPresenter : BasePresenter<ITransactionReportView>
    {

        private const int DefaultPageNumber = 0;
        private const string DefaultDirection = "ASC";
        private const string DefaultProperty = "created";

        public void OnStartLoadingPage()
        {
            GetReportTransactions(DefaultPageNumber, DefaultDirection, DefaultProperty);
        }

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

        private async void GetReportTransactions(int pageNumber, string direction, string property)
        {
            if (NetworkUtils.IsConnected)
            {
                try
                {
                    View.ShowProgress(Resources.StringResources.progress_loading);

                    var transactionReport = await RestApiServise.GetTransactoionReport(FormRequestBody(pageNumber, direction, property));

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
                        View.SetData(transactionReport.Content);
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
    }
}
