using Model;

namespace ViewModel
{
    public class CalendarVM : BaseVM
    {
        private readonly Shift shift;

        public DateTime StartTime
        {
            get { return shift.StartTime; }
            set { shift.StartTime = value; OnPropertyChanged(); }
        }

        public DateTime EndTime
        {
            get { return shift.EndTime; }
            set { shift.EndTime = value; OnPropertyChanged(); }
        }

        public TimeSpan BreakDuration
        {
            get { return shift.BreakDuration; }
        }

        public CalendarVM()
        {
            shift = new Shift(1, new DateTime(1991,6,2), new DateTime(1992,6,2), 1);
        }
    }
}
