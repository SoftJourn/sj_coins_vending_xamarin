using System;
using Foundation;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
    public class LoginPageTextFieldsDelegate: UITextFieldDelegate
    {
		public event EventHandler<UITextField> ShouldReturnEvent;

		public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
		{
			return replacementString != " ";
		}

		public override bool ShouldReturn(UITextField textField)
		{
            ShouldReturnEvent?.Invoke(this, textField);
			return true;
		}
    }
}
