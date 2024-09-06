namespace Model
{
    public class Availability(int employeeId, DayOfWeek dayOfWeek, int? availabilityId = null)
    {
        public int? AvailabilityId { get; private set; } = availabilityId;
        public int EmployeeId { get; private set; } = employeeId;
        public virtual Employee Employee { get; private set; }
        public DayOfWeek DayOfWeek { get; private set; } = dayOfWeek;
    }
}