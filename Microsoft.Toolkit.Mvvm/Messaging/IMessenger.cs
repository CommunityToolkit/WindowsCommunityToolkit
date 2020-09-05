// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;

namespace Microsoft.Toolkit.Mvvm.Messaging
{
    /// <summary>
    /// A <see langword="delegate"/> used to represent actions to invoke when a message is received.
    /// The recipient is given as an input argument to allow message registrations to avoid creating
    /// closures: if an instance method on a recipient needs to be invoked it is possible to just
    /// cast the recipient to the right type and then access the local method from that instance.
    /// </summary>
    /// <typeparam name="TRecipient">The type of recipient for the message.</typeparam>
    /// <typeparam name="TMessage">The type of message to receive.</typeparam>
    /// <param name="recipient">The recipient that is receiving the message.</param>
    /// <param name="message">The message being received.</param>
    public delegate void MessageHandler<in TRecipient, in TMessage>(TRecipient recipient, TMessage message)
        where TRecipient : class
        where TMessage : class;

    /// <summary>
    /// An interface for a type providing the ability to exchange messages between different objects.
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Checks whether or not a given recipient has already been registered for a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to check for the given recipient.</typeparam>
        /// <typeparam name="TToken">The type of token to check the channel for.</typeparam>
        /// <param name="recipient">The target recipient to check the registration for.</param>
        /// <param name="token">The token used to identify the target channel to check.</param>
        /// <returns>Whether or not <paramref name="recipient"/> has already been registered for the specified message.</returns>
        [Pure]
        bool IsRegistered<TMessage, TToken>(object recipient, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>;

        /// <summary>
        /// Registers a recipient for a given type of message.
        /// </summary>
        /// <typeparam name="TRecipient">The type of recipient for the message.</typeparam>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <typeparam name="TToken">The type of token to use to pick the messages to receive.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="token">A token used to determine the receiving channel to use.</param>
        /// <param name="handler">The <see cref="MessageHandler{TRecipient,TMessage}"/> to invoke when a message is received.</param>
        /// <exception cref="InvalidOperationException">Thrown when trying to register the same message twice.</exception>
        void Register<TRecipient, TMessage, TToken>(TRecipient recipient, TToken token, MessageHandler<TRecipient, TMessage> handler)
            where TRecipient : class
            where TMessage : class
            where TToken : IEquatable<TToken>;

        /// <summary>
        /// Unregisters a recipient from all registered messages.
        /// </summary>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <remarks>
        /// This method will unregister the target recipient across all channels.
        /// Use this method as an easy way to lose all references to a target recipient.
        /// If the recipient has no registered handler, this method does nothing.
        /// </remarks>
        void UnregisterAll(object recipient);

        /// <summary>
        /// Unregisters a recipient from all messages on a specific channel.
        /// </summary>
        /// <typeparam name="TToken">The type of token to identify what channel to unregister from.</typeparam>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <param name="token">The token to use to identify which handlers to unregister.</param>
        /// <remarks>If the recipient has no registered handler, this method does nothing.</remarks>
        void UnregisterAll<TToken>(object recipient, TToken token)
            where TToken : IEquatable<TToken>;

        /// <summary>
        /// Unregisters a recipient from messages of a given type.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to stop receiving.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to unregister from.</typeparam>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <param name="token">The token to use to identify which handlers to unregister.</param>
        /// <remarks>If the recipient has no registered handler, this method does nothing.</remarks>
        void Unregister<TMessage, TToken>(object recipient, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>;

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <param name="token">The token indicating what channel to use.</param>
        /// <returns>The message that was sent (ie. <paramref name="message"/>).</returns>
        TMessage Send<TMessage, TToken>(TMessage message, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>;

        /// <summary>
        /// Performs a cleanup on the current messenger.
        /// Invoking this method does not unregister any of the currently registered
        /// recipient, and it can be used to perform cleanup operations such as
        /// trimming the internal data structures of a messenger implementation.
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Resets the <see cref="IMessenger"/> instance and unregisters all the existing recipients.
        /// </summary>
        void Reset();
    }
}
