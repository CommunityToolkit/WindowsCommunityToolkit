// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_AsyncRelayCommand
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public async Task Test_AsyncRelayCommand_AlwaysEnabled()
        {
            int ticks = 0;

            var command = new AsyncRelayCommand(async () =>
            {
                await Task.Delay(1000);
                ticks++;
                await Task.Delay(1000);
            });

            Assert.IsTrue(command.CanExecute(null));
            Assert.IsTrue(command.CanExecute(new object()));

            Assert.IsFalse(command.CanBeCanceled);
            Assert.IsFalse(command.IsCancellationRequested);

            (object, EventArgs) args = default;

            command.CanExecuteChanged += (s, e) => args = (s, e);

            command.NotifyCanExecuteChanged();

            Assert.AreSame(args.Item1, command);
            Assert.AreSame(args.Item2, EventArgs.Empty);

            Assert.IsNull(command.ExecutionTask);
            Assert.IsFalse(command.IsRunning);

            Task task = command.ExecuteAsync(null);

            Assert.IsNotNull(command.ExecutionTask);
            Assert.AreSame(command.ExecutionTask, task);
            Assert.IsTrue(command.IsRunning);

            await task;

            Assert.IsFalse(command.IsRunning);

            Assert.AreEqual(ticks, 1);

            command.Execute(new object());

            await command.ExecutionTask!;

            Assert.AreEqual(ticks, 2);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_AsyncRelayCommand_WithCanExecuteFunctionTrue()
        {
            int ticks = 0;

            var command = new AsyncRelayCommand(
                () =>
            {
                ticks++;
                return Task.CompletedTask;
            }, () => true);

            Assert.IsTrue(command.CanExecute(null));
            Assert.IsTrue(command.CanExecute(new object()));

            Assert.IsFalse(command.CanBeCanceled);
            Assert.IsFalse(command.IsCancellationRequested);

            command.Execute(null);

            Assert.AreEqual(ticks, 1);

            command.Execute(new object());

            Assert.AreEqual(ticks, 2);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_AsyncRelayCommand_WithCanExecuteFunctionFalse()
        {
            int ticks = 0;

            var command = new AsyncRelayCommand(
                () =>
            {
                ticks++;
                return Task.CompletedTask;
            }, () => false);

            Assert.IsFalse(command.CanExecute(null));
            Assert.IsFalse(command.CanExecute(new object()));

            Assert.IsFalse(command.CanBeCanceled);
            Assert.IsFalse(command.IsCancellationRequested);

            command.Execute(null);

            Assert.AreEqual(ticks, 0);

            command.Execute(new object());

            Assert.AreEqual(ticks, 0);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public async Task Test_AsyncRelayCommand_WithCancellation()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            var command = new AsyncRelayCommand(token => tcs.Task);

            Assert.IsTrue(command.CanExecute(null));
            Assert.IsTrue(command.CanExecute(new object()));

            Assert.IsTrue(command.CanBeCanceled);
            Assert.IsFalse(command.IsCancellationRequested);

            command.Execute(null);

            Assert.IsFalse(command.IsCancellationRequested);

            command.Cancel();

            Assert.IsTrue(command.IsCancellationRequested);

            tcs.SetResult(null);

            await command.ExecutionTask!;

            Assert.IsTrue(command.IsCancellationRequested);
        }
    }
}
