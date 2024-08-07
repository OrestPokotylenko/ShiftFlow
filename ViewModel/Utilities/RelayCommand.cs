﻿using System.Windows.Input;

namespace ViewModel.Utilities
{
    class RelayCommand(Action<object> execute, Predicate<object>? canExecute = null) : ICommand
    {
        private readonly Action<object> _execute = execute;
        private readonly Predicate<object>? _canExecute = canExecute;
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
    }
}
