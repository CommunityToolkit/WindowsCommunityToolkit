using System;
using System.Windows.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Represents a command that can perform a given action.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private Action action;
        private Action<object> actionT;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        public DelegateCommand(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        public DelegateCommand(Action<object> action)
        {
            this.actionT = action;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to a null reference</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Event handler to check if the command could be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to a null reference</param>
        public void Execute(object parameter)
        {
            if (action != null)
            {
                action();
            }

            if (actionT != null)
            {
                actionT.Invoke(parameter);
            }
        }
    }
}