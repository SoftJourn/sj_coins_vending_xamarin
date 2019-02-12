using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.UIElements
{
    [Register("CustomSegmentControl")]
    public class CustomSegmentControl : UISegmentedControl
    {
        private int oldValue;

        public CustomSegmentControl(IntPtr handle) : base(handle)
        {
            Configure();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            oldValue = (int)SelectedSegment;
            base.TouchesBegan(touches, evt);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (oldValue == (int)SelectedSegment)
                SendActionForControlEvents(UIControlEvent.TouchUpInside);
        }

        private void Configure()
        {
            var attributes = new UITextAttributes
            {
                Font = UIFont.SystemFontOfSize(16)
            };
            SetTitleTextAttributes(attributes, UIControlState.Normal);
            SetTitleTextAttributes(attributes, UIControlState.Selected);
        }
    }
}
