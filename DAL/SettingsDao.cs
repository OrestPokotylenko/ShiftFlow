using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class SettingsDao : BaseDao
    {
        public async Task<Settings?> GetSettingsAsync(Employee employee)
        {
            return await _context.Settings.Where(s => s.EmployeeId == employee.EmployeeId).FirstOrDefaultAsync();
        }

        public async Task SaveSettingsAsync(Settings settings)
        {
            _context.Settings.Update(settings);
            await _context.SaveChangesAsync();
        }
    }
}