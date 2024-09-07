using DAL;
using Model;

namespace Service.ModelServices
{
    public class ShiftService
    {
        private ShiftDao _shiftDao = new();

        public Shift? GetShiftForToday(Employee employee, DateTime today)
        {
            return _shiftDao.GetShiftForToday(employee, today);
        }

        public List<Shift>? GetUpcomingShifts(Employee employee, DateTime today)
        {
            return _shiftDao.GetUpcomingShifts(employee, today);
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