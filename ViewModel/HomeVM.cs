using Model;
using Service.ModelServices;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ViewModel
{
    public class HomeVM : BaseVM
    {
        private ShiftService _shiftService = new();
        private readonly Employee _employee;

        private Shift? _shiftForToday;
        public Shift? ShiftForToday
        {
            get { return _shiftForToday; }
            set { _shiftForToday = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Shift>? _shiftsForWeek;
        public ObservableCollection<Shift>? ShiftsForWeek
        {
            get { return _shiftsForWeek; }
            set { _shiftsForWeek = value; OnPropertyChanged(); }
        }

        public string TotalTimeWeek
        {
            get { return CalculateTotalTimeWeek(); }
        }

        public int CurrentWeekOfYear
        {
            get { return GetCurrentWeekOfYear(); }
        }

        public string CurrentWeek
        {
            get { return GetCurrentWeekDates(DateTime.Today); }
        }

        private int _pendingRequests;
        public int PendingRequests
        {
            get { return _pendingRequests; }
            set { _pendingRequests = value; OnPropertyChanged(); }
        }

        private int _approvedRequests;
        public int ApprovedRequests
        {
            get { return _approvedRequests; }
            set { _approvedRequests = value; OnPropertyChanged(); }
        }

        private int _rejectedRequests;
        public int RejectedRequests
        {
            get { return _rejectedRequests; }
            set { _rejectedRequests = value; OnPropertyChanged(); }
        }

        private bool _noTodayShift;
        public bool NoTodayShift
        {
            get { return _noTodayShift; }
            set { _noTodayShift = value; OnPropertyChanged(); }
        }

        private bool _noWeekShifts;
        public bool NoWeekShifts
        {
            get { return _noWeekShifts; }
            set { _noWeekShifts = value; OnPropertyChanged(); }
        }

        public string GetCurrentWeekDates(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime startDate = date.Date.AddDays(-diff);
            DateTime endDate = startDate.AddDays(6);

            return $"{startDate:dd MMMM} - {endDate:dd MMMM}";
        }

        private string CalculateTotalTimeWeek()
        {
            TimeSpan totalTime = TimeSpan.Zero;

            foreach (Shift shift in ShiftsForWeek)
            {
                totalTime += shift.EndTime - shift.StartTime;
            }

            return $"{(int)totalTime.TotalHours:D2}:{totalTime.Minutes:D2}";
        }

        public int GetCurrentWeekOfYear()
        {
            DateTime today = DateTime.Today;
            CultureInfo ci = CultureInfo.CurrentCulture;
            CalendarWeekRule rule = ci.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek;

            return ci.Calendar.GetWeekOfYear(today, rule, firstDayOfWeek);
        }

        public HomeVM(Employee employee)
        {
            _employee = employee;
            Load();
        }

        private void Load()
        {
            GetTodayShift();
            GetWeekShifts();
            CountRequestsAsync();
        }

        private void GetTodayShift()
        {
            ShiftForToday = _shiftService.GetShiftForToday(1, DateTime.Now);

            if (ShiftForToday == null)
            {
                NoTodayShift = true;
            }
        }

        private void GetWeekShifts()
        {
            DateTime startDate = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
            ShiftsForWeek = new(_shiftService.GetShiftsForWeek(_employee, startDate));

            if (ShiftsForWeek.Count == 0)
            {
                NoWeekShifts = true;
            }
        }

        private void CountRequestsAsync()
        {
            RequestService requestService = new();
            PendingRequests = requestService.CountRequests(null, _employee);
            ApprovedRequests = requestService.CountRequests(true, _employee);
            RejectedRequests = requestService.CountRequests(false, _employee);
        }
    }
}