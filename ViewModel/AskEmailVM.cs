using Messages;
using Microsoft.IdentityModel.Tokens;
using Model;
using Service;
using Service.ModelServices;
using System.Windows.Input;
using ViewModel.Utilities;

namespace ViewModel
{
    public class AskEmailVM : BaseVM
    {
        private string _email = string.Empty;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); TextBoxError = false; }
        }

        private string _message = string.Empty;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        private bool _emailMessageError = false;
        public bool EmailMessageError
        {
            get { return _emailMessageError; }
            set { _emailMessageError = value; OnPropertyChanged(); }
        }

        private bool _textboxError = false;
        public bool TextBoxError
        {
            get { return _textboxError; }
            set { _textboxError = value; OnPropertyChanged(); }
        }

        public ICommandAsync SendEmailCommand { get; set; }
        public ICommand BackToLoginCommand { get; set; }

        public AskEmailVM()
        {
            SendEmailCommand = new AsyncRelayCommand(RestorePasswordProcess);
            BackToLoginCommand = new RelayCommand(BackToLogin);
        }

        private void BackToLogin(object parameter)
        {
            EventAggregator.Instance.ChangeView("Login");
        }

        private async Task RestorePasswordProcess(object parameter)
        {
            if (Email.IsNullOrEmpty())
            {
                SetErrorState(UIMessages.EmptyFields);
                return;
            }

            if (!Email.Contains('@'))
            {
                SetErrorState(UIMessages.NotAnEmail);
                return;
            }

            EmployeeService _employeeService = new();

            Employee employee = await _employeeService.GetEmployeeByEmailAsync(Email);

            if (employee is null)
            {
                SetErrorState(UIMessages.InvalidEmail);
                return;
            }

            await CreateDeepLink(employee);
            ResetView();
            DisplayEmailSentMessage();

        }

        private async Task CreateDeepLink(Employee employee)
        {
            DeepLink deepLink = LinkGenerator.GenerateResetPasswordLink(employee, out string token);
            DeepLinkService deepLinkService = new();
            await deepLinkService.AddDeepLinkAsync(deepLink);

             await SendResetEmailAsync(employee, token);
        }

        private async Task SendResetEmailAsync(Employee employee, string token)
        {
            ResetPasswordEmailSender emailSender = new();
            await emailSender.SendEmailAsync(employee.Email, employee.FullName, LinkGenerator.GenerateWebLink(token));
        }

        private void SetErrorState(string errorMessage)
        {
            Message = errorMessage;
            EmailMessageError = true;
            TextBoxError = true;
        }

        private void ResetView()
        {
            Message = string.Empty;
            Email = string.Empty;
            EmailMessageError = false;
            TextBoxError = false;
        }

        private void DisplayEmailSentMessage()
        {
            Message = UIMessages.EmailSent;
        }
    }
}