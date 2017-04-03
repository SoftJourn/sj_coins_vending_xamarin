using System;
using System.Collections.Generic;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using CoreGraphics;

using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("SelectMachineViewController")]
	public partial class SelectMachineViewController : BaseViewController<SelectMachinePresenter>, ISelectMachineView
	{
		#region Properties
		private SelectMachineSource _tableSource;
		#endregion

		#region Constructor
		public SelectMachineViewController(IntPtr handle) : base(handle) 
		{
		}
		#endregion

		#region Controller Life cycle 
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ConfigureTableView();
			Presenter.GetMachinesList();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			// Attach 
			_tableSource.ItemSelected += TableSource_ItemClicked;
			NoMachinesLabel.Hidden = true;
		}

		public override void ViewWillDisappear(bool animated)
		{
			// Detach 
			_tableSource.ItemSelected -= TableSource_ItemClicked;
			base.ViewWillDisappear(animated);
		}
		#endregion

		#region BaseViewController
		public override void ShowProgress(string message)
		{
		}

		public override void HideProgress()
		{
			StopRefreshing();
		}
		#endregion

		#region ISelectMachineView implementation
		public void ShowNoMachineView(string message)
		{
			// show label that no machines fetched
			NoMachinesLabel.Text = message;
			NoMachinesLabel.Hidden = false;
		}

		public void ShowMachinesList(List<Machines> list, Machines selectedMachine = null)
		{
			// save list in controller and reload tableView
			_tableSource.SetParameters(list, selectedMachine, ConfigureVendingMachinesHeader());
			TableView.ReloadData();
		}
		#endregion

		#region Private methods
		private void ConfigureTableView()
		{
			_tableSource = new SelectMachineSource();
			TableView.Source = _tableSource;
		}

		private UIView ConfigureVendingMachinesHeader()
		{
			UIView view = new UIView();
			UILabel label = new UILabel(frame: new CGRect(x: 25, y: 15, width: 300, height: 20));
			label.TextAlignment = UITextAlignment.Left;
			label.Text = "Vending Machines";
			label.TextColor = UIColor.Gray;
			view.Add(label);
			return view;
		}

		// -------------------- Event handlers --------------------
		private void TableSource_ItemClicked(object sender, Machines machine)
		{
			Presenter.OnMachineSelected(machine);
		}
		// -------------------------------------------------------- 
		#endregion

		// Throw TableView to parent
		protected override UIScrollView GetRefreshableScrollView() => TableView;

		protected override void PullToRefreshTriggered(object sender, EventArgs e)
		{
			Presenter.GetMachinesList();
		}
	}

	#region SelectMachineSource implementation
	public class SelectMachineSource : UITableViewSource
	{
		private List<Machines> machines = new List<Machines>();
		private Machines selectedMachine;
		private UIView headerView;

		public event EventHandler<Machines> ItemSelected;

		public void SetParameters(List<Machines> machines, Machines selectedMachine = null, UIView headerView = null)
		{
			this.machines = machines;
			this.selectedMachine = selectedMachine;
			this.headerView = headerView;
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

		public override nfloat GetHeightForHeader(UITableView tableView, nint section) => section == 0 ? 40 : 0;

		public override UIView GetViewForHeader(UITableView tableView, nint section) => headerView;

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
