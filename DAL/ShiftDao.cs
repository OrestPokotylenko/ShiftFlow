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

        public Shift? GetShiftForToday(Employee employee, DateTime today)
        {
            DateTime startDate = today.Date;
            DateTime endDate = startDate.AddDays(1);

            return CompiledShiftForToday(_context, employee.EmployeeId, startDate, endDate);
        }

        private static Func<ShiftFlowContext, int, DateTime, IEnumerable<Shift>> CompiledShiftsForWeek =
            EF.CompileQuery((
                ShiftFlowContext context, int id, DateTime today) => context
                    .Shifts.AsNoTracking()
                        .Where(s => s.EmployeeId == id && s.StartTime.Date > today.Date));

        public List<Shift>? GetUpcomingShifts(Employee employee, DateTime today)
        {
            var shiftsForWeek = new List<Shift>();

            foreach (Shift shift in CompiledShiftsForWeek(_context, employee.EmployeeId, today))
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
                Shift? shift = GetShiftForToday(employee, date);

                if (shift != null)
                {
                    _context.Shifts.Remove(shift);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}