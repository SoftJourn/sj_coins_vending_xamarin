using System;
using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Delegates
{
	public class AmountTextFieldDelegate : UITextFieldDelegate
	{
		public override bool ShouldChangeCharacters(UITextField textField, Foundation.NSRange range, string replacementString)
		{
			if (replacementString != "")
			{
				string text = textField.Text;
				string newText = text.Substring(0, (int)range.Location) + replacementString + text.Substring((int)(range.Location + range.Length));
				int val;
				return Int32.TryParse(newText, out val);
			}
			else
				return true;
		}

		public override bool ShouldReturn(UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}
	}
}
