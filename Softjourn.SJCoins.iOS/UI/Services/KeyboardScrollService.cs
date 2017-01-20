using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS.Services
{
	public class KeyboardScrollService
	{
		#region Properties
		private NSObject _onKeyboardWillShowNotificationObserver;
		private NSObject _onKeyboardWillHideNotificationObserver;
		private UIEdgeInsets _originalInsets;
		private UITapGestureRecognizer _tapGetureRecognizer;
		private UIScrollView _scrollView;
		#endregion

		#region Constructor
		public KeyboardScrollService(UIScrollView scrollView)
		{
			_originalInsets = scrollView.ContentInset;
			_scrollView = scrollView;
			Action action = () =>
			{
				UIView editableView = _scrollView;
				while (editableView.Superview != null)
				{
					editableView = editableView.Superview;
				}
				editableView.EndEditing(true);
			};
			_tapGetureRecognizer = new UITapGestureRecognizer(action);
			_tapGetureRecognizer.Enabled = false;
			_scrollView.AddGestureRecognizer(_tapGetureRecognizer);
		}
		#endregion

		#region Public methods
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
		#endregion

		#region Private methods
		private void OnKeyboardWillShowNotification(NSNotification notification)
		{
			_tapGetureRecognizer.Enabled = true;

			var keyboardSize = UIKeyboard.FrameBeginFromNotification(notification);
			var keyboardHeight = keyboardSize.Height;

			_originalInsets.Top = _scrollView.ContentInset.Top;

			var insets = new UIEdgeInsets(_originalInsets.Top, _originalInsets.Left, keyboardHeight, _originalInsets.Right);
			_scrollView.ContentInset = insets;
			_scrollView.ScrollIndicatorInsets = insets;
		}

		private void OnKeyboardWillHideNotification(NSNotification notification)
		{
			_tapGetureRecognizer.Enabled = false;

			var defaultInsets = _originalInsets;
			_scrollView.ContentInset = defaultInsets;
			_scrollView.ScrollIndicatorInsets = defaultInsets;
		}
		#endregion
	}
}
