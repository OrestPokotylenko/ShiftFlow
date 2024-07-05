using Microsoft.IdentityModel.Tokens;
using Model;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI.Login
{
    public partial class AskEmailView : UserControl
    {
        private EmployeeService employeeService = new();

        public static readonly RoutedEvent BackToLoginAskEmailEvent = EventManager.RegisterRoutedEvent(
        "BackToLoginAskEmail", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LoginView));

        public event RoutedEventHandler BackToLoginAskEmail
        {
            add { AddHandler(BackToLoginAskEmailEvent, value); }
            remove { RemoveHandler(BackToLoginAskEmailEvent, value); }
        }

        public AskEmailView()
        {
            InitializeComponent();
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            RestorePasswordProcess();
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BackToLoginAskEmailEvent));
            ResetView();
        }


        private async void SendResetEmailAsync(Employee employee, string token)
        {
            ResetPasswordEmailSender emailSender = new();
            await emailSender.SendEmailAsync(employee.Email, employee.FullName, LinkGenerator.GenerateWebLink(token));
        }

        private async void CreateDeepLink(Employee employee)
        {
            DeepLink deepLink = LinkGenerator.GenerateResetPasswordLink(employee, out string token);
            DeepLinkService deepLinkService = new();
            await deepLinkService.AddDeepLinkAsync(deepLink);

            SendResetEmailAsync(employee, token);
        }

        private async void RestorePasswordProcess()
        {
            if (txtEmail.Text.IsNullOrEmpty())
            {
                SetErrorState(ErrorMessages.UIErrorMessages.EmptyFields);
                return;
            }

            Employee employee = await employeeService.GetEmployeeByEmailAsync(txtEmail.Text);

            if (employee == null)
            {
                SetErrorState(ErrorMessages.UIErrorMessages.InvalidEmail);
            }
            else
            {
                CreateDeepLink(employee);
                ResetView();
            }
        }

        private void SetErrorState(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            txtEmail.Style = (Style)FindResource("LoginErrorTextBoxStyle");
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Style = (Style)FindResource("LoginTextBoxStyle");
        }

        private void ResetView()
        {
            lblErrorMessage.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtEmail.Style = (Style)FindResource("LoginTextBoxStyle");
        }
    }
}
