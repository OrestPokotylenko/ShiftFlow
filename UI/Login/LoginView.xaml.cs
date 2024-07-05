using System.Windows.Controls;
using System.Windows;
using Service;
using Model;
using ErrorMessages;
using Microsoft.IdentityModel.Tokens;

namespace UI
{
    public partial class LoginView : UserControl
    {
        private EmployeeService employeeService = new();

        public static readonly RoutedEvent LoginEvent = EventManager.RegisterRoutedEvent(
            "Login", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LoginView));

        public event RoutedEventHandler Login
        {
            add { AddHandler(LoginEvent, value); }
            remove { RemoveHandler(LoginEvent, value); }
        }

        public static readonly RoutedEvent ForgotPasswordEvent = EventManager.RegisterRoutedEvent(
            "ForgotPassword", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LoginView));

        public event RoutedEventHandler ForgotPassword
        {
            add { AddHandler(ForgotPasswordEvent, value); }
            remove { RemoveHandler(ForgotPasswordEvent, value); }
        }

        public LoginView()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            Employee employee = await LoginAsync();

            if (employee != null)
            {
                RaiseEvent(new RoutedEventArgs(LoginEvent));
                ResetView();
            }
        }

        private async Task<Employee> LoginAsync()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (FieldsEmpty(username, password))
            {
                SetErrorState(UIErrorMessages.EmptyFields);
                return null;
            }

            Employee employee = await employeeService.GetEmployeeAsync(username, password);

            if (employee == null)
            {
                SetErrorState(UIErrorMessages.InvalidCredentials);
            }

            return employee;
        }

        private bool FieldsEmpty(string username, string password)
        {
            return username.IsNullOrEmpty() || password.IsNullOrEmpty();
        }

        private void SetErrorState(string errorMessage)
        {
            lblErrorMessage.Text = errorMessage;

            txtPassword.Style = (Style)FindResource("LoginErrorPasswordBoxStyle");
            txtUsername.Style = (Style)FindResource("LoginErrorTextBoxStyle");
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Style = (Style)FindResource("LoginTextBoxStyle");
        }

        private void PasswordBox_GotFocus(object sender, EventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            passwordBox.Style = (Style)FindResource("LoginPasswordBoxStyle");
        }

        private void ForgotPasswordButton_Click(object sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ForgotPasswordEvent));
            ResetView();
        }

        private void ResetView()
        {
            lblErrorMessage.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtUsername.Style = (Style)FindResource("LoginTextBoxStyle");
            txtPassword.Style = (Style)FindResource("LoginPasswordBoxStyle");
        }
    }
}
