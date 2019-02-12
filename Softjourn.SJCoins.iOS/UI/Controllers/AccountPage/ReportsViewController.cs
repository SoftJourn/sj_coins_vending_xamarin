using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.TransactionReports;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using UIKit;
using Softjourn.SJCoins.iOS.General.Helper;
using Softjourn.SJCoins.iOS.General.Constants;
using Softjourn.SJCoins.iOS.UI.Sources.AccountPage;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
    [Register("ReportsViewController")]
    public partial class ReportsViewController : BaseViewController<TransactionReportPresenter>, ITransactionReportView
    {
        private const string InputTitle = "Input";
        private const string OutputTitle = "Output";
        private const int InputSegment = 0;
        private const int OutputSegment = 1;

        enum SegmentControls { DateAmount, InputOutput };

        private bool firstStart = true;
        private ReportsViewSource tableSource;
        private SegmentControlHelper segmentControlHelper;

        public ReportsViewController(IntPtr handle) : base(handle)
        {
        }

        #region Controller Life cycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigurePage();
            ConfigureTableView();
            ConfigureSegmentControl();
            Presenter.OnStartLoadingPage();
        }

        #endregion

        #region IReportsView implementation

        public void SetData(List<Transaction> transactionsList)
        {
            tableSource.SetItems(transactionsList);
            if (firstStart)
            {
                TableView.ReloadData();
                firstStart = false;
            }
            else
                ReloadTable();
            ShowScreenAnimated(true);
        }

        public void ShowEmptyView()
        {
            ShowScreenAnimated(false);
        }

        public void AddItemsToExistedList(List<Transaction> transactionsList)
        {
            tableSource.AddItems(transactionsList, TableView);
        }

        public void SetCompoundDrawableInput(bool? isAsc)
        {
            SetCompoundDrawableSegment(isAsc, InputTitle, InputSegment);
        }

        public void SetCompoundDrawableOutput(bool? isAsc)
        {
            SetCompoundDrawableSegment(isAsc, OutputTitle, OutputSegment);
        }

        #endregion

        #region BaseViewController

        public override void AttachEvents()
        {
            base.AttachEvents();
            InputOutputSegmentControl.TouchUpInside += InputOutputSegmentControl_SameButtonClicked;
            InputOutputSegmentControl.ValueChanged += InputOutputSegmentControl_AnotherButtonClicked;
            DateAmountSegmentControl.ValueChanged += DateAmountSegmentControl_AnotherButtonClicked;
            tableSource.GetNexPage += TableSource_GetNextPageExecuted;
        }

        public override void DetachEvents()
        {
            InputOutputSegmentControl.TouchUpInside -= InputOutputSegmentControl_SameButtonClicked;
            InputOutputSegmentControl.ValueChanged -= InputOutputSegmentControl_AnotherButtonClicked;
            DateAmountSegmentControl.ValueChanged -= DateAmountSegmentControl_AnotherButtonClicked;
            tableSource.GetNexPage -= TableSource_GetNextPageExecuted;
            base.DetachEvents();
        }

        public override void ShowProgress(string message)
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
        }

        public override void HideProgress()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }

        #endregion

        #region Private methods

        private void ConfigurePage()
        {
            NoItemsLabel.Hidden = true;
            NoItemsLabel.Alpha = 0.0f;
            TableView.Alpha = 0.0f;
        }

        private void ConfigureTableView()
        {
            tableSource = new ReportsViewSource();
            TableView.Source = tableSource;
        }

        private void ConfigureSegmentControl()
        {
            segmentControlHelper = new SegmentControlHelper();
            // Configure 0 segment
            ConfigureSegment(InputTitle, InputSegment, ImageConstants.ArrowUpward);
        }

        private void ReloadTable()
        {
            TableView.ReloadSections(new NSIndexSet(0), UITableViewRowAnimation.Fade);
        }

        private void SetCompoundDrawableSegment(bool? isAsc, string title, int segment)
        {
            if (isAsc == true)
                ConfigureSegment(title, segment, ImageConstants.ArrowDownward);
            else if (isAsc == false)
                ConfigureSegment(title, segment, ImageConstants.ArrowUpward);
            else
                ConfigureSegment(title, segment, null);
        }

        private void ConfigureSegment(string title, int segment, string imageName = null)
        {
            // Configure segment depending on whether the picture is present or not 
            if (imageName == null)
                InputOutputSegmentControl.SetTitle(title, segment);
            else
            {
                var inputImage = UIImage.FromBundle(imageName);
                var mergedImage = segmentControlHelper.ImageFromImageAndText(inputImage, title, UIColor.Black);
                InputOutputSegmentControl.SetImage(mergedImage, segment);
            }
        }

        private void SortBy(SegmentControls control)
        {
            switch (control)
            {
                case SegmentControls.DateAmount:
                    SortByDateAmount();
                    break;
                case SegmentControls.InputOutput:
                    SortByInputOutput();
                    break;
            }
        }

        private void SortByDateAmount()
        {
            switch (DateAmountSegmentControl.SelectedSegment)
            {
                case 0:
                    Presenter.OnOrderByDateClick();
                    break;
                case 1:
                    Presenter.OnOrderByAmountClick();
                    break;
            }
        }

        private void SortByInputOutput()
        {
            switch (InputOutputSegmentControl.SelectedSegment)
            {
                case 0:
                    Presenter.OnInputClicked();
                    break;
                case 1:
                    Presenter.OnOutputClicked();
                    break;
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// DateAmountSegmentControl methods 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DateAmountSegmentControl_AnotherButtonClicked(object sender, EventArgs e)
        {
            SortBy(SegmentControls.DateAmount);
        }

        /// <summary>
        /// InputOutputSegmentControl methods 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void InputOutputSegmentControl_SameButtonClicked(object sender, EventArgs e)
        {
            SortBy(SegmentControls.InputOutput);
        }

        public void InputOutputSegmentControl_AnotherButtonClicked(object sender, EventArgs e)
        {
            SortBy(SegmentControls.InputOutput);
        }

        /// <summary>
        /// TableSource methods 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TableSource_GetNextPageExecuted(object sender, EventArgs e)
        {
            Presenter.GetNextPage();
        }

        #endregion

        /// <summary>
        /// Throw TableView to parent
        /// </summary>
        /// <returns></returns>
        protected override UIScrollView GetRefreshableScrollView() => TableView;

        protected override void PullToRefreshTriggered(object sender, EventArgs e)
        {
            StopRefreshing();
            Presenter.OnStartLoadingPage();
        }

        protected override void ShowAnimated(bool loadSuccess)
        {
            TableView.TableFooterView.Hidden = !loadSuccess;
            NoItemsLabel.Alpha = !loadSuccess ? 1.0f : 0f;
            NoItemsLabel.Hidden = loadSuccess;
            TableView.Alpha = 1.0f;
        }
    }
}
