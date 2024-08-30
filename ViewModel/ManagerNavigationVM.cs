using System.Windows.Input;
using ViewModel.Utilities;

namespace ViewModel
{
    public class ManagerNavigationVM : EmplyeeNavigationVM
    {
        public ICommand SchedulesCommand { get; set; }
        public ICommand RequestsCommand { get; set; }

        private void Schedules(object obj) => CurrentView = new SchedulesVM();
        private void Requests(object obj) => CurrentView = new RequestsVM();

        public ManagerNavigationVM()
        {
            SchedulesCommand = new RelayCommand(Schedules);
            RequestsCommand = new RelayCommand(Requests);

            CurrentView = new HomeVM();
        }
    }
}