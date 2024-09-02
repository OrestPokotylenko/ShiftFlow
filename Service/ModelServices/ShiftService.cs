using DAL;
using Model;

namespace Service.ModelServices
{
    public class ShiftService
    {
        private ShiftDao _shiftDao = new();

        public Shift? GetShiftForToday(int employeeId, DateTime today)
        {
            return _shiftDao.GetShiftForToday(employeeId, today);
        }

        public List<Shift>? GetShiftsForWeek(Employee employee, DateTime weekStart)
        {
            return _shiftDao.GetShiftsForWeek(employee, weekStart);
        }
    }
}