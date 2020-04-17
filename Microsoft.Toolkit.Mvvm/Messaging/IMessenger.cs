// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;

namespace Microsoft.Toolkit.Mvvm.Messaging
{
    /// <summary>
    /// An interface for a type providing the ability to exchange messages between different objects.
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Checks whether or not a given recipient has already been registered for a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to check for the given recipient.</typeparam>
        /// <param name="recipient">The target recipient to check the registration for.</param>
        /// <returns>Whether or not <paramref name="recipient"/> has already been registered for the specified message.</returns>
        [Pure]
        bool IsRegistered<TMessage>(object recipient)
            where TMessage : class;

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
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke when a message is received.</param>
        /// <exception cref="InvalidOperationException">Thrown when trying to register the same message twice.</exception>
        void Register<TMessage>(object recipient, Action<TMessage> action)
            where TMessage : class;

        /// <summary>
        /// Registers a recipient for a given type of message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <typeparam name="TToken">The type of token to use to pick the messages to receive.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="token">A token used to determine the receiving channel to use.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke when a message is received.</param>
        /// <exception cref="InvalidOperationException">Thrown when trying to register the same message twice.</exception>
        void Register<TMessage, TToken>(object recipient, TToken token, Action<TMessage> action)
            where TMessage : class
            where TToken : IEquatable<TToken>;

        /// <summary>
        /// Unregisters a recipient from all registered messages.
        /// </summary>
        /// <param name="recipient">The recipient to unregister.</param>
        void Unregister(object recipient);

        /// <summary>
        /// Unregisters a recipient from messages of a given type.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to stop receiving.</typeparam>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <remarks>If the recipient has no registered handler, this method does nothing.</remarks>
        void Unregister<TMessage>(object recipient)
            where TMessage : class;

        /// <summary>
        /// Unregisters a recipient from messages on a specific channel.
        /// </summary>
        /// <typeparam name="TToken">The type of token to identify what channel to unregister from.</typeparam>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <param name="token">The token to use to identify which handlers to unregister.</param>
        /// <remarks>If the recipient has no registered handler, this method does nothing.</remarks>
        void Unregister<TToken>(object recipient, TToken token)
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
        /// <remarks>
        /// This method is a shorthand for <see cref="Send{TMessage}(TMessage)"/> when the
        /// message type exposes a parameterless constructor: it will automatically create
        /// a new <typeparamref name="TMessage"/> instance and send that to its recipients.
        /// </remarks>
        void Send<TMessage>()
            where TMessage : class, new();

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <param name="message">The message to send.</param>
        void Send<TMessage>(TMessage message)
            where TMessage : class;

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <param name="token">The token indicating what channel to use.</param>
        /// <remarks>
        /// This method will automatically create a new <typeparamref name="TMessage"/> instance
        /// just like <see cref="Send{TMessage}()"/>, and then send it to the right recipients.
        /// </remarks>
        void Send<TMessage, TToken>(TToken token)
            where TMessage : class, new()
            where TToken : IEquatable<TToken>;

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <param name="token">The token indicating what channel to use.</param>
        void Send<TMessage, TToken>(TMessage message, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>;

        /// <summary>
        /// Resets the <see cref="Messenger"/> class and unregisters all the existing recipients.
        /// </summary>
        void Reset();
    }
}
