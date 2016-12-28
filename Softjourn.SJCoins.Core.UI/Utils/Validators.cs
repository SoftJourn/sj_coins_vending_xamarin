
using System.Text.RegularExpressions;

namespace Softjourn.SJCoins.Core.UI.Utils
{
    public class Validators
    {
        public static int ValidateCredentials(string userName, string password)
        {
            const int fieldsAreAmpty = 1;
            const int userNameNotValid = 2;
            const int passwordNotValid = 3;
            const int credentialsAreValid = -1;

            if (password.Length< 1 && userName.Length< 1)
            {
                return fieldsAreAmpty;
            }

            if (userName.Length< 1 || !Regex.IsMatch(userName, "[a-z]+"))
            {
                return userNameNotValid;
            }

            if (password.Length< 1)
            {
                return passwordNotValid;
            }

            return credentialsAreValid;
        }
    }
}
