using Model;
using Service.ModelServices;
using System.Windows.Input;
using ViewModel.Utilities;

namespace ViewModel
{
    public class EmplyeeNavigationVM : BaseVM
    {
        private readonly Employee _loggedInEmployee;

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        private bool _newNotification;
        public bool NewNotification
        {
            get { return _newNotification; }
            set { _newNotification = value; OnPropertyChanged(); }
        }

        private bool _notificationsChecked = false;
        public bool NotificationsChecked
        {
            get { return _notificationsChecked; }
            set { _notificationsChecked = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand CalendarCommand { get; set; }
        public ICommand ReplaceCommand { get; set; }
        public ICommand VacationCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand NotificationsCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM(_loggedInEmployee);
        private void Calendar(object obj) => CurrentView = new CalendarVM(_loggedInEmployee);
        private void Replace(object obj) => CurrentView = new ReplaceVM(_loggedInEmployee);
        private void Vacation(object obj) => CurrentView = new VacationVM(_loggedInEmployee);
        private void Profile(object obj) => CurrentView = new ProfileVM(_loggedInEmployee);
        private void Settings(object obj) => CurrentView = new SettingsVM(_loggedInEmployee);
        private void Logout(object obj) => EventAggregator.Instance.ChangeView("Login");
        private void Notifications(object obj)
        {
            CurrentView = new NotificationsVM(_loggedInEmployee);
            NotificationsChecked = true;
        }

        public EmplyeeNavigationVM(Employee employee)
        {
            _loggedInEmployee = employee;
            GetNewNotification();

            HomeCommand = new RelayCommand(Home);
            CalendarCommand = new RelayCommand(Calendar);
            ReplaceCommand = new RelayCommand(Replace);
            VacationCommand = new RelayCommand(Vacation);
            ProfileCommand = new RelayCommand(Profile);
            SettingsCommand = new RelayCommand(Settings);
            LogoutCommand = new RelayCommand(Logout);
            NotificationsCommand = new RelayCommand(Notifications);

            Home(null);
        }

        private async Task GetNewNotification()
        {
            RequestService requestService = new();
            List<Request>? notifications = await requestService.GetReplaceRequests(_loggedInEmployee);

            NewNotification = notifications != null && notifications.Count > 0;
        }
    }
}