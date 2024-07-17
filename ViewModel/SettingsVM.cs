namespace ViewModel
{
    public class SettingsVM : BaseVM
    {
        private bool _emailNotifications;
        public bool EmailNotifications
        {
            get { return _emailNotifications; }
            set { _emailNotifications = value; OnPropertyChanged(); }
        }

        private bool _pushNotifications;
        public bool PushNotifications
        {
            get { return _pushNotifications; }
            set { _pushNotifications = value; OnPropertyChanged(); }
        }
    }
}