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

        public LoginView()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = await LoginAsync();

            if (employee != null)
            {
                MessageBox.Show($"Welcome, {employee.FirstName}!");
            }
        }

        private async Task<Employee> LoginAsync()
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

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

            Style errorStyle = (Style)FindResource("LoginErrorTextBoxStyle");
            txtPassword.Style = errorStyle;
            txtUsername.Style = errorStyle;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Style = (Style)FindResource("LoginTextBoxStyle");
        }
    }
}
