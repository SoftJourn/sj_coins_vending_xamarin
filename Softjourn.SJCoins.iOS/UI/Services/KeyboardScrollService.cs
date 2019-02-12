using CoreGraphics;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Services
{
    public class KeyboardScrollService
    {
        private NSObject _onKeyboardWillShowNotificationObserver;
        private NSObject _onKeyboardWillHideNotificationObserver;
        private UIEdgeInsets _originalInsets;
        private readonly UITapGestureRecognizer _tapGetureRecognizer;
        private readonly UIScrollView scrollView;
        private readonly CGPoint _buttonLocation;
        private readonly CGRect _frame;

        public KeyboardScrollService(UIScrollView scrollView, CGPoint buttonLocation, CGRect frame)
        {
            _originalInsets = scrollView.ContentInset;
            this.scrollView = scrollView;
            _buttonLocation = buttonLocation;
            _frame = frame;

            void Action()
            {
                UIView editableView = scrollView;
                while (editableView.Superview != null)
                {
                    editableView = editableView.Superview;
                }

                editableView.EndEditing(true);
            }

            _tapGetureRecognizer = new UITapGestureRecognizer(Action)
            {
                Enabled = false
            };
            scrollView.AddGestureRecognizer(_tapGetureRecognizer);
        }

        public void AttachToKeyboardNotifications()
        {
            _onKeyboardWillShowNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardWillShowNotification);
            _onKeyboardWillHideNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardWillHideNotification);
        }

        public void DetachToKeyboardNotifications()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(_onKeyboardWillShowNotificationObserver);
            NSNotificationCenter.DefaultCenter.RemoveObserver(_onKeyboardWillHideNotificationObserver);
        }

        #region Private methods

        private void OnKeyboardWillShowNotification(NSNotification notification)
        {
            _tapGetureRecognizer.Enabled = true;

            var keyboardSize = UIKeyboard.FrameEndFromNotification(notification);
            var keyboardHeight = keyboardSize.Height;

            _originalInsets.Top = scrollView.ContentInset.Top;

            var insets = new UIEdgeInsets(_originalInsets.Top, _originalInsets.Left, keyboardHeight, _originalInsets.Right);
            scrollView.ContentInset = insets;
            scrollView.ScrollIndicatorInsets = insets;

            var visibleRect = _frame;
            visibleRect.Height -= keyboardSize.Height;

            if (!visibleRect.Contains(_buttonLocation))
            {
                var scrollPoint = new CGPoint(0, _buttonLocation.Y - visibleRect.Height + 60);
                scrollView.SetContentOffset(scrollPoint, true);
            }
        }

        private void OnKeyboardWillHideNotification(NSNotification notification)
        {
            _tapGetureRecognizer.Enabled = false;

            var defaultInsets = _originalInsets;
            scrollView.ContentInset = defaultInsets;
            scrollView.ScrollIndicatorInsets = defaultInsets;

            scrollView.SetContentOffset(CGPoint.Empty, true);
        }

        #endregion
    }
}
