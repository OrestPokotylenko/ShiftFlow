using Model;
using Service.ModelServices;

namespace ViewModel
{
    public class SettingsVM : BaseVM
    {
        private readonly Employee _employee;
        private Settings? _settings;
        private readonly SettingsService _settingsService = new();

        private bool _emailNotifications;
        public bool EmailNotifications
        {
            get { return _emailNotifications; }
            set { _emailNotifications = value; OnPropertyChanged();  UpdateSettings(); }
        }

        private async void UpdateSettings()
        {
            _settings.EmailNotifications = EmailNotifications;
            await _settingsService.SaveSettingsAsync(_settings);
        }

        public SettingsVM(Employee employee)
        {
            _employee = employee;
            LoadSettings();
        }

        private async void LoadSettings()
        {
            _settings = await _settingsService.GetSettingsAsync(_employee);
            EmailNotifications = _settings.EmailNotifications;
        }
    }
}