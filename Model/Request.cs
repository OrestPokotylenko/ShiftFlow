namespace Model
{
    public class Request(int employeeId, RequestType requestType, DateTime requestDate, int? requestId = null, DateTime? startDate = null, DateTime? endDate = null, bool? approved = null)
    {
        public int? RequestId { get; private set; } = requestId;
        public int EmployeeId { get; private set; } = employeeId;
        public virtual Employee? Employee { get; private set; }
        public RequestType RequestType { get; private set; } = requestType;
        public DateTime? StartDate { get; private set; } = startDate;
        public DateTime? EndDate { get; private set; } = endDate;
        public DateTime RequestDate { get; private set; } = requestDate;
        public bool? Approved { get; private set; } = approved;
    }
}