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

        public async Task UpdateShiftAsync(Shift shift)
        {
            await _shiftDao.UpdateShiftAsync(shift);
        }

        public async Task AddShiftsAsync(List<Shift> shifts)
        {
            await _shiftDao.AddShiftAsync(shifts);
        }

        public async Task DeleteShiftsAsync(List<Employee> employeesToRemove, DateTime date)
        {
            await _shiftDao.DeleteShiftAsync(employeesToRemove, date);
        }
    }
}