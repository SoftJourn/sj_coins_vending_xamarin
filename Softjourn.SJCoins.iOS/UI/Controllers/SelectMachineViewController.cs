using System;
using Foundation;

namespace Softjourn.SJCoins.iOS
{
	[Register("SelectMachineViewController")]
	public class SelectMachineViewController : BaseViewController<SelectMachinePresenter>, ISelectMachineView
	{
		#region Constructor
		public SelectMachineViewController(IntPtr handle) : base(handle)
		{
		}
		#endregion

		#region Controller Life cycle 
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		
			//Presenter. call method get list
		}
		#endregion

		#region ISelectMachineView implementation
		#endregion



	}
}
