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

        public async Task AddAvailabilityAsync(Availability availability)
        {
            _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();
        }
    }
}