// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Microsoft.Toolkit.Mvvm.Commands
{
    /// <summary>
    /// A generic command that mirrors the functionality of <see cref="RelayCommand"/>, with the addition
    /// of accepting a <see cref="Func{TResult}"/> returning a <see cref="Task{TResult}"/> as the execute
    /// action, and providing a <see cref="ExecutionTask"/> property that notifies changes when
    /// <see cref="Execute(T)"/> is invoked and when the returned <see cref="Task{TResult}"/> completes.
    /// </summary>
    /// <typeparam name="T">The type of parameter being passed as input to the callbacks.</typeparam>
    public sealed class AsyncRelayCommand<T> : ObservableObject, ICommand
    {
        /// <summary>
        /// The <see cref="Func{TResult}"/> to invoke when <see cref="Execute(T)"/> is used.
        /// </summary>
        private readonly Func<T, Task> execute;

        /// <summary>
        /// The optional action to invoke when <see cref="CanExecute(T)"/> is used.
        /// </summary>
        private readonly Func<T, bool>? canExecute;

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public AsyncRelayCommand(Func<T, Task> execute)
        {
            this.execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        private Task? executionTask;

        /// <summary>
        /// Gets the last scheduled <see cref="Task"/>, if available.
        /// This property notifies a change when the <see cref="Task{TResult}"/> completes.Th
        /// </summary>
        public Task? ExecutionTask
        {
            get => this.executionTask;
            private set => SetAndNotifyOnCompletion(() => this.executionTask, value);
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged" /> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Provides a strongly-typed variant of <see cref="CanExecute(object)"/>.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        /// <returns>Whether or not the current command can be executed.</returns>
        /// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(T parameter)
        {
            return this.canExecute?.Invoke(parameter) != false;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        /// <summary>
        /// Provides a strongly-typed variant of <see cref="Execute(object)"/>.
        /// </summary>
        /// <param name="parameter">The input parameter.</param>
        /// <remarks>Use this overload to avoid boxing, if <typeparamref name="T"/> is a value type.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(T parameter)
        {
            if (CanExecute(parameter))
            {
                ExecutionTask = this.execute(parameter);
            }
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            Execute((T)parameter);
        }
    }
}