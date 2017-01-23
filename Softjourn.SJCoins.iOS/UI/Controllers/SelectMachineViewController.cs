using System;
using System.Collections.Generic;
using BigTed;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Controllers;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Controllers
{
	[Register("SelectMachineViewController")]
	public partial class SelectMachineViewController : BaseViewController<SelectMachinePresenter>, ISelectMachineView
	{
		#region Properties
		private List<Machines> machines;
		private Machines selectedMachine;
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

			TableView.Source = new SelectMachineViewControllerDataSource(this);
			TableView.Delegate = new SelectMachineViewControllerDelegate(this);
			Presenter.GetMachinesList();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			NoMachinesLabel.Hidden = true;
		}
		#endregion

		#region ISelectMachineView implementation
		public void ShowProgress(string message)
		{
			BTProgressHUD.Show(message);
		}

		public void HideProgress()
		{
			BTProgressHUD.Dismiss();
		}

		public void ShowNoMachineView(string message)
		{
			// show label that no machines fetched
			NoMachinesLabel.Text = message;
			NoMachinesLabel.Hidden = false;
		}

		public void ShowMachinesList(List<Machines> list, Machines selectedMachine = null)
		{
			// save list in controller and reload tableView
			machines = list;
			this.selectedMachine = selectedMachine;
			TableView.ReloadData();
		}
		#endregion

		#region BaseViewController -> IBaseView implementation
		public override void SetUIAppearance()
		{
		}

		public override void AttachEvents()
		{
		}

		public override void DetachEvents()
		{
		}
		#endregion

		#region SelectMachineViewControllerDataSource implementation
		private class SelectMachineViewControllerDataSource : UITableViewSource
		{
			private SelectMachineViewController parent;

			public SelectMachineViewControllerDataSource(SelectMachineViewController parent)
			{
				this.parent = parent;
			}

			public override nint NumberOfSections(UITableView tableView) => 1;

			public override nint RowsInSection(UITableView tableview, nint section) => parent.machines == null ? 0 : parent.machines.Count;

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell(SelectMachineCell.Key, indexPath);
				if (parent.machines != null)
				{
					cell.TextLabel.Text = parent.machines[indexPath.Row].Name;
					if (parent.selectedMachine != null)
					{
						if (parent.machines[indexPath.Row].Id == parent.selectedMachine.Id)
						{
							cell.Accessory = UITableViewCellAccessory.Checkmark;
						}
						else {
							cell.Accessory = UITableViewCellAccessory.None;
						}
					}
				}
				return cell;
			}

			//public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
			//{
			//	Console.WriteLine("WillDisplay called");
			//	if (parent.machines != null)
			//	{
			//		cell.TextLabel.Text = parent.machines[indexPath.Row].Name;
			//		if (parent.machines[indexPath.Row].Id == parent.selectedMachine.Id)
			//		{
			//			cell.Accessory = UITableViewCellAccessory.Checkmark;
			//		}
			//		else {
			//			cell.Accessory = UITableViewCellAccessory.None;
			//		}
			//	}
			//}
		}
		#endregion

		#region SelectMachineViewControllerDelegate implementation
		private class SelectMachineViewControllerDelegate : UITableViewDelegate
		{
			private SelectMachineViewController parent;

			public SelectMachineViewControllerDelegate(SelectMachineViewController parent)
			{
				this.parent = parent;
			}

			public override nfloat GetHeightForHeader(UITableView tableView, nint section) => section == 0 ? 40 : 0;

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				if (parent.machines != null)
				{
					parent.selectedMachine = parent.machines[indexPath.Row];
					tableView.ReloadData();
				}
			}
		}
		#endregion
	}
}
