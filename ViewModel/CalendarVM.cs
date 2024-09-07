using Model;
using Service.ModelServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModel.Utilities;

namespace ViewModel
{
    public class CalendarVM : BaseVM
    {
        private ShiftService _shiftService = new();
        private RequestService _requestService = new();
        private AvailabilityService _availabilityService = new();
        private readonly Employee _employee;

        private ObservableCollection<Day> _daysOfMonth = new();
        public ObservableCollection<Day> DaysOfMonth
        {
            get { return _daysOfMonth; }
            set { _daysOfMonth = value; OnPropertyChanged(); }
        }

        private DateOnly _currentDate;
        public DateOnly CurrentDate
        {
            get { return _currentDate; }
            set { _currentDate = value; OnPropertyChanged(); }
        }

        private bool _isPopUpOpen;
        public bool IsPopUpOpen
        {
            get { return _isPopUpOpen; }
            set { _isPopUpOpen = value; OnPropertyChanged(); }
        }

        private Day _selectedDay;
        public Day SelectedDay
        {
            get { return _selectedDay; }
            set { _selectedDay = value; OnPropertyChanged(); }
        }

        private bool _requestSent = false;
        public bool RequestSent
        {
            get { return _requestSent; }
            set { _requestSent = value; OnPropertyChanged(); }
        }

        public ICommand GetNextMonthCommand { get; set; }
        public ICommand GetPreviousMonthCommand { get; set; }
        public ICommand OpenPopUpCommand { get; set; }
        public ICommand ClosePopUpCommand { get; set; }
        public ICommandAsync RequestAvailabilityCommand { get; set; }
        public ICommandAsync RequestUnavailabilityCommand { get; set; }

        public CalendarVM(Employee employee)
        {
            _employee = employee;
            CurrentDate = DateOnly.FromDateTime(DateTime.Now);
            LoadCalendar(_employee);
            LoadCommands();
        }

        private void LoadCommands()
        {
            GetNextMonthCommand = new RelayCommand(GetMonthExecute);
            GetPreviousMonthCommand = new RelayCommand(GetMonthExecute);
            OpenPopUpCommand = new RelayCommand(OpenPopUp);
            ClosePopUpCommand = new RelayCommand(ClosePopUp);
            RequestAvailabilityCommand = new AsyncRelayCommand(RequestAvailabilityAsync);
            RequestUnavailabilityCommand = new AsyncRelayCommand(RequestUnavailabilityAsync);
        }

        private async void LoadCalendar(Employee employee)
        {
            DateTime now = DateTime.Now;
            int year = now.Year;
            int month = now.Month;

            await SetDaysOfMonth(year, month, employee);
        }

        private async Task SetDaysOfMonth(int year, int month, Employee employee)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            DateTime firstDayOfMonth = new(year, month, 1);
            int offset = (int)firstDayOfMonth.DayOfWeek;

            for (int i = 0; i < offset; i++)
            {
                DaysOfMonth.Add(null);
            }

            foreach (var day in Enumerable.Range(1, daysInMonth))
            {
                DateTime date = new(year, month, day);
                Shift? shift = _shiftService.GetShiftForToday(employee, date);
                Request? vacation = null;
                bool available = await _availabilityService.IsAvailableAsync(_employee, date.DayOfWeek);

                if (shift is null)
                {
                    vacation = _requestService.GetVacationForToday(employee.EmployeeId, date);
                }

                DaysOfMonth.Add(new Day(date, available, shift, vacation));
            }
        }

        private void GetMonthExecute(object parameter)
        {
            CurrentDate = CurrentDate.AddMonths(int.Parse((string)parameter));
            DaysOfMonth.Clear();
            SetDaysOfMonth(CurrentDate.Year, CurrentDate.Month, _employee);
        }

        private void OpenPopUp(object parameter)
        {
            IsPopUpOpen = true;
            SelectedDay = parameter as Day;
        }

        private void ClosePopUp(object parameter)
        {
            IsPopUpOpen = false;
        }

        private async Task RequestAvailabilityAsync(object parameter)
        {
            Request changeAvailability = 
                new(1, RequestType.ScheduleChange, DateOnly.FromDateTime(DateTime.Now), SelectedDay.DayValue, SelectedDay.DayValue);
            await _requestService.AddRequestAsync(changeAvailability);
            RequestSent = true;
        }

        private async Task RequestUnavailabilityAsync(object parameter)
        {
            RequestSent = true;
            await _availabilityService.DeleteAvailabilityAsync(_employee, SelectedDay.DayValue.DayOfWeek);
        }
    }
}