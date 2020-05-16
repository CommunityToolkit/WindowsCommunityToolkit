// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Microsoft.Toolkit.Mvvm.Messaging
{
    /// <summary>
    /// Extensions for the <see cref="IMessenger"/> type.
    /// </summary>
    public static class MessengerExtensions
    {
        /// <summary>
        /// Sends a request of the specified type to all registered recipients, and returns the received response.
        /// </summary>
        /// <typeparam name="TMessage">The type of request message to send.</typeparam>
        /// <typeparam name="TResult">The type of response to expect.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to send the request.</param>
        /// <returns>The <typeparamref name="TResult"/> response value for the given message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no response is received for the message.</exception>
        /// <remarks>
        /// This method is a shorthand for <see cref="Request{TMessage,TResult}(IMessenger,TMessage)"/> when the
        /// message type exposes a parameterless constructor: it will automatically create
        /// a new <typeparamref name="TMessage"/> request message and use that to perform the request.
        /// </remarks>
        public static TResult Request<TMessage, TResult>(this IMessenger messenger)
            where TMessage : RequestMessage<TResult>, new()
        {
            return Request<TMessage, TResult, Messenger.Unit>(messenger, new TMessage(), default);
        }

        /// <summary>
        /// Sends a request of the specified type to all registered recipients, and returns the received response.
        /// </summary>
        /// <typeparam name="TMessage">The type of request message to send.</typeparam>
        /// <typeparam name="TResult">The type of response to expect.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to send the request.</param>
        /// <param name="message">The message to send.</param>
        /// <returns>The <typeparamref name="TResult"/> response value for the given message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no response is received for the message.</exception>
        public static TResult Request<TMessage, TResult>(this IMessenger messenger, TMessage message)
            where TMessage : RequestMessage<TResult>
        {
            return Request<TMessage, TResult, Messenger.Unit>(messenger, message, default);
        }

        /// <summary>
        /// Sends a request of the specified type to all registered recipients, and returns the received response.
        /// </summary>
        /// <typeparam name="TMessage">The type of request message to send.</typeparam>
        /// <typeparam name="TResult">The type of response to expect.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to send the request.</param>
        /// <param name="token">The token indicating what channel to use.</param>
        /// <returns>The <typeparamref name="TResult"/> response value for the given message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no response is received for the message.</exception>
        /// <remarks>
        /// This method will automatically create a new <typeparamref name="TMessage"/> instance
        /// just like <see cref="Request{TMessage,TResult}(IMessenger)"/>, and then send it to the right recipients.
        /// </remarks>
        public static TResult Request<TMessage, TResult, TToken>(this IMessenger messenger, TToken token)
            where TMessage : RequestMessage<TResult>, new()
            where TToken : IEquatable<TToken>
        {
            return Request<TMessage, TResult, TToken>(messenger, new TMessage(), token);
        }

        /// <summary>
        /// Sends a request of the specified type to all registered recipients, and returns the received response.
        /// </summary>
        /// <typeparam name="TMessage">The type of request message to send.</typeparam>
        /// <typeparam name="TResult">The type of response to expect.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to send the request.</param>
        /// <param name="message">The request message to send.</param>
        /// <param name="token">The token indicating what channel to use.</param>
        /// <returns>The <typeparamref name="TResult"/> response value for the given message.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no response is received for the message.</exception>
        public static TResult Request<TMessage, TResult, TToken>(this IMessenger messenger, TMessage message, TToken token)
            where TMessage : RequestMessage<TResult>
            where TToken : IEquatable<TToken>
        {
            messenger.Send(message, token);

            if (!message.IsResponseReceived)
            {
                ThrowInvalidOperationExceptionForNoResponseReceived();
            }

            return message.Result;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when trying to add a duplicate handler.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidOperationExceptionForNoResponseReceived()
        {
            throw new InvalidOperationException("No response was received for the given request message");
        }
    }
}
