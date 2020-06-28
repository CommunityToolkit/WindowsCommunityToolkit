// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;

namespace Microsoft.Toolkit.Mvvm.Messaging
{
    /// <summary>
    /// Extensions for the <see cref="IMessenger"/> type.
    /// </summary>
    public static partial class MessengerExtensions
    {
        /// <summary>
        /// Checks whether or not a given recipient has already been registered for a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to check for the given recipient.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to check the registration.</param>
        /// <param name="recipient">The target recipient to check the registration for.</param>
        /// <returns>Whether or not <paramref name="recipient"/> has already been registered for the specified message.</returns>
        /// <remarks>This method will use the default channel to check for the requested registration.</remarks>
        [Pure]
        public static bool IsRegistered<TMessage>(this IMessenger messenger, object recipient)
            where TMessage : class
        {
            return messenger.IsRegistered<TMessage, Unit>(recipient, default);
        }

        /// <summary>
        /// Registers a recipient for a given type of message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to register the recipient.</param>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke when a message is received.</param>
        /// <exception cref="InvalidOperationException">Thrown when trying to register the same message twice.</exception>
        /// <remarks>This method will use the default channel to perform the requested registration.</remarks>
        public static void Register<TMessage>(this IMessenger messenger, object recipient, Action<TMessage> action)
            where TMessage : class
        {
            messenger.Register(recipient, default(Unit), action);
        }

        /// <summary>
        /// Unregisters a recipient from messages of a given type.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to stop receiving.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to unregister the recipient.</param>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <remarks>
        /// This method will unregister the target recipient only from the default channel.
        /// If the recipient has no registered handler, this method does nothing.
        /// </remarks>
        public static void Unregister<TMessage>(this IMessenger messenger, object recipient)
            where TMessage : class
        {
            messenger.Unregister<TMessage, Unit>(recipient, default);
        }

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to send the message.</param>
        /// <returns>The message that has been sent.</returns>
        /// <remarks>
        /// This method is a shorthand for <see cref="Send{TMessage}(IMessenger,TMessage)"/> when the
        /// message type exposes a parameterless constructor: it will automatically create
        /// a new <typeparamref name="TMessage"/> instance and send that to its recipients.
        /// </remarks>
        public static TMessage Send<TMessage>(this IMessenger messenger)
            where TMessage : class, new()
        {
            return messenger.Send(new TMessage(), default(Unit));
        }

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to send the message.</param>
        /// <param name="message">The message to send.</param>
        /// <returns>The message that was sent (ie. <paramref name="message"/>).</returns>
        public static TMessage Send<TMessage>(this IMessenger messenger, TMessage message)
            where TMessage : class
        {
            return messenger.Send(message, default(Unit));
        }

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to send the message.</param>
        /// <param name="token">The token indicating what channel to use.</param>
        /// <returns>The message that has been sen.</returns>
        /// <remarks>
        /// This method will automatically create a new <typeparamref name="TMessage"/> instance
        /// just like <see cref="Send{TMessage}(IMessenger)"/>, and then send it to the right recipients.
        /// </remarks>
        public static TMessage Send<TMessage, TToken>(this IMessenger messenger, TToken token)
            where TMessage : class, new()
            where TToken : IEquatable<TToken>
        {
            return messenger.Send(new TMessage(), token);
        }
    }
}
