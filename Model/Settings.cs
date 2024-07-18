namespace Model
{
    public class Settings(bool emailNotifications = true, bool pushNotifiacations =true)
    {
        public bool EmailNotifications { get; set; } = emailNotifications;
        public bool PushNotifications { get; set; } = pushNotifiacations;
    }
}