namespace Model
{
    public class Settings(int employeeId, bool emailNotifications = true, int? settingsId = null)
    {
        public int? SettingsId { get; private set; } = settingsId;
        public int EmployeeId { get; private set; } = employeeId;
        public virtual Employee Employee { get; private set; }
        public bool EmailNotifications { get; set; } = emailNotifications;
    }
}