using DAL;
using Model;

namespace Service.ModelServices
{
    public class AvailabilityService
    {
        private readonly AvailabilityDao _availabilityDao = new();

        public async Task<List<Availability>?> GetAvailabilitiesByDateAsync(DayOfWeek dayOfWeek)
        {
            return await _availabilityDao.GetAvailabilitiesByDateAsync(dayOfWeek);
        }

        public async Task AddAvailabilityAsync(Availability availability)
        {
            await _availabilityDao.AddAvailabilityAsync(availability);
        }
    }
}