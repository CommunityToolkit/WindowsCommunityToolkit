// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
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
        /// Registers a recipient for a given type of message.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="action">The <see cref="Action{T}"/> to invoke when a message is received.</param>
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
        public static void Register<TMessage, TToken>(object recipient, TToken token, Action<TMessage> action)
        {
            lock (RecipientsMap)
            {
                // Get the <TMessage, TToken> registration list for this recipient
                var values = Container<TMessage, TToken>.Values;
                var key = new Recipient(recipient);
                ref var list = ref values.GetOrAddValueRef(key);

                if (list is null)
                {
                    list = new List<Entry<TMessage, TToken>>();
                }

                // Add the new registration entry
                list.Add(new Entry<TMessage, TToken>(action, token));

                // Make sure this registration map is tracked for the current recipient
                ref var set = ref RecipientsMap.GetOrAddValueRef(key);

                if (set is null)
                {
                    set = new HashSet<IDictionary<Recipient>>();
                }

                set.Add(values);
            }
        }

        /// <summary>
        /// Unregisters a recipient from messages of a given type.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to stop receiving.</typeparam>
        /// <param name="recipient">The recipient to unregister.</param>
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
        public static void Unregister<TMessage, TToken>(object recipient, TToken token)
        {
            lock (RecipientsMap)
            {
                // Get the registration list (same as above)
                var values = Container<TMessage, TToken>.Values;
                var key = new Recipient(recipient);
                ref var list = ref values.GetOrAddValueRef(key);

                if (list is null)
                {
                    return;
                }

                // Remove all entries with a matching token
                list.RemoveAll(entry => EqualityComparer<TToken>.Default.Equals(entry.Token, token));

                /* If the list is empty, it means that the current recipient has no remaining
                 * registered handlers for the current <TMessage, TToken> combination, regardless,
                 * of the specific token value (ie. the channel used to receive messages of that type).
                 * We can remove the list entirely from this map, and remove the link to the map itself
                 * to the static mapping between existing registered recipients. */
                if (list.Count == 0)
                {
                    values.Remove(key);

                    ref var set = ref RecipientsMap.GetOrAddValueRef(key);

                    set.Remove(values);

                    if (set.Count == 0)
                    {
                        RecipientsMap.Remove(key);
                    }
                }
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
                ref var set = ref RecipientsMap.GetOrAddValueRef(key);

                if (set is null || set.Count == 0)
                {
                    return;
                }

                // Removes all the lists of registered handlers for the recipient
                foreach (var map in set)
                {
                    map.Remove(key);
                }

                // Remove the associated set in the static map
                RecipientsMap.Remove(key);
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
        {
            Entry<TMessage, TToken>[] entries;
            int length = 0;

            lock (RecipientsMap)
            {
                /* We need to make a local copy of the currently registered handlers,
                 * since users might try to unregister (or register) new handlers from
                 * inside one of the currently existing handlers. We can use memory pooling
                 * to reuse arrays, to minimize the average memory usage. In practice,
                 * we usually just need to pay the small overhead of copying the items. */
                var values = Container<TMessage, TToken>.Values;

                // Count the total number of recipients
                foreach (var pair in values)
                {
                    length += pair.Value.Count;
                }

                // Rent the array to copy handlers to
                entries = ArrayPool<Entry<TMessage, TToken>>.Shared.Rent(length);

                /* Copy the handlers to the local collection.
                 * Both types being enumerate expose a struct enumerator,
                 * so we're not actually allocating the enumerator here. */
                int i = 0;
                foreach (var pair in values)
                {
                    foreach (var entry in pair.Value)
                    {
                        entries[i++] = entry;
                    }
                }
            }

            try
            {
                // Invoke all the necessary handlers on the local copy of entries
                foreach (var entry in entries.AsSpan(0, length))
                {
                    if (EqualityComparer<TToken>.Default.Equals(token, entry.Token))
                    {
                        entry.Action(message);
                    }
                }
            }
            finally
            {
                ArrayPool<Entry<TMessage, TToken>>.Shared.Return(entries, true);
            }
        }

        /// <summary>
        /// A container class for registered recipients.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <typeparam name="TToken">The type of token to use to pick the messages to receive.</typeparam>
        private static class Container<TMessage, TToken>
        {
            /// <summary>
            /// The mapping of currently registered recipients for the combination of types
            /// <typeparamref name="TMessage"/> and <typeparamref name="TToken"/>, along with
            /// their registered actions and tokens. Each recipient has an associated <see cref="List{T}"/>
            /// since it could have a number of handlers registered in parallel, each using a different
            /// registration channel specified by the token in use. Using a <see cref="DictionarySlim{TKey,TValue}"/>
            /// for the mapping allows for quick access to all the registered entries for each recipient.
            /// </summary>
            public static readonly DictionarySlim<Recipient, List<Entry<TMessage, TToken>>> Values
                = new DictionarySlim<Recipient, List<Entry<TMessage, TToken>>>();
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
        private struct Unit
        {
        }

        /// <summary>
        /// A type representing a pair of message and token used for registration.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <typeparam name="TToken">The type of token to use to pick the messages to receive.</typeparam>
        private readonly struct Entry<TMessage, TToken>
        {
            /// <summary>
            /// The <see cref="Action{T}"/> to invoke when a message is receive.
            /// </summary>
            public readonly Action<TMessage> Action;

            /// <summary>
            /// The token used for the message registration.
            /// </summary>
            public readonly TToken Token;

            /// <summary>
            /// Initializes a new instance of the <see cref="Entry{TMessage, TToken}"/> struct.
            /// </summary>
            /// <param name="action">The <see cref="Action{T}"/> to invoke when a message is receive.</param>
            /// <param name="token">The token used for the message registration.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Entry(Action<TMessage> action, TToken token)
            {
                this.Action = action;
                this.Token = token;
            }
        }
    }
}
