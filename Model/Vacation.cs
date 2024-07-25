namespace Model
{
    public class Vacation(int vacationId, int employeeId, DateTime startDate, DateTime endDate, string? note)
    {
        public int VacationId { get; private set; } = vacationId;
        public int EmployeeId { get; private set; } = employeeId;
        public virtual Employee? Employee { get; private set; }
        public DateTime StartDate { get; private set; } = startDate;
        public DateTime EndDate { get; private set; } = endDate;
        public string? Note { get; private set; } = note;
    }
}