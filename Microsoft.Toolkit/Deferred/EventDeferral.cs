// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Deferred
{
    /// <summary>
    /// Deferral handle provided by a <see cref="DeferredEventArgs"/>.
    /// </summary>
    public class EventDeferral : IDisposable
    {
        //// TODO: If/when .NET 5 is base, we can upgrade to non-generic version
        private readonly TaskCompletionSource<object?> _taskCompletionSource = new TaskCompletionSource<object?>();
        private bool _disposed = false;

        internal EventDeferral()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="EventDeferral"/> class.
        /// </summary>
        ~EventDeferral() => Dispose(false);

        /// <summary>
        /// Call when finished with the Deferral.
        /// </summary>
        public void Complete() => _taskCompletionSource.TrySetResult(null);

        /// <summary>
        /// Waits for the <see cref="EventDeferral"/> to be completed by the event handler.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns><see cref="Task"/>.</returns>
#if !NETSTANDARD1_4
        [Browsable(false)]
#endif
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This is an internal only method to be used by EventHandler extension classes, public callers should call GetDeferral() instead on the DeferredEventArgs.")]
        public async Task WaitForCompletion(CancellationToken cancellationToken)
        {
            using (cancellationToken.Register(() => _taskCompletionSource.TrySetCanceled()))
            {
                await _taskCompletionSource.Task;
            }
        }

        /// <summary>
        /// Recommended helper method pattern for <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="disposing">Source of dispose request.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            Complete();

            _disposed = true;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}