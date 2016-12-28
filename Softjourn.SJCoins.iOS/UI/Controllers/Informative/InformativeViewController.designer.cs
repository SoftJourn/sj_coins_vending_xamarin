// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Softjourn.SJCoins.iOS
{
	[Register ("InformativeViewController")]
	partial class InformativeViewController
	{
		[Outlet]
		UIKit.UIPageControl PageControl { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PageControl != null) {
				PageControl.Dispose ();
				PageControl = null;
			}
		}
	}
}
