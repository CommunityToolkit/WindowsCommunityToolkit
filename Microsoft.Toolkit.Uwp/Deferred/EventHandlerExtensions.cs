// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Deferred
{
    /// <summary>
    /// Extensions to <see cref="EventHandler{TEventArgs}"/> for Deferred Events.
    /// </summary>
    public static class EventHandlerExtensions
    {
        private static readonly Task CompletedTask = Task.FromResult(0);

        /// <summary>
        /// Use to invoke an async <see cref="EventHandler{TEventArgs}"/> using <see cref="DeferredEventArgs"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="EventArgs"/> type.</typeparam>
        /// <param name="eventHandler"><see cref="EventHandler{TEventArgs}"/> to be invoked.</param>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="eventArgs"><see cref="EventArgs"/> instance.</param>
        /// <returns><see cref="Task"/> to wait on deferred event handler.</returns>
        public static Task InvokeAsync<T>(this EventHandler<T> eventHandler, object sender, T eventArgs)
            where T : DeferredEventArgs
        {
            return InvokeAsync(eventHandler, sender, eventArgs, CancellationToken.None);
        }

        /// <summary>
        /// Use to invoke an async <see cref="EventHandler{TEventArgs}"/> using <see cref="DeferredEventArgs"/> with a <see cref="CancellationToken"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="EventArgs"/> type.</typeparam>
        /// <param name="eventHandler"><see cref="EventHandler{TEventArgs}"/> to be invoked.</param>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="eventArgs"><see cref="EventArgs"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> option.</param>
        /// <returns><see cref="Task"/> to wait on deferred event handler.</returns>
        public static Task InvokeAsync<T>(this EventHandler<T> eventHandler, object sender, T eventArgs, CancellationToken cancellationToken)
            where T : DeferredEventArgs
        {
            if (eventHandler == null)
            {
                return CompletedTask;
            }

            var tasks = eventHandler.GetInvocationList()
                .OfType<EventHandler<T>>()
                .Select(invocationDelegate =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    invocationDelegate(sender, eventArgs);

                    var deferral = eventArgs.GetCurrentDeferralAndReset();

                    return deferral?.WaitForCompletion(cancellationToken) ?? CompletedTask;
                })
                .ToArray();

            return Task.WhenAll(tasks);
        }
    }
}
