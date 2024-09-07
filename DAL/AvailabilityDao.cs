using Model;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AvailabilityDao : BaseDao
    {
        public async Task<List<Availability>?> GetAvailabilitiesByDateAsync(DayOfWeek dayOfWeek)
        {
            return await _context.Availabilities.AsNoTracking().Where(a => a.DayOfWeek == dayOfWeek).ToListAsync();
        }

        public async Task<Availability?> GetAvailabilityByEmployeeAndDateAsync(Employee employee, DayOfWeek day)
        {
            return await _context.Availabilities
                .Where(a => a.EmployeeId == employee.EmployeeId && a.DayOfWeek == day).FirstOrDefaultAsync();
        }

        public async Task AddAvailabilityAsync(Availability availability)
        {
            _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsAvailableAsync(Employee employee, DayOfWeek day)
        {
            return await _context.Availabilities.AsNoTracking()
                .AnyAsync(a => a.EmployeeId == employee.EmployeeId && a.DayOfWeek == day);
        }

        public async Task DeleteAvailabilityAsync(Employee employee, DayOfWeek day)
        {
            Availability availability = await GetAvailabilityByEmployeeAndDateAsync(employee, day);
            _context.Availabilities.Remove(availability);
            await _context.SaveChangesAsync();
        }
    }
}