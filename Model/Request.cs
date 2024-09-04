namespace Model
{
    public class Request(int employeeId, RequestType requestType, DateOnly requestDate, DateTime? startDate = null, DateTime? endDate = null, string? note = null, int? shiftId = null, bool? approved = null, int? requestId = null)
    {
        public int? RequestId { get; private set; } = requestId;
        public int EmployeeId { get; private set; } = employeeId;
        public virtual Employee? Employee { get; private set; }
        public int? ShiftId { get; private set; } = shiftId;
        public virtual Shift? Shift { get; private set; }
        public RequestType RequestType { get; private set; } = requestType;
        public DateTime? StartDate { get; private set; } = startDate;
        public DateTime? EndDate { get; private set; } = endDate;
        public DateOnly RequestDate { get; private set; } = requestDate;
        public string? Note { get; private set; } = note;
        public bool? Approved { get; set; } = approved;
    }
}