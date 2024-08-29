using System.Windows.Input;
using ViewModel.Utilities;
using Service.ModelServices;
using GalaSoft.MvvmLight.Messaging;
using Model;

namespace ViewModel
{
    public class MainVM : BaseVM
    {
        private readonly EmployeeService _employeeService = new();

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand LoginViewCommand { get; set; }
        public ICommand AskEmailViewCommand { get; set; }
        public ICommand ResetPasswordViewCommand { get; set; }
        public ICommand EmployeeViewCommand { get; set; }
        public ICommand ManagerViewCommand { get; set; }

        private void LoginView(object obj) => CurrentView = new LoginVM();
        private void AskEmailView(object obj) => CurrentView = new AskEmailVM();
        private void ResetPasswordView(object obj) => CurrentView = new ResetPasswordVM();
        private void EmployeeView(object obj) => CurrentView = new EmplyeeNavigationVM();
        private void ManagerView(object obj) => CurrentView = new ManagerNavigationVM();

        public MainVM()
        {
            LoginViewCommand = new RelayCommand(LoginView);
            AskEmailViewCommand = new RelayCommand(AskEmailView);
            ResetPasswordViewCommand = new RelayCommand(ResetPasswordView);
            EmployeeViewCommand = new RelayCommand(EmployeeView);
            ManagerViewCommand = new RelayCommand(ManagerView);

            EventAggregator.Instance.OnChangeView += ChangeView;
            Messenger.Default.Register<Employee>(this, LoginEmployee);

            Task.Run(_employeeService.WarmUp);
            ProcessArgs(null);
        }

        private void ChangeView(string viewName)
        {
            switch (viewName)
            {
                case "Login":
                    ShowLoginView();
                    break;

                case "AskEmail":
                    ShowAskEmailView();
                    break;

                case "ResetPassword":
                    ShowResetPasswordView(viewName);
                    break;
            }
        }

        private void LoginEmployee(Employee employee)
        {
            if (employee.Occupation is not OccupationType.Manager)
            {
                ShowEmployeeMainView();
            }
            else
            {
                ShowManagerMainView();
            }
        }

        public void ProcessArgs(string? deepLink)
        {
            if (deepLink != null)
            {
                ProcessDeepLink(deepLink);
            }
            else
            {
                ShowLoginView();
            }
        }

        private void ProcessDeepLink(string deepLink)
        {
            if (deepLink.StartsWith("shiftflow://reset"))
            {
                ShowResetPasswordView(deepLink);
            }
        }

        private void ShowLoginView()
        {
            LoginViewCommand.Execute(null);
        }

        private void ShowResetPasswordView(string deepLink)
        {
            ResetPasswordViewCommand.Execute(deepLink);
        }

        private void ShowAskEmailView()
        {
            AskEmailViewCommand.Execute(null);
        }

        private void ShowEmployeeMainView()
        {
            EmployeeViewCommand.Execute(null);
        }

        private void ShowManagerMainView()
        {
            ManagerViewCommand.Execute(null);
        }
    }
}