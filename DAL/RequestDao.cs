using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class RequestDao : BaseDao
    {
        public int CountRequests(bool? approve, Employee employee)
        {
            return _context.Requests.Count(r => r.Approved == approve &&
            employee.EmployeeId == r.EmployeeId &&
            r.RequestType != RequestType.Replace);
        }

        public async Task AddRequestAsync(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
        }

        public Request? GetVacationForToday(int employeeId, DateTime today)
        {
            return _context.Requests
                .FirstOrDefault(r => r.EmployeeId == employeeId && r.StartDate <= today && r.EndDate >= today && 
                    (r.RequestType == RequestType.DayOff || r.RequestType == RequestType.Vacation) &&
                    r.Approved == true);
        }

        public async Task<List<Request>?> GetReplaceRequests(Employee employee)
        {
            return await _context.Requests
                .Where(
                r => r.EmployeeId == employee.EmployeeId && 
                r.Approved == null && 
                r.RequestType == RequestType.Replace).ToListAsync();
        }

        public async Task UpdateRequestAsync(Request request)
        {
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Request>?> GetAllRequests()
        {
            return await _context.Requests.Where(r => r.Approved == null && r.RequestType != RequestType.Replace).ToListAsync();
        }
    }
}