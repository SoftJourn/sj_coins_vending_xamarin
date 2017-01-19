
using System.Text.RegularExpressions;
using Softjourn.SJCoins.Core.UI.Presenters;


namespace Softjourn.SJCoins.Core.UI.Utils
{
    public class Validators
    {
        public static bool IsPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password);
        }

        public static bool IsUserNameValid(string userName)
        {
            return !string.IsNullOrEmpty(userName) && Regex.IsMatch(userName, "^[a-z]+(?:[ _-][a-z]+)*$");
        }
    }
}
