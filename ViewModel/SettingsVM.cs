using Model;
using Service;

namespace ViewModel
{
    public class SettingsVM : BaseVM
    {
        private readonly Settings _settings;
        private readonly SettingsService _settingsService = new();

        public bool EmailNotifications
        {
            get { return _settings.EmailNotifications; }
            set { _settings.EmailNotifications = value; OnPropertyChanged();  UpdateSettings(); }
        }

        public bool PushNotifications
        {
            get { return _settings.PushNotifications; ; }
            set { _settings.PushNotifications = value; OnPropertyChanged(); UpdateSettings(); }
        }

        private void UpdateSettings()
        {
            _settingsService.SaveSettings(_settings);
        }

        public SettingsVM()
        {
            _settings = _settingsService.GetSettings();
        }
    }
}