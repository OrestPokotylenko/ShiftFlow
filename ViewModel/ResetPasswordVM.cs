using GalaSoft.MvvmLight.Messaging;
using Messages;
using Microsoft.IdentityModel.Tokens;
using Model;
using Service.ModelServices;
using System.Windows.Input;
using ViewModel.Utilities;

namespace ViewModel
{
    public class ResetPasswordVM : BaseVM
    {
        private string _deepLink;

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; OnPropertyChanged(); NewPasswordError = false; }
        }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set { _repeatPassword = value; OnPropertyChanged(); RepeatPasswordError = false; }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private bool _newPasswordError = false;
        public bool NewPasswordError
        {
            get { return _newPasswordError; }
            set { _newPasswordError = value; OnPropertyChanged(); }
        }

        private bool _repeatPasswordError = false;
        public bool RepeatPasswordError
        {
            get { return _repeatPasswordError; }
            set { _repeatPasswordError = value; OnPropertyChanged(); }
        }

        public ICommandAsync ResetPasswordCommand { get; set; }
        public ICommand BackToLoginCommand {  get; set; }

        public ResetPasswordVM(string deepLink)
        {
            _deepLink = deepLink;
            ResetPasswordCommand = new AsyncRelayCommand(ResetPassword);
            BackToLoginCommand = new RelayCommand(BackToLogin);
        }

        private void BackToLogin(object parameter)
        {
            EventAggregator.Instance.ChangeView("Login");
        }

        private bool ValidPassword()
        {
            return NewPassword == RepeatPassword;
        }

        private bool EmptyFields()
        {
            return NewPassword.IsNullOrEmpty() || RepeatPassword.IsNullOrEmpty();
        }

        private async Task ResetPassword(object parameter)
        {
            EmployeeService employeeService = new();

            if (EmptyFields())
            {
                SetErrorState(UIMessages.EmptyFields);
                return;
            }

            if (!ValidPassword())
            {
                SetErrorState(UIMessages.PasswordsDoNotMatch);
                return;
            }

            string employeeNumber = RetreiveEmployeeNumber(_deepLink);
            Employee employee = employeeService.GetEmployeeByNumber(employeeNumber);
            await employeeService.UpdateEmployeePasswordAsync(employee, NewPassword);
            BackToLoginCommand.Execute(null);
            ResetView();
        }

        private void SetErrorState(string errorMessage)
        {
            ErrorMessage = errorMessage;
            NewPasswordError = true;
            RepeatPasswordError = true;
        }

        private string RetreiveEmployeeNumber(string deepLink)
        {
            int startIndex = deepLink.IndexOf("userId=") + "userId=".Length;
            int endIndex = deepLink.IndexOf('&', startIndex);

            return deepLink[startIndex..endIndex];
        }

        private void ResetView()
        {
            NewPassword = string.Empty;
            RepeatPassword = string.Empty;
            ErrorMessage = string.Empty;
            NewPasswordError = false;
            RepeatPasswordError = false;
        }

        internal void SetDeepLink(string deepLink)
        {
            _deepLink = deepLink;
        }
    }
}