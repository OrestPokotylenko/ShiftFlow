using DAL;
using Model;

namespace Service.ModelServices
{
    public class SettingsService
    {
        private readonly SettingsDao _settingsDao = new();

        public async Task SaveSettingsAsync(Settings settings)
        {
            await _settingsDao.SaveSettingsAsync(settings);
        }

        public async Task<Settings?> GetSettingsAsync(Employee employee)
        {
            Settings? settings = await _settingsDao.GetSettingsAsync(employee);

            if (settings is null)
            {
                settings = new Settings(employee.EmployeeId);
                await SaveSettingsAsync(settings);
            }

            return settings;
        }
    }
}