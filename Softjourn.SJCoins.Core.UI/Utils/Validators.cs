
using System.Text.RegularExpressions;


namespace Softjourn.SJCoins.Core.UI.Utils
{
    public class Validators
    {
        public static bool IsPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password);
        }

        public static bool IsUserNameEmpty(string userName)
        {
            return !string.IsNullOrEmpty(userName);
        }

        public static bool IsAmountValid(string amount)
        {
            return Regex.IsMatch(amount, "^[0-9]*$");
        }
    }
}
