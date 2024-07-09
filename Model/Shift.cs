namespace Model
{
    public class Shift(int employeeId, DateTime startTime, DateTime endTime, int? shiftId = null)
    {
        public int? ShiftId { get; private set; } = shiftId;
        public int EmployeeId { get; private set; } = employeeId;
        public virtual Employee? Employee { get; private set; }
        public DateTime StartTime { get; set; } = startTime;
        public DateTime EndTime { get; set; } = endTime;
        public TimeSpan BreakDuration { get => CalculateBreak(); }


        private TimeSpan CalculateBreak()
        {
            (int Threshold, TimeSpan BreakTime)[] Breaks =
            [
                (660, new TimeSpan(1, 15, 0)),
                (540, new TimeSpan(1, 0, 0)),
                (420, new TimeSpan(0, 45, 0)),
                (360, new TimeSpan(0, 30, 0)),
                (270, new TimeSpan(0, 15, 0))
            ];

            TimeSpan timeSpan = EndTime - StartTime;

            foreach (var (threshold, breakTime) in Breaks)
            {
                if (timeSpan.TotalMinutes >= threshold)
                {
                    return breakTime;
                }
            }

            return TimeSpan.Zero;
        }
    }
}
