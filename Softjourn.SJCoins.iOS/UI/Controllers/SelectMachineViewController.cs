using System;
using System.Collections.Generic;
using BigTed;
using Foundation;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Presenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.iOS.UI.Controllers;
using UIKit;

namespace Softjourn.SJCoins.iOS
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

			Presenter.GetMachinesList();
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

		public void ShowMachinesList(List<Machines> list)
		{
			// save list in controller and reload tableView
			machines = list;
			TableView.ReloadData();
		}

		public void ShowNoMachineView()
		{
			// show label that no machines
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

			public override nint RowsInSection(UITableView tableview, nint section) => 1; 

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) => tableView.DequeueReusableCell(SelectMachineCell.Key, indexPath);

			public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
			{
				if (parent.machines != null)
				{
					cell.TextLabel.Text = parent.machines[indexPath.Row].Name;
					if (parent.machines[indexPath.Row].Id == parent.selectedMachine.Id)
					{
						cell.Accessory = UITableViewCellAccessory.Checkmark;
					}
					else {
						cell.Accessory = UITableViewCellAccessory.None;
					}
				}
			}
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
