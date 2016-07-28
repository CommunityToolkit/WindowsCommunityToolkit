// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Microsoft.Toolkit.Uwp.Services.Core
{
    /// <summary>
    /// Represents a generic command that can perform a given action.
    /// </summary>
    /// <typeparam name="T">Type associated with the DelegateCommand</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        /// <summary>
        /// Field for commandExecuteAction.
        /// </summary>
        private readonly Action<T> commandExecuteAction;

        /// <summary>
        /// Field for whether the command can execute.
        /// </summary>
        private readonly Func<T, bool> commandCanExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class.
        /// </summary>
        /// <param name="executeAction">
        /// The action to execute when called.
        /// </param>
        /// <param name="canExecute">
        /// The function to call to determine if the command can execute the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the execute action is null.
        /// </exception>
        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecute = null)
        {
            if (executeAction == null)
            {
                throw new ArgumentNullException(nameof(executeAction));
            }

            commandExecuteAction = executeAction;
            commandCanExecute = canExecute ?? (e => true);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        /// The parameter used by the command.
        /// </param>
        /// <returns>
        /// Returns a value indicating whether this command can be executed.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            try
            {
                return commandCanExecute(ConvertParameterValue(parameter));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// The parameter used by the command.
        /// </param>
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            try
            {
                commandExecuteAction(ConvertParameterValue(parameter));
            }
            catch
            {
                Debugger.Break();
            }
        }

        /// <summary>
        /// Fires notification event when the CanExecute property has changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Converts a paramter value to given type.
        /// </summary>
        /// <param name="parameter">Value of the command parameter.</param>
        /// <returns>Strong typed parameter value.</returns>
        private static T ConvertParameterValue(object parameter)
        {
            parameter = parameter is T ? parameter : Convert.ChangeType(parameter, typeof(T));
            return (T)parameter;
        }
    }
}