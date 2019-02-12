using UIKit;

namespace Softjourn.SJCoins.iOS.UI.Delegates
{
    public class AmountTextFieldDelegate : UITextFieldDelegate
    {
        public override bool ShouldChangeCharacters(UITextField textField, Foundation.NSRange range, string replacementString)
        {
            if (replacementString != string.Empty)
            {
                var text = textField.Text;
                var newText = text.Substring(0, (int)range.Location) + replacementString + text.Substring((int)(range.Location + range.Length));

                return int.TryParse(newText, out _);
            }

            return true;
        }

        public override bool ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();

            return true;
        }
    }
}
