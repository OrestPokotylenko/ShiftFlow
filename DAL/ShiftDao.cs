using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class ShiftDao
    {
        private ShiftFlowContext _context = new();

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
    }
}