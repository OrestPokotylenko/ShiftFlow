using Messages;
using Microsoft.IdentityModel.Tokens;
using Model;
using Service.ModelServices;
using System.Windows.Input;
using ViewModel.Utilities;
using GalaSoft.MvvmLight.Messaging;

namespace ViewModel
{
    public class LoginVM : BaseVM
    {
        private string _username = "";
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password = "";
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private bool _usernameError = false;
        public bool UsernameError
        {
            get { return _usernameError; }
            set { _usernameError = value; OnPropertyChanged(); }
        }

        private bool _passwordError = false;
        public bool PasswordError
        {
            get { return _passwordError; }
            set { _passwordError = value; OnPropertyChanged(); }
        }

        public ICommandAsync LoginCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        public LoginVM()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            ForgotPasswordCommand = new RelayCommand(ForgotPassword);
        }

        private async Task LoginAsync(object parameter)
        {
            if (!CanLogin())
            {
                SetErrorState(UIMessages.EmptyFields);
                return;
            }

            EmployeeService employeeService = new();
            Employee employee = await employeeService.GetEmployeeAsync(Username, Password);

            if (employee is null)
            {
                SetErrorState(UIMessages.InvalidCredentials);
                return;
            }

            Messenger.Default.Send(employee);
            ResetView();
        }

        private bool CanLogin()
        {
            return !Username.IsNullOrEmpty() && !Password.IsNullOrEmpty();
        }

        private void ForgotPassword(object parameter)
        {
            EventAggregator.Instance.ChangeView("AskEmail");
        }

        private void ResetView()
        {
            Username = "";
            Password = "";
            ErrorMessage = "";
            UsernameError = false;
            PasswordError = false;
        }

        private void SetErrorState(string errorMessage)
        {
            ErrorMessage = errorMessage;
            UsernameError = true;
            PasswordError = true;
        }
    }
}