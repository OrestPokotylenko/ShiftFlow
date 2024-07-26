using System.Windows.Input;

namespace ViewModel.Utilities
{
    public interface ICommandAsync : ICommand
    {
        Task ExecuteAsync(object parameter);
        bool CanExecute(object parameter);
        void RaiseCanExecuteChanged();
    }
}