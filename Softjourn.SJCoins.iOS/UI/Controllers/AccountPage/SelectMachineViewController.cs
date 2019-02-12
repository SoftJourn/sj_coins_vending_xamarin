using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Cells;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers.AccountPage
{
	[Register("SelectMachineViewController")]
	public partial class SelectMachineViewController : BaseViewController<SelectMachinePresenter>, ISelectMachineView
	{
        private SelectMachineSource TableSource;
		private bool pullToRefreshTriggered;

		public SelectMachineViewController(IntPtr handle) : base(handle) 
		{
		}

        #region Controller Life cycle 

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigurePage();
            ConfigureTableView();
            Presenter.GetMachinesList();
            NavigationController.SetNavigationBarHidden(true, false);
        }

		#endregion

		#region BaseViewController

		public override void ShowProgress(string message)
		{
			if (!pullToRefreshTriggered)
				base.ShowProgress(message);
		}

		public override void HideProgress()
		{
			if (!pullToRefreshTriggered)
				base.HideProgress();

            pullToRefreshTriggered = false;
			StopRefreshing();
		}

        public override void AttachEvents()
        {
            base.AttachEvents();
            TableSource.ItemSelected += TableSource_ItemClicked;
        }

        public override void DetachEvents()
        {
            TableSource.ItemSelected -= TableSource_ItemClicked;
            base.DetachEvents();
        }

		#endregion

		#region ISelectMachineView implementation

		public void ShowNoMachineView(string message)
		{
            NoMachinesLabel.Text = message;
            NavigationController.SetNavigationBarHidden(false, false);
            ShowScreenAnimated(false);			
		}

		public void ShowMachinesList(List<Machines> list, Machines selectedMachine = null)
		{
			TableSource.SetParameters(list, selectedMachine);
			TableView.ReloadData();
            NavigationController.SetNavigationBarHidden(false, false);
            ShowScreenAnimated(true);
		}

		#endregion

		#region Private methods

        private void ConfigurePage()
        {
            NoMachinesLabel.Alpha = 0.0f;
            TableView.Alpha = 0.0f;
            StyleNavigationBar();
        }

		private void ConfigureTableView()
		{
			TableSource = new SelectMachineSource();
			TableView.Source = TableSource;
		}

        #endregion

        #region Event handlers

		private void TableSource_ItemClicked(object sender, Machines machine)
		{
			Presenter.OnMachineSelected(machine);
		}

        #endregion 
		
		// Throw TableView to parent
		protected override UIScrollView GetRefreshableScrollView() => TableView;

		protected override void PullToRefreshTriggered(object sender, EventArgs e)
		{
            NoMachinesLabel.Alpha = 0f;
            StopRefreshing();
            pullToRefreshTriggered = true;
			Presenter.GetMachinesList();
		}

        protected override void ShowAnimated(bool loadSuccess)
        {
            NoMachinesLabel.Alpha = !loadSuccess ? 1.0f : 0f;
            TableView.Alpha = 1.0f;
            NavigationItem.Title = "Vending Machines";
        }
	}

	#region SelectMachineSource implementation

	public class SelectMachineSource : UITableViewSource
	{
		private List<Machines> machines = new List<Machines>();
		private Machines selectedMachine;

		public event EventHandler<Machines> ItemSelected;

		public void SetParameters(List<Machines> machines, Machines selectedMachine = null)
		{
			this.machines = machines;
			this.selectedMachine = selectedMachine;
		}

		public override nint RowsInSection(UITableView tableview, nint section) => machines.Count;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(SelectMachineCell.Key, indexPath);

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			var item = machines[indexPath.Row];
			cell.TextLabel.Text = item.Name;

				// display checkmarks
				if (selectedMachine != null)
				{
					if (machines[indexPath.Row].Id == selectedMachine.Id)
						cell.Accessory = UITableViewCellAccessory.Checkmark;
					else
						cell.Accessory = UITableViewCellAccessory.None;
				}
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			// change checkmark
			selectedMachine = machines[indexPath.Row];
			tableView.ReloadData();

			// sent to presenter selected machine
			tableView.DeselectRow(indexPath, true);
			ItemSelected?.Invoke(this, selectedMachine);
		}
	}

	#endregion
}
