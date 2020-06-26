// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Mvvm.Messaging.Messages
{
    /// <summary>
    /// A <see langword="class"/> for request messages that can receive multiple replies, which can either be used directly or through derived classes.
    /// </summary>
    /// <typeparam name="T">The type of request to make.</typeparam>
    public class AsyncCollectionRequestMessage<T> : IAsyncEnumerable<T>
    {
        private readonly ConcurrentBag<Task<T>> responses = new ConcurrentBag<Task<T>>();

        /// <summary>
        /// Gets the message responses.
        /// </summary>
        public IReadOnlyCollection<Task<T>> Responses => this.responses;

        /// <summary>
        /// The <see cref="CancellationTokenSource"/> instance used to link the token passed to
        /// <see cref="GetAsyncEnumerator"/> and the one passed to all subscribers to the message.
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Gets the <see cref="System.Threading.CancellationToken"/> instance that will be linked to the
        /// one used to asynchronously enumerate the received responses. This can be used to cancel asynchronous
        /// replies that are still being processed, if no new items are needed from this request message.
        /// Consider the following example, where we define a message to retrieve the currently opened documents:
        /// <code>
        /// public class OpenDocumentsRequestMessage : AsyncCollectionRequestMessage&lt;XmlDocument&gt; { }
        /// </code>
        /// We can then request and enumerate the results like so:
        /// <code>
        /// await foreach (var document in Messenger.Default.Send&lt;OpenDocumentsRequestMessage&gt;())
        /// {
        ///     // Process each document here...
        /// }
        /// </code>
        /// If we also want to control the cancellation of the token passed to each subscriber to the message,
        /// we can do so by passing a token we control to the returned message before starting the enumeration
        /// (<see cref="TaskAsyncEnumerableExtensions.WithCancellation{T}(IAsyncEnumerable{T},CancellationToken)"/>).
        /// The previous snippet with this additional change looks as follows:
        /// <code>
        /// await foreach (var document in Messenger.Default.Send&lt;OpenDocumentsRequestMessage&gt;().WithCancellation(cts.Token))
        /// {
        ///     // Process each document here...
        /// }
        /// </code>
        /// When no more new items are needed (or for any other reason depending on the situation), the token
        /// passed to the enumerator can be canceled (by calling <see cref="CancellationTokenSource.Cancel()"/>),
        /// and that will also notify the remaining tasks in the request message. The token exposed by the message
        /// itself will automatically be linked and canceled with the one passed to the enumerator.
        /// </summary>
        public CancellationToken CancellationToken => this.cancellationTokenSource.Token;

        /// <summary>
        /// Replies to the current request message.
        /// </summary>
        /// <param name="response">The response to use to reply to the request message.</param>
        public void Reply(T response)
        {
            Reply(Task.FromResult(response));
        }

        /// <summary>
        /// Replies to the current request message.
        /// </summary>
        /// <param name="response">The response to use to reply to the request message.</param>
        public void Reply(Task<T> response)
        {
            this.responses.Add(response);
        }

        /// <inheritdoc/>
        [Pure]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(this.cancellationTokenSource.Cancel);
            }

            foreach (var task in this.responses)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return await task.ConfigureAwait(false);
            }
        }
    }
}
