using Model;
using Service.ModelServices;
using System.Collections.ObjectModel;
using ViewModel.Utilities;

namespace ViewModel
{
    public class NotificationsVM : BaseVM
    {
        private readonly Employee _employee;
        private readonly RequestService _requestService = new();

        private ObservableCollection<Request>? _notifications;
        public ObservableCollection<Request>? Notifications
        {
            get { return _notifications; }
            set { _notifications = value; OnPropertyChanged(); }
        }

        public ICommandAsync AcceptCommand { get; set; }
        public ICommandAsync DeclineCommand { get; set; }

        public NotificationsVM(Employee employee)
        {
            _employee = employee;
            LoadNotifications();
            LoadCommands();
        }

        private async void LoadNotifications()
        {
            RequestService requestService = new();
            Notifications = new(await requestService.GetReplaceRequests(_employee));
        }

        private void LoadCommands()
        {
            AcceptCommand = new AsyncRelayCommand(AcceptRequestAsync);
            DeclineCommand = new AsyncRelayCommand(DeclineRequestAsync);
        }

        private async Task AcceptRequestAsync(object parameter)
        {
            ShiftService _shiftService = new();
            Request selectedRequest = (Request)parameter;
            selectedRequest.Approved = true;

            Shift selectedShift = selectedRequest.Shift;
            selectedShift.EmployeeId = selectedRequest.EmployeeId;

            Notifications.Remove(selectedRequest);
            _requestService.UpdateRequestAsync(selectedRequest);
            _shiftService.UpdateShiftAsync(selectedShift);
        }

        private async Task DeclineRequestAsync(object parameter)
        {
            Request selectedRequest = (Request)parameter;
            selectedRequest.Approved = false;

            Notifications.Remove(selectedRequest);
            _requestService.UpdateRequestAsync(selectedRequest);
        }
    }
}