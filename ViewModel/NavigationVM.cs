using System.Windows.Input;
using ViewModel.Utilities;

namespace ViewModel
{
    public class NavigationVM : BaseVM
    {
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

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Calendar(object obj) => CurrentView = new CalendarVM();
        private void Replace(object obj) => CurrentView = new ReplaceVM();
        private void Vacation(object obj) => CurrentView = new VacationVM();
        private void Profile(object obj) => CurrentView = new ProfileVM();
        private void Settings(object obj) => CurrentView = new SettingsVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            CalendarCommand = new RelayCommand(Calendar);
            ReplaceCommand = new RelayCommand(Replace);
            VacationCommand = new RelayCommand(Vacation);
            ProfileCommand = new RelayCommand(Profile);
            SettingsCommand = new RelayCommand(Settings);

            CurrentView = new HomeVM();
        }
    }
}