using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	[Register("CustomSegmentControl")]
	public class CustomSegmentControl : UISegmentedControl
	{
		private int oldValue;

		#region Constructor
		public CustomSegmentControl(IntPtr handle) : base(handle)
		{
		}
		#endregion

		public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
		{
			oldValue = (int)SelectedSegment;
			base.TouchesBegan(touches, evt);
		}

		public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);
			if (oldValue == (int)SelectedSegment)
				SendActionForControlEvents(UIControlEvent.TouchUpInside);
		}
	}
}
