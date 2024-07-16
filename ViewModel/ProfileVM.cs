using Model;
using Service.ModelServices;
using ViewModel.Utilities;

namespace ViewModel
{
    public class ProfileVM : BaseVM
    {
        private readonly Employee employee;
        private EmployeeService employeeService = new();

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string _emailTemp;
        public string EmailTemp
        {
            get { return _emailTemp; }
            set { _emailTemp = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private string _phoneNumberTemp;
        public string PhoneNumberTemp
        {
            get { return _phoneNumberTemp; }
            set { _phoneNumberTemp = value; OnPropertyChanged(); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }

        private string _addressTemp;
        public string AddressTemp
        {
            get { return _addressTemp; }
            set { _addressTemp = value; OnPropertyChanged(); }
        }

        public string Password { get; set; }

        private string _passwordReset;
        public string PasswordReset
        {
            get { return _passwordReset; }
            set { _passwordReset = value; OnPropertyChanged(); OnPropertyChanged(nameof(CorrectPassword)); }
        }

        private string _passwordConfirmation;
        public string PasswordConfirmation
        {
            get { return _passwordConfirmation; }
            set { _passwordConfirmation = value; OnPropertyChanged(); OnPropertyChanged(nameof(CorrectPassword)); }
        }

        public bool CorrectPassword { get => !string.IsNullOrEmpty(PasswordReset) && PasswordReset == PasswordConfirmation; }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = employee.FullName; OnPropertyChanged(); }
        }

        private string _employeeNumber;
        public string EmployeeNumber
        {
            get { return _employeeNumber; }
            set { _employeeNumber = employee.EmployeeNumber; OnPropertyChanged(); }
        }

        private OccupationType _occupation;
        public OccupationType Occupation
        {
            get { return _occupation; }
            set { _occupation = employee.Occupation; OnPropertyChanged(); }
        }

        private ContractType _contract;
        public ContractType Contract
        {
            get { return _contract; }
            set { _contract = employee.Contract; OnPropertyChanged(); }
        }

        private DateOnly _hireDate;
        public DateOnly HireDate
        {
            get { return _hireDate; }
            set { _hireDate = employee.HireDate; OnPropertyChanged(); }
        }

        public ICommandAsync ChangeEmailCommand { get; set; }
        public ICommandAsync ChangePhoneNumberCommand { get; set; }
        public ICommandAsync ChangeAddressCommand { get; set; }
        public ICommandAsync ChangePasswordCommand { get; set; }

        public ProfileVM()
        {
            employee = employeeService.GetEmployeeByNumber("12345678");
            LoadEmployeeData();
            LoadCommands();
        }

        private void LoadCommands()
        {
            ChangeEmailCommand = new AsyncRelayCommand(ChangeEmail, CanChange);
            ChangePhoneNumberCommand = new AsyncRelayCommand(ChangePhoneNumber, CanChange);
            ChangeAddressCommand = new AsyncRelayCommand(ChangeAddress, CanChange);
            ChangePasswordCommand = new AsyncRelayCommand(ChangePasswordAsync);
        }

        private void LoadEmployeeData()
        {
            Email = employee.Email;
            PhoneNumber = employee.PhoneNumber;
            Address = employee.Address;
            Password = "••••••••";
            FullName = employee.FullName;
            EmployeeNumber = employee.EmployeeNumber;
            Occupation = employee.Occupation;
            Contract = employee.Contract;
            HireDate = employee.HireDate;
        }

        private bool CanChange(object parameter)
        {
            return parameter is string input && !string.IsNullOrEmpty(input);
        }

        public bool CanChangePassword(object parameter)
        {
            return !string.IsNullOrEmpty(PasswordReset) && PasswordReset == PasswordConfirmation; 
        }

        private async Task ChangeEmail(object parameter)
        {
            string newEmail = parameter as string;
            Email = newEmail;
            employee.Email = newEmail;
            EmailTemp = string.Empty;

            await employeeService.UpdateEmployeeAsync(employee);
        }

        private async Task ChangePhoneNumber(object parameter)
        {
            string newPhoneNumber = parameter as string;
            PhoneNumber = newPhoneNumber;
            employee.PhoneNumber = newPhoneNumber;
            PhoneNumberTemp = string.Empty;

            await employeeService.UpdateEmployeeAsync(employee);
        }

        private async Task ChangeAddress(object parameter)
        {
            string newAddress = parameter as string;
            Address = newAddress;
            employee.Address = newAddress;
            AddressTemp = string.Empty;

            await employeeService.UpdateEmployeeAsync(employee);
        }

        private async Task ChangePasswordAsync(object parameter)
        {
            string newPassword = PasswordReset;
            PasswordReset = string.Empty;
            PasswordConfirmation = string.Empty;

            await employeeService.UpdateEmployeePasswordAsync(employee, newPassword);
        }
    }
}