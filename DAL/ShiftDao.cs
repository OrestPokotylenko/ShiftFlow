using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class ShiftDao : BaseDao
    {
        private static Func<ShiftFlowContext, int, DateTime, DateTime, Shift?> CompiledShiftForToday =
            EF.CompileQuery((
                ShiftFlowContext context, int id, DateTime startDate, DateTime endDate) => context
                    .Shifts.AsNoTracking()
                        .FirstOrDefault(s => s.EmployeeId == id && s.StartTime >= startDate && s.StartTime < endDate));

        public Shift? GetShiftForToday(int employeeId, DateTime today)
        {
            DateTime startDate = today.Date;
            DateTime endDate = startDate.AddDays(1);

            return CompiledShiftForToday(_context, employeeId, startDate, endDate);
        }

        private static Func<ShiftFlowContext, int, DateTime, DateTime, IEnumerable<Shift>> CompiledShiftsForWeek =
            EF.CompileQuery((
                ShiftFlowContext context, int id, DateTime weekStart, DateTime weekEnd) => context
                    .Shifts.AsNoTracking()
                        .Where(s => s.EmployeeId == id && s.StartTime.Date >= weekStart.Date && s.StartTime.Date <= weekEnd.Date));

        public List<Shift>? GetShiftsForWeek(Employee employee, DateTime weekStart)
        {
            DateTime weekEnd = weekStart.AddDays(6);
            var shiftsForWeek = new List<Shift>();

            foreach (Shift shift in CompiledShiftsForWeek(_context, employee.EmployeeId, weekStart, weekEnd))
            {
                shiftsForWeek.Add(shift);
            }

            return shiftsForWeek;
        }

        public async Task UpdateShiftAsync(Shift shift)
        {
            _context.Shifts.Update(shift);
            await _context.SaveChangesAsync();
        }

        public async Task AddShiftAsync(List<Shift> shifts)
        {
            _context.Shifts.AddRange(shifts);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShiftAsync(List<Employee> employeesToRemove, DateTime date)
        {
            foreach (Employee employee in employeesToRemove)
            {
                Shift? shift = GetShiftForToday(employee.EmployeeId, date);

                if (shift != null)
                {
                    _context.Shifts.Remove(shift);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}