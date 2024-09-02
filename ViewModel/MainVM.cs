using System.Windows.Input;
using ViewModel.Utilities;
using Service.ModelServices;
using GalaSoft.MvvmLight.Messaging;
using Model;
using Microsoft.Extensions.DependencyInjection;

namespace ViewModel
{
    public class MainVM : BaseVM
    {
        private readonly EmployeeService _employeeService = new();
        private readonly IServiceProvider _serviceProvider;
        private Employee _loggedInEmployee;

        private object? _currentView;
        public object? CurrentView
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
        private void EmployeeView(object obj) => CurrentView = new EmplyeeNavigationVM(_loggedInEmployee);
        private void ManagerView(object obj) => CurrentView = new ManagerNavigationVM(_loggedInEmployee);

        public MainVM(IServiceProvider serviceProvider, string deepLink)
        {
            _serviceProvider = serviceProvider;

            LoginViewCommand = new RelayCommand(LoginView);
            AskEmailViewCommand = new RelayCommand(AskEmailView);
            ResetPasswordViewCommand = new RelayCommand(ResetPasswordView);
            EmployeeViewCommand = new RelayCommand(EmployeeView);
            ManagerViewCommand = new RelayCommand(ManagerView);

            EventAggregator.Instance.OnChangeView += ChangeView;
            Messenger.Default.Register<Employee>(this, LoginEmployee);
            Messenger.Default.Register<string>(this, CurrentDeepLink);

            Task.Run(_employeeService.WarmUp);
            ProcessArgs(deepLink);
        }

        private void ResetPasswordView(object obj)
        {
            string deepLink = obj as string;
            var resetPasswordVM = ActivatorUtilities.CreateInstance<ResetPasswordVM>(_serviceProvider, deepLink);
            CurrentView = resetPasswordVM;
        }

        private void CurrentDeepLink(string deepLink)
        {
            ResetPasswordView(deepLink);
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
            }
        }

        private void LoginEmployee(Employee employee)
        {
            _loggedInEmployee = employee;

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
            if (deepLink is not null)
            {
                ProcessDeepLink(deepLink);
            }
            else
            {
                ShowLoginView();
            }
        }

        public void ProcessDeepLink(string deepLink)
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