using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class ShiftDao
    {
        private ShiftFlowContext _context = new();

        public async Task<Shift?> GetShiftForTodayAsync(Employee employee, DateTime today)
        {
            return await _context.Shifts
                .Where(s => s.EmployeeId == employee.EmployeeId && s.StartTime.Date == today.Date)
                .FirstOrDefaultAsync();
        }

        public Shift? GetShiftForToday(Employee employee, DateTime today)
        {
            return _context.Shifts
                .Where(s => s.EmployeeId == employee.EmployeeId && s.StartTime.Date == today.Date)
                .FirstOrDefault();
        }

        public async Task<List<Shift>> GetShiftsForWeekAsync(Employee employee, DateTime weekStart)
        {
            DateTime weekEnd = weekStart.AddDays(6);

            return await _context.Shifts
                .Where(s => s.EmployeeId == employee.EmployeeId && s.StartTime.Date >= weekStart.Date && s.StartTime.Date <= weekEnd.Date)
                .AsNoTracking().ToListAsync();
        }
    }
}