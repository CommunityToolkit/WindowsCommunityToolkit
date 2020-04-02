﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.Deferred
{
    /// <summary>
    /// Extensions to <see cref="TypedEventHandler{TSender, TResult}"/> for Deferred Events.
    /// </summary>
    public static class TypedEventHandlerExtensions
    {
        /// <summary>
        /// Use to invoke an async <see cref="TypedEventHandler{TSender, TResult}"/> using <see cref="DeferredEventArgs"/>.
        /// </summary>
        /// <typeparam name="S">Type of sender.</typeparam>
        /// <typeparam name="R"><see cref="EventArgs"/> type.</typeparam>
        /// <param name="eventHandler"><see cref="TypedEventHandler{TSender, TResult}"/> to be invoked.</param>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="eventArgs"><see cref="EventArgs"/> instance.</param>
        /// <returns><see cref="Task"/> to wait on deferred event handler.</returns>
        public static Task InvokeAsync<S, R>(this TypedEventHandler<S, R> eventHandler, S sender, R eventArgs)
            where R : DeferredEventArgs
        {
            return InvokeAsync(eventHandler, sender, eventArgs, CancellationToken.None);
        }

        /// <summary>
        /// Use to invoke an async <see cref="TypedEventHandler{TSender, TResult}"/> using <see cref="DeferredEventArgs"/> with a <see cref="CancellationToken"/>.
        /// </summary>
        /// <typeparam name="S">Type of sender.</typeparam>
        /// <typeparam name="R"><see cref="EventArgs"/> type.</typeparam>
        /// <param name="eventHandler"><see cref="TypedEventHandler{TSender, TResult}"/> to be invoked.</param>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="eventArgs"><see cref="EventArgs"/> instance.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> option.</param>
        /// <returns><see cref="Task"/> to wait on deferred event handler.</returns>
        public static Task InvokeAsync<S, R>(this TypedEventHandler<S, R> eventHandler, S sender, R eventArgs, CancellationToken cancellationToken)
            where R : DeferredEventArgs
        {
            if (eventHandler == null)
            {
                return Task.CompletedTask;
            }

            var tasks = eventHandler.GetInvocationList()
                .OfType<TypedEventHandler<S, R>>()
                .Select(invocationDelegate =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    invocationDelegate(sender, eventArgs);

                    var deferral = eventArgs.GetCurrentDeferralAndReset();

                    return deferral?.WaitForCompletion(cancellationToken) ?? Task.CompletedTask;
                })
                .ToArray();

            return Task.WhenAll(tasks);
        }
    }
}
