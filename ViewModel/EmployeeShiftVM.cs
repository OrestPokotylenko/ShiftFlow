using Model;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class EmployeeShiftVM : BaseVM
    {
        public ObservableCollection<DepartmentType> Departments { get; }

        private Employee _employee;
        public Employee Employee
        {
            get { return _employee; }
            set { _employee = value; OnPropertyChanged(); }
        }

        private WorkingTimeVM _workingTime;
        public WorkingTimeVM WorkingTime
        {
            get { return _workingTime; }
            set { _workingTime = value; OnPropertyChanged(); }
        }

        private DepartmentType _selectedDepartment;
        public DepartmentType SelectedDepartment
        {
            get { return _selectedDepartment; }
            set { _selectedDepartment = value; OnPropertyChanged(); }
        }

        public bool IsAvailable { get; set; }

        public EmployeeShiftVM()
        {
            Departments = new ObservableCollection<DepartmentType>(Enum.GetValues(typeof(DepartmentType)).Cast<DepartmentType>());
        }
    }
}