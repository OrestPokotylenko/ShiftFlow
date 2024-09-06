namespace ViewModel
{
    public class WorkingTimeVM : BaseVM
    {
        private SchedulesVM _schedulesVM;

        private int? _startHour;
        public int? StartHours
        {
            get { return _startHour; }
            set { _startHour = value; OnPropertyChanged(); UpdateWorkingTime(); }
        }

        private int? _startMinute;
        public int? StartMinutes
        {
            get { return _startMinute; }
            set { _startMinute = value; OnPropertyChanged(); UpdateWorkingTime(); }
        }

        private int? _endHour;
        public int? EndHours
        {
            get { return _endHour; }
            set { _endHour = value; OnPropertyChanged(); UpdateWorkingTime(); }
        }

        private int? _endMinute;
        public int? EndMinutes
        {
            get { return _endMinute; }
            set { _endMinute = value; OnPropertyChanged(); UpdateWorkingTime(); }
        }

        public (DateTime, DateTime) GetWorkingTime(DateTime date)
        {
            DateTime startTime = new(date.Year, date.Month, date.Day, StartHours ?? 0, StartMinutes ?? 0, 0);
            DateTime endTime = new(date.Year, date.Month, date.Day, EndHours ?? 0, EndMinutes ?? 0, 0);

            return (startTime, endTime);
        }

        public WorkingTimeVM(SchedulesVM schedulesVM, DateTime? startTime = null, DateTime? endTime = null)
        {
            _schedulesVM = schedulesVM;
            StartHours = startTime?.Hour;
            StartMinutes = startTime?.Minute;
            EndHours = endTime?.Hour;
            EndMinutes = endTime?.Minute;
        }

        private void UpdateWorkingTime()
        {
            _schedulesVM.SetTotalHours();
        }
    }
}