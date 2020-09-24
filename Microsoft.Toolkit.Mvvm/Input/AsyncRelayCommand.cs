// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Microsoft.Toolkit.Mvvm.Input
{
    /// <summary>
    /// A command that mirrors the functionality of <see cref="RelayCommand"/>, with the addition of
    /// accepting a <see cref="Func{TResult}"/> returning a <see cref="Task"/> as the execute
    /// action, and providing an <see cref="ExecutionTask"/> property that notifies changes when
    /// <see cref="ExecuteAsync"/> is invoked and when the returned <see cref="Task"/> completes.
    /// </summary>
    public sealed class AsyncRelayCommand : ObservableObject, IAsyncRelayCommand
    {
        /// <summary>
        /// The <see cref="Func{TResult}"/> to invoke when <see cref="Execute"/> is used.
        /// </summary>
        private readonly Func<Task>? execute;

        /// <summary>
        /// The cancelable <see cref="Func{T,TResult}"/> to invoke when <see cref="Execute"/> is used.
        /// </summary>
        /// <remarks>Only one between this and <see cref="execute"/> is not <see langword="null"/>.</remarks>
        private readonly Func<CancellationToken, Task>? cancelableExecute;

        /// <summary>
        /// The optional action to invoke when <see cref="CanExecute"/> is used.
        /// </summary>
        private readonly Func<bool>? canExecute;

        /// <summary>
        /// The <see cref="CancellationTokenSource"/> instance to use to cancel <see cref="cancelableExecute"/>.
        /// </summary>
        /// <remarks>This is only used when <see cref="cancelableExecute"/> is not <see langword="null"/>.</remarks>
        private CancellationTokenSource? cancellationTokenSource;

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public AsyncRelayCommand(Func<Task> execute)
        {
            this.execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class that can always execute.
        /// </summary>
        /// <param name="cancelableExecute">The cancelable execution logic.</param>
        public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute)
        {
            this.cancelableExecute = cancelableExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="cancelableExecute">The cancelable execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public AsyncRelayCommand(Func<CancellationToken, Task> cancelableExecute, Func<bool> canExecute)
        {
            this.cancelableExecute = cancelableExecute;
            this.canExecute = canExecute;
        }

        private TaskNotifier? executionTask;

        /// <inheritdoc/>
        public Task? ExecutionTask
        {
            get => this.executionTask;
            private set
            {
                if (SetPropertyAndNotifyOnCompletion(ref this.executionTask, value, _ => OnPropertyChanged(nameof(IsRunning))))
                {
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        /// <inheritdoc/>
        public bool CanBeCanceled => !(this.cancelableExecute is null);

        /// <inheritdoc/>
        public bool IsCancellationRequested => this.cancellationTokenSource?.IsCancellationRequested == true;

        /// <inheritdoc/>
        public bool IsRunning => ExecutionTask?.IsCompleted == false;

        /// <inheritdoc/>
        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(object? parameter)
        {
            return this.canExecute?.Invoke() != false;
        }

        /// <inheritdoc/>
        public void Execute(object? parameter)
        {
            ExecuteAsync(parameter);
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(object? parameter)
        {
            if (CanExecute(parameter))
            {
                // Non cancelable command delegate
                if (!(this.execute is null))
                {
                    return ExecutionTask = this.execute();
                }

                // Cancel the previous operation, if one is pending
                this.cancellationTokenSource?.Cancel();

                var cancellationTokenSource = this.cancellationTokenSource = new CancellationTokenSource();

                OnPropertyChanged(nameof(IsCancellationRequested));

                // Invoke the cancelable command delegate with a new linked token
                return ExecutionTask = this.cancelableExecute!(cancellationTokenSource.Token);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Cancel()
        {
            this.cancellationTokenSource?.Cancel();

            OnPropertyChanged(nameof(IsCancellationRequested));
        }
    }
}