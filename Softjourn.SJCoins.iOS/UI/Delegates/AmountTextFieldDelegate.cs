using System;
using UIKit;

namespace Softjourn.SJCoins.iOS
{
	public class AmountTextFieldDelegate : UITextFieldDelegate
	{
		public AmountTextFieldDelegate() 
		{
		}

		public override bool ShouldChangeCharacters(UITextField textField, Foundation.NSRange range, string replacementString)
		{
			string text = textField.Text;
			string newText = text.Substring(0, (int)range.Location) + replacementString + text.Substring((int)(range.Location + range.Length));
			int val;
			return Int32.TryParse(newText, out val);

			//var numberOnly = NSCharacterSet("1");
			//string filter = "0123456789";
			//var filtered = new string(text.Where(c => filter.IndexOf(c) >= 0).ToArray());
		}

		public override bool ShouldReturn(UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}
	}
}
