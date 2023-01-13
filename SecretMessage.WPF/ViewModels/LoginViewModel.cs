using Firebase.Auth;
using MVVMEssentials.ViewModels;
using MVVMEssentials.Commands;
using SecretMessage.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVMEssentials.Services;
using SecretMessage.WPF.Stores;

namespace SecretMessage.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand SubmitCommand { get; }
        public ICommand NavigateRegisterCommand { get; }

        public LoginViewModel(AuthenticationStore authenticationStore ,INavigationService registerNavigationService, INavigationService homeNavigationService)
        {
            SubmitCommand = new LoginCommand(this, authenticationStore, homeNavigationService);
            NavigateRegisterCommand = new NavigateCommand(registerNavigationService);
        }
    }
}
