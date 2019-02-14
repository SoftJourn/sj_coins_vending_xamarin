using System.Text.RegularExpressions;

namespace Softjourn.SJCoins.Core.UI.Utils
{
    public class Validators
    {
        public static bool IsPasswordValid(string password) => !string.IsNullOrEmpty(password);

        public static bool IsUserNameEmpty(string userName) => !string.IsNullOrEmpty(userName);

        public static bool IsAmountValid(string amount) => Regex.IsMatch(amount, "^[0-9]*$");
    }
}
