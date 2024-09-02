using Model;
using Service.ModelServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ViewModel.Utilities;

namespace ViewModel
{
    public class ReplaceVM : BaseVM
    {
        private readonly Employee _employee;

        private ObservableCollection<Shift>? _shiftsForWeek;
        public ObservableCollection<Shift>? ShiftsForWeek
        {
            get { return _shiftsForWeek; }
            set { _shiftsForWeek = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Employee> _freeEmployees;
        public ObservableCollection<Employee> FreeEmployees
        {
            get { return _freeEmployees; }
            set { _freeEmployees = value; OnPropertyChanged(); }
        }

        private Shift _selectedShift;
        public Shift SelectedShift
        {
            get { return _selectedShift; }
            set { _selectedShift = value; OnPropertyChanged(); }
        }

        private bool _requestSent = false;
        public bool RequestSent
        {
            get { return _requestSent; }
            set { _requestSent = value; OnPropertyChanged(); }
        }

        public ICommand SelectShiftCommand { get; set; }
        public ICommandAsync ReplaceCommand { get; set; }

        public ReplaceVM(Employee employee)
        {
            _employee = employee;
            ShiftService _shiftService = new();
            DateTime startDate = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
            ShiftsForWeek = new(_shiftService.GetShiftsForWeek(_employee, startDate));
            SelectShiftCommand = new RelayCommand(SelectShiftExecute);
            ReplaceCommand = new AsyncRelayCommand(AskReplaceAsync);
        }

        private ObservableCollection<Employee> GetFreeEmployees()
        {
            EmployeeService _employeeService = new();
            return new(_employeeService.GetFreeEmployees(_employee, SelectedShift.StartTime, SelectedShift.EndTime));
        }

        private void SelectShiftExecute(object parameter)
        {
            SelectedShift = parameter as Shift;
            FreeEmployees = GetFreeEmployees();
        }

        private async Task AskReplaceAsync(object parameter)
        {
            RequestSent = true;
            RequestService _requestService = new();
            Employee selectedEmployee = parameter as Employee;
            DateOnly requestDate = DateOnly.FromDateTime(DateTime.Now);
            Request request = 
                new(selectedEmployee.EmployeeId, RequestType.Replace, requestDate, SelectedShift.StartTime, SelectedShift.EndTime, null, SelectedShift.ShiftId);

            _requestService.AddRequestAsync(request);
        }
    }
}