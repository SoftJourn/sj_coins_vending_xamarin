using Softjourn.SJCoins.Core.Interfaces;
using MvvmCross.Core.ViewModels;

namespace Softjourn.SJCoins.Core.UI.ViewModels
{
	public class LoginViewModel: MvxViewModel
	{
		private readonly ILoginService _loginService;

		public LoginViewModel(ILoginService loginService)
		{
			_loginService = loginService;
		}

		private string _username;
		public string Username
		{
			get 
			{
				return _username;
			}

			set
			{
				SetProperty(ref _username, value);
				RaisePropertyChanged(() => Username);
			}
		}

		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}

			set
			{
				SetProperty(ref _password, value);
				RaisePropertyChanged(() => Password);
			}
		}

		private IMvxCommand _loginCommand;
		public virtual IMvxCommand LoginCommand
		{
			get
			{
				_loginCommand = _loginCommand ?? new MvxCommand(AttemptLogin, CanExecuteLogin);
				return _loginCommand;
			}
		}

		private void AttemptLogin()
		{
			if (_loginService.Login(Username, Password))
			{
				//ShowViewModel<MainViewModel>();
			}
			else
			{
				// Show alert
				//_dialogService.Alert("We were unable to log you in!", "Login Failed", "OK");
			}
		}

		private bool CanExecuteLogin()
		{
			return (!string.IsNullOrEmpty(Username) || !string.IsNullOrWhiteSpace(Username))
				   && (!string.IsNullOrEmpty(Password) || !string.IsNullOrWhiteSpace(Password));
		}
	}
}
