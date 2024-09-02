using ViewModel.Utilities;
using Service.ModelServices;
using System.Windows.Input;
using Model;
using Messages;

namespace ViewModel
{
    public class VacationVM : BaseVM
    {
        private readonly Employee _employee;
        private readonly RequestService _requestService = new();

        private bool _fullDay = true;
        public bool FullDay
        {
            get { return _fullDay; }
            set { _fullDay = value; OnPropertyChanged(); LoadCalendarDates(); }
        }

        private bool _dayOffCallendarOpen = false;
        public bool DayOffCallendarOpen
        {
            get { return _dayOffCallendarOpen; }
            set { _dayOffCallendarOpen = value; OnPropertyChanged(); }
        }

        private bool _vacationStartDateOpen = false;
        public bool VacationStartDateOpen
        {
            get { return _vacationStartDateOpen; }
            set { _vacationStartDateOpen = value; OnPropertyChanged(); }
        }

        private bool _vacationEndDateOpen = false;
        public bool VacationEndDateOpen
        {
            get { return _vacationEndDateOpen; }
            set { _vacationEndDateOpen = value; OnPropertyChanged(); }
        }

        private DateTime _dayOffDate;
        public DateTime DayOffDate
        {
            get { return _dayOffDate; }
            set { _dayOffDate = value; OnPropertyChanged(); }
        }

        private DateTime _vacationStartDate;
        public DateTime VacationStartDate
        {
            get { return _vacationStartDate; }
            set { _vacationStartDate = value; OnPropertyChanged(); SubmitCommand.RaiseCanExecuteChanged(); }
        }

        private DateTime _vacationEndDate;
        public DateTime VacationEndDate
        {
            get { return _vacationEndDate; }
            set { _vacationEndDate = value; OnPropertyChanged(); SubmitCommand.RaiseCanExecuteChanged(); }
        }

        private bool _dayOffStartOpen = false;
        public bool DayOffStartOpen
        {
            get { return _dayOffStartOpen; }
            set { _dayOffStartOpen = value; OnPropertyChanged(); }
        }

        private bool _dayOffEndOpen = false;
        public bool DayOffEndOpen
        {
            get { return _dayOffEndOpen; }
            set { _dayOffEndOpen = value; OnPropertyChanged(); }
        }

        private TimeSpan? _selectedTimeStart;
        public TimeSpan? SelectedTimeStart
        {
            get { return _selectedTimeStart; }
            set { _selectedTimeStart = value; OnPropertyChanged(); SubmitCommand.RaiseCanExecuteChanged(); }
        }

        private TimeSpan? _selectedTimeEnd;
        public TimeSpan? SelectedTimeEnd
        {
            get { return _selectedTimeEnd; }
            set { _selectedTimeEnd = value; OnPropertyChanged(); SubmitCommand.RaiseCanExecuteChanged(); }
        }

        private string? _note;
        public string? Note
        {
            get { return _note; }
            set { _note = value; OnPropertyChanged(); }
        }

        private string? _errorMessage = "";
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand DayOffCalendarCommand { get; set; }
        public ICommand StartDateCalendarCommand { get; set; }
        public ICommand EndDateCallendarCommand { get; set; }
        public ICommand DayOffStartTimeCommand { get; set; }
        public ICommand DayOffEndTimeCommand { get; set; }
        public ICommandAsync SubmitCommand { get; set; }

        public VacationVM(Employee employee)
        {
            _employee = employee;
            LoadCommands();
            LoadCalendarDates();
        }

        private void OpenDayOffCalendar(object obj)
        {
            VacationStartDateOpen = false;
            VacationEndDateOpen = false;
            DayOffCallendarOpen = !DayOffCallendarOpen;
        }

        private void OpenStartDateCalendar(object obj)
        {
            DayOffCallendarOpen = false;
            VacationEndDateOpen = false;
            VacationStartDateOpen = !VacationStartDateOpen;
        }

        private void OpenEndDateCalendar(object obj)
        {
            DayOffCallendarOpen = false;
            VacationStartDateOpen = false;
            VacationEndDateOpen = !VacationEndDateOpen;
        }

        private void OpenDayOffStartTime(object obj)
        {
            DayOffEndOpen = false;
            DayOffStartOpen = !DayOffStartOpen;
        }

        private void OpenDayOffEndTime(object obj)
        {
            DayOffStartOpen = false;
            DayOffEndOpen = !DayOffEndOpen;
        }

        private bool CanExecute(object parameter)
        {
            if (!(VacationStartDate <= VacationEndDate && SelectedTimeStart <= SelectedTimeEnd))
            {
                SetErrorMessage(UIMessages.InvalidDate);
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
        }

        private async Task AddRequestAsync(object parameter)
        {
            DateOnly requestDate = DateOnly.FromDateTime(DateTime.Now);
            Request request = FullDay ? 
                CreateRequest(_employee.EmployeeId, RequestType.Vacation, requestDate, VacationStartDate, VacationEndDate, Note) : 
                CreateRequest(_employee.EmployeeId, RequestType.DayOff, requestDate, ConvertToDate(DayOffDate, SelectedTimeStart.Value), ConvertToDate(DayOffDate, SelectedTimeEnd.Value), Note);

            await _requestService.AddRequestAsync(request);

            ResetValues();
        }

        private Request CreateRequest(int employeeId, RequestType requestType, DateOnly requestDate, DateTime startDate, DateTime endDate, string note)
        {
            return new(employeeId, requestType, requestDate, startDate, endDate, note);
        }

        private DateTime ConvertToDate(DateTime date, TimeSpan time)
        {
            DateTime dateWithTime = date.Date + time;
            return dateWithTime;
        }

        private void ResetValues()
        {
            LoadCalendarDates();
            Note = string.Empty;
            ErrorMessage = string.Empty;
            FullDay = true;
            DayOffCallendarOpen = false;
            VacationStartDateOpen = false;
            VacationEndDateOpen = false;
            DayOffStartOpen = false;
            DayOffEndOpen = false;
        }

        private void LoadCommands()
        {
            DayOffCalendarCommand = new RelayCommand(OpenDayOffCalendar);
            StartDateCalendarCommand = new RelayCommand(OpenStartDateCalendar);
            EndDateCallendarCommand = new RelayCommand(OpenEndDateCalendar);
            DayOffStartTimeCommand = new RelayCommand(OpenDayOffStartTime);
            DayOffEndTimeCommand = new RelayCommand(OpenDayOffEndTime);
            SubmitCommand = new AsyncRelayCommand(AddRequestAsync, CanExecute);
        }

        private void LoadCalendarDates()
        {
            DateTime currentTime = DateTime.Now;

            DayOffDate = currentTime;
            VacationStartDate = currentTime;
            VacationEndDate = currentTime;
            SelectedTimeStart = currentTime.TimeOfDay;
            SelectedTimeEnd = currentTime.TimeOfDay;
        }

        private void SetErrorMessage(string message)
        {
            ErrorMessage = message;
        }
    }
}