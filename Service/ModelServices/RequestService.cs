using DAL;

namespace Service.ModelServices
{
    public class RequestService
    {
        RequestDao requestDao = new();

        public int CountRequests(bool? approve, int employeeId)
        {
            return requestDao.CountRequests(approve, employeeId);
        }
    }
}