﻿using Model;
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
        private void Settings(object obj) => CurrentView = new SettingsVM();
        private void Logout(object obj) => EventAggregator.Instance.ChangeView("Login");
        private void Notifications(object obj) => CurrentView = new NotificationsVM();

        public EmplyeeNavigationVM(Employee employee)
        {
            _loggedInEmployee = employee;

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
    }
}