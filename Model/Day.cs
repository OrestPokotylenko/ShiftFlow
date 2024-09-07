namespace Model
{
    public class Day(DateTime day, bool available, Shift? shift = null, Request? vacation = null)
    {
        public DateTime DayValue { get; private set; }  = day;
        public Shift? Shift { get; private set; } = shift;
        public Request? Vacation { get; private set; } = vacation;
        public bool HasShift { get => Shift is not null; }
        public bool HasVacation { get => Vacation is not null; }
        public bool Available { get; private set; } = available;
    }
}