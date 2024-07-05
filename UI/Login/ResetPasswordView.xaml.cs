using Messages;
using Microsoft.IdentityModel.Tokens;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI.Login
{
    public partial class ResetPasswordView : UserControl
    {
        private string deepLink;

        public static readonly RoutedEvent ResetPasswordEvent = EventManager.RegisterRoutedEvent(
        "ResetPasswordClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LoginView));

        public event RoutedEventHandler ResetPasswordClick
        {
            add { AddHandler(ResetPasswordEvent, value); }
            remove { RemoveHandler(ResetPasswordEvent, value); }
        }

        public static readonly RoutedEvent BackToLoginResetEvent = EventManager.RegisterRoutedEvent(
        "BackToLoginReset", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LoginView));

        public event RoutedEventHandler BackToLoginReset
        {
            add { AddHandler(BackToLoginResetEvent, value); }
            remove { RemoveHandler(BackToLoginResetEvent, value); }
        }

        public ResetPasswordView(string deepLink)
        {
            InitializeComponent();
            this.deepLink = deepLink;
        }

        private async void ResetPasswordButton_Click(object sender, EventArgs e)
        {
            bool passwordReset = await ResetPassword();

            if (passwordReset)
            {
                RaiseEvent(new RoutedEventArgs(ResetPasswordEvent));
                ResetView();
            }
        }

        private bool ValidPassword()
        {
            return txtPassword.Text == txtConfirmPassword.Password;
        }

        private bool EmptyFields()
        {
            return txtPassword.Text.IsNullOrEmpty() || txtConfirmPassword.Password.IsNullOrEmpty();
        }

        private async Task<bool> ResetPassword()
        {
            EmployeeService employeeService = new();

            if (EmptyFields())
            {
                SetErrorState(UIMessages.EmptyFields);
                return false;
            }

            if (!ValidPassword())
            {
                SetErrorState(UIMessages.PasswordsDoNotMatch);
                return false;
            }

            string employeeNumber = RetreiveEmployeeNumber(deepLink);
            await employeeService.UpdateEmployeePasswordAsync(employeeNumber, txtPassword.Text);
            return true;
        }

        private void SetErrorState(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;
            txtPassword.Style = (Style)FindResource("LoginErrorTextBoxStyle");
            txtConfirmPassword.Style = (Style)FindResource("LoginErrorPasswordBoxStyle");
        }

        private string RetreiveEmployeeNumber(string deepLink)
        {
            int startIndex = deepLink.IndexOf("userId=") + "userId=".Length;
            int endIndex = deepLink.IndexOf('&', startIndex);

            return deepLink[startIndex..endIndex];
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Style = (Style)FindResource("LoginTextBoxStyle");
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            passwordBox.Style = (Style)FindResource("LoginPasswordBoxStyle");
        }

        private void BackToLoginButton_Click(object sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BackToLoginResetEvent));
            ResetView();
        }

        private void ResetView()
        {
            lblErrorMessage.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Password = string.Empty;
            txtPassword.Style = (Style)FindResource("LoginTextBoxStyle");
            txtConfirmPassword.Style = (Style)FindResource("LoginPasswordBoxStyle");
        }
    }
}
