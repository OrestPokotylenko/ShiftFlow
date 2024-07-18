using Model;
using Newtonsoft.Json;

namespace Service
{
    public class SettingsService 
    {
        private string settingsPath;

        public SettingsService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDirectory = Path.Combine(appDataPath, "UI");

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            settingsPath = Path.Combine(appDirectory, "settings.json");
        }

        public Settings GetSettings()
        {
            if (!File.Exists(settingsPath))
            {
                return new Settings();
            }

            var settingsJson = File.ReadAllText(settingsPath);
            return JsonConvert.DeserializeObject<Settings>(settingsJson);
        }

        public void SaveSettings(Settings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(settingsPath, json);
        }
    }
}