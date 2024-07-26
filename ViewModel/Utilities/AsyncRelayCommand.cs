namespace ViewModel.Utilities
{
    class AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null) : ICommandAsync
    {
        private readonly Func<object, Task> _execute = execute;
        private readonly Func<object, bool> _canExecute = canExecute;
        private bool _isExecuting;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute(parameter));
        }

        public async void Execute(object parameter) => await ExecuteAsync(parameter);

        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}