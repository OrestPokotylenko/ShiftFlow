using Model;
using Service.ModelServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModel.Utilities;

namespace ViewModel
{
    public class SchedulesVM : BaseVM
    {
        private readonly ShiftService _shiftService = new();
        private readonly RequestService _requestService = new();
        private readonly EmployeeService _employeeService = new();
        private readonly AvailabilityService _availabilityService = new();
        private DateTime _selectedDate;
        private List<EmployeeShiftVM> _initializedEmployeeAndShift = new();

        private ObservableCollection<DateTime?> _daysOfMonth = new();
        public ObservableCollection<DateTime?> DaysOfMonth
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

        private ObservableCollection<EmployeeShiftVM>? _selectedEmployees;
        public ObservableCollection<EmployeeShiftVM>? SelectedEmployees
        {
            get { return _selectedEmployees; }
            set { _selectedEmployees = value; OnPropertyChanged(); }
        }

        private ObservableCollection<EmployeeShiftVM>? _availableEmployees;
        public ObservableCollection<EmployeeShiftVM>? AvailableEmployees
        {
            get { return _availableEmployees; }
            set { _availableEmployees = value; OnPropertyChanged(); }
        }

        private ObservableCollection<EmployeeShiftVM>? _unavailableEmployees;
        public ObservableCollection<EmployeeShiftVM>? UnavailableEmployees
        {
            get { return _unavailableEmployees; }
            set { _unavailableEmployees = value; OnPropertyChanged(); }
        }

        private TimeSpan _totalHours;
        public TimeSpan TotalHours
        {
            get { return _totalHours; }
            set { _totalHours = value; OnPropertyChanged(); }
        }

        private bool _isListOpen = false;
        public bool IsListOpen
        {
            get { return _isListOpen; }
            set { _isListOpen = value; OnPropertyChanged(); }
        }

        public ICommand GetNextMonthCommand { get; set; }
        public ICommand GetPreviousMonthCommand { get; set; }
        public ICommandAsync OpenListCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommandAsync SaveCommand { get; set; }

        public SchedulesVM()
        {
            LoadCalendar();
            LoadCommands();
        }

        private void LoadCommands()
        {
            GetNextMonthCommand = new RelayCommand(GetMonthExecute);
            GetPreviousMonthCommand = new RelayCommand(GetMonthExecute);
            OpenListCommand = new AsyncRelayCommand(OpenListExecuteAsync);
            DeleteCommand = new RelayCommand(DeleteExecute);
            AddCommand = new RelayCommand(AddExecute);
            SaveCommand = new AsyncRelayCommand(SaveExecuteAsync);
        }

        private void GetMonthExecute(object parameter)
        {
            CurrentDate = CurrentDate.AddMonths(int.Parse((string)parameter));
            DaysOfMonth.Clear();
            SetDaysOfMonth(CurrentDate.Year, CurrentDate.Month);
        }

        private async Task SaveExecuteAsync(object parameter)
        {
            IsListOpen = false;
            (List<EmployeeShiftVM> employeesToDelete, List<EmployeeShiftVM> employeesToAdd) = CompareChosenEmployees(_initializedEmployeeAndShift, SelectedEmployees.ToList());
            (List<Shift> shiftsToAdd, List<Employee> employeesShiftsToRemove) = CreateShift(employeesToAdd, employeesToDelete);

            SelectedEmployees.Clear();
            await _shiftService.DeleteShiftsAsync(employeesShiftsToRemove, _selectedDate);
            await _shiftService.AddShiftsAsync(shiftsToAdd);
        }

        private (List<Shift>, List<Employee>) CreateShift(List<EmployeeShiftVM> employeesToAdd, List<EmployeeShiftVM> employeesToDelete)
        {
            List<Shift> shiftsToAdd = [];
            List<Employee> employeesShiftsToRemove = [];

            foreach (var employeeAndShift in employeesToDelete)
            {
                employeesShiftsToRemove.Add(employeeAndShift.Employee);
            }

            foreach (var employeeAndShift in employeesToAdd)
            {
                (DateTime startTime, DateTime endTime) = employeeAndShift.WorkingTime.GetWorkingTime(_selectedDate);
                shiftsToAdd.Add(new Shift(employeeAndShift.Employee.EmployeeId, startTime, endTime, employeeAndShift.SelectedDepartment));
            }

            return (shiftsToAdd, employeesShiftsToRemove);
        }

        private (List<EmployeeShiftVM>, List<EmployeeShiftVM>) CompareChosenEmployees(List<EmployeeShiftVM> oldList, List<EmployeeShiftVM> newList)
        {
            List<EmployeeShiftVM> employeesToDelete = [];
            List<EmployeeShiftVM> employeesToAdd = [];

            foreach (var employee in oldList)
            {
                if (!newList.Contains(employee))
                {
                    employeesToDelete.Add(employee);
                }
            }

            foreach (var employee in newList)
            {
                if (!oldList.Contains(employee))
                {
                    employeesToAdd.Add(employee);
                }
            }

            return (employeesToDelete, employeesToAdd);
        }

        private async Task OpenListExecuteAsync(object parameter)
        {
            IsListOpen = true;

            if (!IsListOpen)
            {
                if (SelectedEmployees.Count > 0)
                {
                    SaveCommand.Execute(null);
                }

                return;
            }

            _selectedDate = (DateTime)parameter;
            DayOfWeek dayOfWeek = _selectedDate.DayOfWeek;
            await LoadEmployees(dayOfWeek);
        }

        private async Task LoadEmployees(DayOfWeek dayOfWeek)
        {
            AvailableEmployees = [];
            UnavailableEmployees = [];
            SelectedEmployees = [];
            List<Employee>? employeesWithShift = await _employeeService.GetEmployeesWithShiftAsync(_selectedDate);
            List<Availability>? availableEmployees = await _availabilityService.GetAvailabilitiesByDateAsync(dayOfWeek);
            List<Employee>? unavailableEmployees = await _employeeService.GetUnavailableEmployeesByDateAsync(dayOfWeek);

            foreach (var employee in employeesWithShift)
            {
                Shift? shift = _shiftService.GetShiftForToday(employee, _selectedDate);

                if (shift is not null)
                {
                    EmployeeShiftVM employeeShiftVM = new() { Employee = employee, WorkingTime = new(this, shift.StartTime, shift.EndTime), IsAvailable = true };
                    SelectedEmployees.Add(employeeShiftVM);
                    _initializedEmployeeAndShift.Add(employeeShiftVM);
                }
            }

            foreach (var availability in availableEmployees)
            {
                AvailableEmployees.Add(
                    new EmployeeShiftVM { Employee = availability.Employee, WorkingTime = new(this), IsAvailable = true }); 
            }

            foreach (var employee in unavailableEmployees)
            {
                UnavailableEmployees.Add(
                    new EmployeeShiftVM { Employee = employee, WorkingTime = new(this), IsAvailable = false });
            }
        }

        private void DeleteExecute(object parameter)
        {
            EmployeeShiftVM employee = (EmployeeShiftVM)parameter;
            SelectedEmployees.Remove(employee);
            employee.WorkingTime = new(this);
            SetTotalHours();

            if (employee.IsAvailable)
            {
                AvailableEmployees.Add(employee);
            }
            else
            {
                UnavailableEmployees.Add(employee);
            }
        }

        private void AddExecute(object parameter)
        {
            EmployeeShiftVM employee = (EmployeeShiftVM)parameter;
            SelectedEmployees.Add(employee);
            SetTotalHours();

            if (employee.IsAvailable)
            {
                AvailableEmployees.Remove(employee);
            }
            else
            {
                UnavailableEmployees.Remove(employee);
            }
        }

        private void SetDaysOfMonth(int year, int month)
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
                DaysOfMonth.Add(date);
            }
        }

        private void LoadCalendar()
        {
            CurrentDate = DateOnly.FromDateTime(DateTime.Now);

            DateTime now = DateTime.Now;
            int year = now.Year;
            int month = now.Month;

            SetDaysOfMonth(year, month);
        }

        internal void SetTotalHours()
        {
            TotalHours = new();

            foreach (var employee in SelectedEmployees)
            {
                (DateTime startTime, DateTime endTime) = employee.WorkingTime.GetWorkingTime(_selectedDate);
                TotalHours += endTime - startTime;
            }
        }
    }
}