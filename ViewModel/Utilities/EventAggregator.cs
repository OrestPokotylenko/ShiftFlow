namespace ViewModel.Utilities
{
    public class EventAggregator
    {
        private static readonly Lazy<EventAggregator> _instance = new(() => new EventAggregator());

        public static EventAggregator Instance => _instance.Value;

        private EventAggregator() { }

        public event Action<string> OnChangeView;

        public void ChangeView(string viewName)
        {
            OnChangeView?.Invoke(viewName);
        }
    }
}