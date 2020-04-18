// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Microsoft.Toolkit.Mvvm.Input
{
    /// <summary>
    /// A generic command that provides a more specific version of <see cref="AsyncRelayCommand"/>.
    /// </summary>
    /// <typeparam name="T">The type of parameter being passed as input to the callbacks.</typeparam>
    public sealed class AsyncRelayCommand<T> : ObservableObject, IAsyncRelayCommand<T>
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

        /// <inheritdoc/>
        public Task? ExecutionTask
        {
            get => this.executionTask;
            private set => SetAndNotifyOnCompletion(ref executionTask, () => executionTask, value);
        }

        /// <inheritdoc/>
        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(T parameter)
        {
            ExecuteAsync(parameter);
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            ExecuteAsync((T)parameter);
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                return ExecutionTask = this.execute(parameter);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(object parameter)
        {
            return ExecuteAsync((T)parameter);
        }
    }
}