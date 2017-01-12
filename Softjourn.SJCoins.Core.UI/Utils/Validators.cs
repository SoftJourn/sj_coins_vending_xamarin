
using System.Text.RegularExpressions;
using Softjourn.SJCoins.Core.UI.Presenters;


namespace Softjourn.SJCoins.Core.UI.Utils
{
    public class Validators
    {
        public static LoginPresenter.ValidateCredentialsResult ValidateCredentials(string userName, string password)
        {
            if (password.Length< 1 && userName.Length< 1)
            {
                return LoginPresenter.ValidateCredentialsResult.FieldsAreAmpty;
            }

            if (userName.Length< 1 || !Regex.IsMatch(userName, "[a-z]+"))
            {
                return LoginPresenter.ValidateCredentialsResult.UserNameNotValid;
            }

            if (password.Length< 1)
            {
                return LoginPresenter.ValidateCredentialsResult.PasswordNotValid;
            }

            return LoginPresenter.ValidateCredentialsResult.CredentialsAreValid;
        }

        
    }
}
