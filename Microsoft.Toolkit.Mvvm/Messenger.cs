// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Collections.Extensions;

namespace Microsoft.Toolkit.Mvvm
{
    /// <summary>
    /// A type implementing a messaging system between objects.
    /// </summary>
    public static class Messenger
    {
        /// <summary>
        /// The collection of currently registered recipients, with a link to their linked message receivers.
        /// </summary>
        /// <remarks>
        /// This collection is used to allow reflection-free access to all the existing
        /// registered recipients from <see cref="Unregister(object)"/>, so that all the
        /// existing handlers can be removed without having to dynamically create the
        /// generic types for the containers of the various dictionaries mapping the handlers.
        /// </remarks>
        private static readonly DictionarySlim<Recipient, HashSet<IDictionary<Recipient>>> RecipientsMap
            = new DictionarySlim<Recipient, HashSet<IDictionary<Recipient>>>();

        /// <summary>
        /// Checks whether or not a given recipient has already been registered for a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to check for the given recipient.</typeparam>
        /// <param name="recipient">The target recipient to check the registration for.</param>
        /// <returns>Whether or not <paramref name="recipient"/> has already been registered for the specified message.</returns>
        [Pure]
        public static bool IsRegistered<TMessage>(object recipient)
        {
            return IsRegistered<TMessage, Unit>(recipient, default);
        }

        /// <summary>
        /// Checks whether or not a given recipient has already been registered for a message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to check for the given recipient.</typeparam>
        /// <typeparam name="TToken">The type of token to check the channel for.</typeparam>
        /// <param name="recipient">The target recipient to check the registration for.</param>
        /// <param name="token">The token used to identify the target channel to check.</param>
        /// <returns>Whether or not <paramref name="recipient"/> has already been registered for the specified message.</returns>
        [Pure]
        public static bool IsRegistered<TMessage, TToken>(object recipient, TToken token)
            where TToken : notnull, IEquatable<TToken>
        {
            lock (RecipientsMap)
            {
                var values = Container<TMessage, TToken>.Values;
                var key = new Recipient(recipient);

                return values.ContainsKey(key);
            }
        }

        /// <summary>
        /// Registers a recipient for a given type of message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke when a message is received.</param>
        /// <exception cref="InvalidOperationException">Thrown when trying to register the same message twice.</exception>
        public static void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            Register(recipient, default(Unit), action);
        }

        /// <summary>
        /// Registers a recipient for a given type of message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <typeparam name="TToken">The type of token to use to pick the messages to receive.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="token">A token used to determine the receiving channel to use.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke when a message is received.</param>
        /// <exception cref="InvalidOperationException">Thrown when trying to register the same message twice.</exception>
        public static void Register<TMessage, TToken>(object recipient, TToken token, Action<TMessage> action)
            where TToken : notnull, IEquatable<TToken>
        {
            lock (RecipientsMap)
            {
                // Get the <TMessage, TToken> registration list for this recipient
                var values = Container<TMessage, TToken>.Values;
                var key = new Recipient(recipient);
                ref DictionarySlim<TToken, Action<TMessage>> map = ref values.GetOrAddValueRef(key);

                if (map is null)
                {
                    map = new DictionarySlim<TToken, Action<TMessage>>();
                }

                // Add the new registration entry
                ref Action<TMessage> handler = ref map.GetOrAddValueRef(token);

                if (!(handler is null))
                {
                    ThrowInvalidOperationExceptionForDuplicateRegistration();
                }

                handler = action;

                // Make sure this registration map is tracked for the current recipient
                ref HashSet<IDictionary<Recipient>> set = ref RecipientsMap.GetOrAddValueRef(key);

                if (set is null)
                {
                    set = new HashSet<IDictionary<Recipient>>();
                }

                set.Add(values);
            }
        }

        /// <summary>
        /// Unregisters a recipient from all registered messages.
        /// </summary>
        /// <param name="recipient">The recipient to unregister.</param>
        public static void Unregister(object recipient)
        {
            lock (RecipientsMap)
            {
                // If the recipient has no registered messages at all, ignore
                var key = new Recipient(recipient);
                ref HashSet<IDictionary<Recipient>> set = ref RecipientsMap.GetOrAddValueRef(key);

                if (set is null)
                {
                    return;
                }

                // Removes all the lists of registered handlers for the recipient
                foreach (IDictionary<Recipient> map in set)
                {
                    map.Remove(key);
                }

                // Remove the associated set in the static map
                RecipientsMap.Remove(key);
            }
        }

        /// <summary>
        /// Unregisters a recipient from messages of a given type.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to stop receiving.</typeparam>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <remarks>If the recipient has no registered handler, this method does nothing.</remarks>
        public static void Unregister<TMessage>(object recipient)
        {
            Unregister<TMessage, Unit>(recipient, default);
        }

        /// <summary>
        /// Unregisters a recipient from messages of a given type.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to stop receiving.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to unregister from.</typeparam>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <param name="token">The token to use to identify which handlers to unregister.</param>
        /// <remarks>If the recipient has no registered handler, this method does nothing.</remarks>
        public static void Unregister<TMessage, TToken>(object recipient, TToken token)
            where TToken : notnull, IEquatable<TToken>
        {
            lock (RecipientsMap)
            {
                // Get the registration list (same as above)
                var values = Container<TMessage, TToken>.Values;
                var key = new Recipient(recipient);
                ref DictionarySlim<TToken, Action<TMessage>> map = ref values.GetOrAddValueRef(key);

                if (map is null)
                {
                    return;
                }

                // Remove the target handler
                map.Remove(token);

                /* If the map is empty, it means that the current recipient has no remaining
                 * registered handlers for the current <TMessage, TToken> combination, regardless,
                 * of the specific token value (ie. the channel used to receive messages of that type).
                 * We can remove the map entirely from this container, and remove the link to the map itself
                 * to the static mapping between existing registered recipients. */
                if (map.Count == 0)
                {
                    values.Remove(key);

                    ref HashSet<IDictionary<Recipient>> set = ref RecipientsMap.GetOrAddValueRef(key);

                    set.Remove(values);

                    if (set.Count == 0)
                    {
                        RecipientsMap.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <param name="message">The message to send.</param>
        public static void Send<TMessage>(TMessage message)
        {
            Send(message, default(Unit));
        }

        /// <summary>
        /// Sends a message of the specified type to all registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <param name="token">The token indicating what channel to use.</param>
        public static void Send<TMessage, TToken>(TMessage message, TToken token)
            where TToken : notnull, IEquatable<TToken>
        {
            Action<TMessage>[] entries;
            int i = 0;

            lock (RecipientsMap)
            {
                /* We need to make a local copy of the currently registered handlers,
                 * since users might try to unregister (or register) new handlers from
                 * inside one of the currently existing handlers. We can use memory pooling
                 * to reuse arrays, to minimize the average memory usage. In practice,
                 * we usually just need to pay the small overhead of copying the items. */
                var values = Container<TMessage, TToken>.Values;

                // Count the total number of recipients (including with different tokens)
                foreach (var pair in values)
                {
                    i += pair.Value.Count;
                }

                // Rent the array to copy handlers to
                entries = ArrayPool<Action<TMessage>>.Shared.Rent(i);

                /* Copy the handlers to the local collection.
                 * Both types being enumerate expose a struct enumerator,
                 * so we're not actually allocating the enumerator here.
                 * The array is oversized at this point, since it also includes
                 * handlers for different tokens. We can reuse the same variable
                 * to count the number of matching handlers to invoke later on.
                 * This will be the array slice with valid actions in the rented buffer. */
                i = 0;
                foreach (var pair in values)
                {
                    foreach (var entry in pair.Value)
                    {
                        // Only select the ones with a matching token
                        if (EqualityComparer<TToken>.Default.Equals(entry.Key, token))
                        {
                            entries[i++] = entry.Value;
                        }
                    }
                }
            }

            try
            {
                // Invoke all the necessary handlers on the local copy of entries
                foreach (var entry in entries.AsSpan(0, i))
                {
                    entry(message);
                }
            }
            finally
            {
                // Remove references to avoid leaks coming from the shader memory pool
                entries.AsSpan(0, i).Clear();

                ArrayPool<Action<TMessage>>.Shared.Return(entries, true);
            }
        }

        /// <summary>
        /// A container class for registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <typeparam name="TToken">The type of token to use to pick the messages to receive.</typeparam>
        private static class Container<TMessage, TToken>
            where TToken : notnull, IEquatable<TToken>
        {
            /// <summary>
            /// The mapping of currently registered recipients for the combination of types
            /// <typeparamref name="TMessage"/> and <typeparamref name="TToken"/>, along with
            /// their registered actions and tokens. Each recipient has an associated <see cref="List{T}"/>
            /// since it could have a number of handlers registered in parallel, each using a different
            /// registration channel specified by the token in use. Using a <see cref="DictionarySlim{TKey,TValue}"/>
            /// for the mapping allows for quick access to all the registered entries for each recipient.
            /// </summary>
            public static readonly DictionarySlim<Recipient, DictionarySlim<TToken, Action<TMessage>>> Values
                = new DictionarySlim<Recipient, DictionarySlim<TToken, Action<TMessage>>>();
        }

        /// <summary>
        /// A simple type representing a recipient.
        /// </summary>
        /// <remarks>
        /// This type is used to enable fast indexing in each mapping dictionary,
        /// since it acts as an external override for the <see cref="GetHashCode"/> and
        /// <see cref="Equals(object?)"/> methods for arbitrary objects, removing both
        /// the virtual call and preventing instances overriding those methods in this context.
        /// Using this type guarantees that all the equality operations are always only done
        /// based on reference equality for each registered recipient, regardless of its type.
        /// </remarks>
        private readonly struct Recipient : IEquatable<Recipient>
        {
            /// <summary>
            /// The registered recipient.
            /// </summary>
            private readonly object target;

            /// <summary>
            /// Initializes a new instance of the <see cref="Recipient"/> struct.
            /// </summary>
            /// <param name="target">The target recipient instance.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Recipient(object target)
            {
                this.target = target;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Equals(Recipient other)
            {
                return ReferenceEquals(this.target, other.target);
            }

            /// <inheritdoc/>
            public override bool Equals(object? obj)
            {
                return obj is Recipient other && Equals(other);
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetHashCode()
            {
                return RuntimeHelpers.GetHashCode(this.target);
            }
        }

        /// <summary>
        /// An empty type representing a generic token with no specific value.
        /// </summary>
        private struct Unit : IEquatable<Unit>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Equals(Unit other)
            {
                return true;
            }

            /// <inheritdoc/>
            public override bool Equals(object? obj)
            {
                return obj is Unit;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetHashCode()
            {
                return 0;
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when trying to add a duplicate handler.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidOperationExceptionForDuplicateRegistration()
        {
            throw new InvalidOperationException("The target recipient has already subscribed to the target message");
        }
    }
}
