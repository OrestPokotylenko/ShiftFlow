using Model;
using Service.ModelServices;
using System.Collections.ObjectModel;
using ViewModel.Utilities;

namespace ViewModel
{
    public class RequestsVM : BaseVM
    {
        private readonly RequestService _requestService = new();

        private ObservableCollection<Request> _requests;
        public ObservableCollection<Request> Requests
        {
            get { return _requests; }
            set { _requests = value; OnPropertyChanged(); }
        }

        public ICommandAsync AcceptCommand { get; set; }
        public ICommandAsync DeclineCommand { get; set; }

        public RequestsVM()
        {
            LoadRequests();
            LoadCommands();
        }

        private async void LoadRequests()
        {
            Requests = new ObservableCollection<Request>(await _requestService.GetAllRequests());
        }

        private void LoadCommands()
        {
            AcceptCommand = new AsyncRelayCommand(AcceptRequestAsync);
            DeclineCommand = new AsyncRelayCommand(DeclineRequestAsync);
        }

        private async Task AcceptRequestAsync(object parameter)
        {
            Request selectedRequest = (Request)parameter;
            selectedRequest.Approved = true;
            Requests.Remove(selectedRequest);

            await _requestService.UpdateRequestAsync(selectedRequest);
        }

        private async Task DeclineRequestAsync(object parameter)
        {
            Request selectedRequest = (Request)parameter;
            selectedRequest.Approved = false;
            Requests.Remove(selectedRequest);

            await _requestService.UpdateRequestAsync(selectedRequest);
        }
    }
}