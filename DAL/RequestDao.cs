namespace DAL
{
    public class RequestDao
    {
        private ShiftFlowContext _context = new();

        public int CountRequests(bool? approve, int employeeId)
        {
            return _context.Requests.Count(r => r.Approved == approve && employeeId == r.EmployeeId);
        }
    }
}