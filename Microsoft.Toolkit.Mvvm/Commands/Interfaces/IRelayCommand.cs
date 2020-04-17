using System.Windows.Input;

namespace Microsoft.Toolkit.Mvvm.Input
{
    /// <summary>
    /// An interface expanding <see cref="ICommand"/> with the ability to raise
    /// the <see cref="ICommand.CanExecuteChanged"/> event externally.
    /// </summary>
    public interface IRelayCommand : ICommand
    {
        /// <summary>
        /// Raises the <see cref="ICommand.CanExecuteChanged" /> event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}
