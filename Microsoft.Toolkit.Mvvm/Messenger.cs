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
        // The Messenger class uses the following logic to link stored instances together:
        // --------------------------------------------------------------------------------------------------------
        // DictionarySlim<Recipient, HashSet<IDictionarySlim<Recipient>>> RecipientsMap
        //                    |                  /       \    /
        //                    \_________________/_____    \__/____[*]IDictionarySlim<Recipient, Token>
        //                                     /      \     /                      ______________/
        //                                    /        \   /                      /
        // Container<TMessage, TToken>.DictionarySlim<Recipient, DictionarySlim<TToken, Action<TMessage>>> Values
        // --------------------------------------------------------------------------------------------------------
        // Each combination of <TMessage, TToken> results in a concrete Container<,> type, which holds the
        // mapping to registered recipients to handlers. The handlers are stored in a <TToken, Action<TMessage>>
        // dictionary, so that each recipient can have up to one registered handler for a given token, for each
        // message type. Each existing recipient is also stored in the main recipients map, along with a set of
        // all the existing dictionaries of handlers for that recipient (for all message types and token types).
        // A recipient is stored in the main map as long as it has at least one registered handler in any of the
        // existing mappings for every message/token type combination. The shared map is used to access the set
        // of all registered handlers for a given recipient, without having to know in advance the type of message
        // or token being used for the registration, and without having to use reflection. Note that each dictionary
        // stored in the associated set for each recipient also implements IDictionarySlim<Recipient, Token>, with
        // any token type currently in use by that recipient. This allows to retrieve the type-closed mappings
        // of registered handlers with a given token type, for any message type, for every receiver, again without
        // having to use reflection. This shared map is used to unregister messages from a given recipients either
        // unconditionally, by message type, by token, or for a specific pair of message type and token value.

        /// <summary>
        /// The collection of currently registered recipients, with a link to their linked message receivers.
        /// </summary>
        /// <remarks>
        /// This collection is used to allow reflection-free access to all the existing
        /// registered recipients from <see cref="Unregister(object)"/> and other overloads,
        /// so that all the existing handlers can be removed without having to dynamically create
        /// the generic types for the containers of the various dictionaries mapping the handlers.
        /// </remarks>
        private static readonly DictionarySlim<Recipient, HashSet<IDictionarySlim<Recipient>>> RecipientsMap
            = new DictionarySlim<Recipient, HashSet<IDictionarySlim<Recipient>>>();

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
                ref HashSet<IDictionarySlim<Recipient>> set = ref RecipientsMap.GetOrAddValueRef(key);

                if (set is null)
                {
                    set = new HashSet<IDictionarySlim<Recipient>>();
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
                ref HashSet<IDictionarySlim<Recipient>> set = ref RecipientsMap.GetOrAddValueRef(key);

                if (set is null)
                {
                    return;
                }

                // Removes all the lists of registered handlers for the recipient
                foreach (IDictionarySlim<Recipient> map in set)
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
        /// Unregisters a recipient from messages on a specific channel.
        /// </summary>
        /// <typeparam name="TToken">The type of token to identify what channel to unregister from.</typeparam>
        /// <param name="recipient">The recipient to unregister.</param>
        /// <param name="token">The token to use to identify which handlers to unregister.</param>
        /// <remarks>If the recipient has no registered handler, this method does nothing.</remarks>
        public static void Unregister<TToken>(object recipient, TToken token)
            where TToken : notnull, IEquatable<TToken>
        {
            lock (RecipientsMap)
            {
                // Get the shared set of mappings for the recipient, if present
                var key = new Recipient(recipient);

                if (!RecipientsMap.TryGetValue(key, out HashSet<IDictionarySlim<Recipient>> set))
                {
                    return;
                }

                /* Copy the candidate mappings for the target recipient to a local
                 * array, as we can't modify the contents of the set while iterating it.
                 * The rented buffer is oversized and will also include mappings for
                 * handlers of messages that are registered through a different token. */
                var maps = ArrayPool<IDictionarySlim<Recipient, IDictionarySlim<TToken>>>.Shared.Rent(set.Count);
                int i = 0;

                try
                {
                    // Select the items with the same token type
                    foreach (IDictionarySlim<Recipient> item in set)
                    {
                        if (item is IDictionarySlim<Recipient, IDictionarySlim<TToken>> map)
                        {
                            maps[i++] = map;
                        }
                    }

                    /* Iterate through all the local maps. These are all the currently
                     * existing maps of handlers for messages of any given type, with a token
                     * of the current type, for the target recipient. We heavily rely on
                     * interfaces here to be able to iterate through all the available mappings
                     * without having to know the concrete type in advance, and without having
                     * to deal with reflection: we can just check if the type of the closed interface
                     * matches with the token type currently in use, and operate on those instances. */
                    foreach (IDictionarySlim<Recipient, IDictionarySlim<TToken>> map in maps.AsSpan(0, i))
                    {
                        IDictionarySlim<TToken> holder = map[key];

                        /* Remove the registered handler for the input token,
                         * for the current message type (unknown from here). */
                        holder.Remove(token);

                        if (holder.Count == 0)
                        {
                            // If the map is empty, remove the recipient entirely from its container
                            map.Remove(key);

                            if (map.Count == 0)
                            {
                                /* If no handlers are left at all for the recipient, across all
                                 * message types and token types, remove the set of mappings
                                 * entirely for the current recipient, and lost the strong
                                 * reference to it as well. This is the same situation that
                                 * would've been achieved by just calling Unregister(recipient). */
                                set.Remove(map);

                                if (set.Count == 0)
                                {
                                    RecipientsMap.Remove(key);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    maps.AsSpan(0, i).Clear();

                    ArrayPool<IDictionarySlim<Recipient, IDictionarySlim<TToken>>>.Shared.Return(maps);
                }
            }
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

                if (!values.TryGetValue(key, out DictionarySlim<TToken, Action<TMessage>> map))
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

                    HashSet<IDictionarySlim<Recipient>> set = RecipientsMap[key];

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

                ArrayPool<Action<TMessage>>.Shared.Return(entries);
            }
        }

        /// <summary>
        /// Resets the <see cref="Messenger"/> class and unregisters all the existing recipients.
        /// </summary>
        public static void Reset()
        {
            lock (RecipientsMap)
            {
                foreach (var pair in RecipientsMap)
                {
                    // Clear all the typed maps, as they're assigned to static fields
                    foreach (var map in pair.Value)
                    {
                        map.Clear();
                    }
                }

                // Clear the shared map too
                RecipientsMap.Clear();
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
