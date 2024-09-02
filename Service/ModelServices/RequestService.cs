using DAL;
using Model;

namespace Service.ModelServices
{
    public class RequestService
    {
        RequestDao requestDao = new();

        public int CountRequests(bool? approve, Employee employee)
        {
            return requestDao.CountRequests(approve, employee);
        }

        public async Task AddRequestAsync(Request request)
        {
            await requestDao.AddRequestAsync(request);
        }

        public Request? GetVacationForToday(int employeeId, DateTime today)
        {
            return requestDao.GetVacationForToday(employeeId, today);
        }
    }
}