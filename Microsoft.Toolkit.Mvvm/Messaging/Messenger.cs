// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Collections.Extensions;

namespace Microsoft.Toolkit.Mvvm.Messaging
{
    /// <summary>
    /// A type that can be used to exchange messages between different objects.
    /// This can be useful to decouple different modules of an application without having to keep strong
    /// references to types being referenced. It is also possible to send messages to specific channels, uniquely
    /// identified by a token, and to have different messengers in different sections of an applications.
    /// In order to use the <see cref="IMessenger"/> functionalities, first define a message type, like so:
    /// <code>
    /// public sealed class LoginCompletedMessage { }
    /// </code>
    /// Then, register your a recipient for this message:
    /// <code>
    /// Messenger.Default.Register&lt;LoginCompletedMessage>(this, m =>
    /// {
    ///     // Handle the message here...
    /// });
    /// </code>
    /// Finally, send a message when needed, like so:
    /// <code>
    /// Messenger.Default.Send&lt;LoginCompletedMessage>();
    /// </code>
    /// For info on the other available features, check the <see cref="IMessenger"/> interface.
    /// </summary>
    public sealed class Messenger : IMessenger
    {
        // The Messenger class uses the following logic to link stored instances together:
        // --------------------------------------------------------------------------------------------------------
        // DictionarySlim<Recipient, HashSet<IMapping>> recipientsMap;
        //                    |                  \             /
        //                    |                   \___________/_____[*]IDictionarySlim<Recipient, IDictionarySlim<TToken>>
        //                    |   ___________________________/             \____                      /
        //                    |  /           ___________________________________\____________________/
        //                    | /           /                                    \
        // DictionarySlim<Recipient, DictionarySlim<TToken, Action<TMessage>>> mapping
        //                                            /               /          /
        //                      ___(Type2.tToken)____/               /          /
        //                     /________________(Type2.tMessage)____/          /
        //                    /       ________________________________________/
        //                   /       /
        // DictionarySlim<Type2, IMapping> typesMap;
        // --------------------------------------------------------------------------------------------------------
        // Each combination of <TMessage, TToken> results in a concrete Mapping<TMessage, TToken> type, which holds
        // the references from registered recipients to handlers. The handlers are stored in a <TToken, Action<TMessage>>
        // dictionary, so that each recipient can have up to one registered handler for a given token, for each
        // message type. Each mapping is stored in the types map, which associates to each pair of concrete types the
        // mapping instances. Each instances is just exposed as an IMapping, as each will be a closed type over
        // a different combination of TMessage and TToken generic type parameters. Each existing recipient is also stored in
        // the main recipients map, along with a set of all the existing dictionaries of handlers for that recipient (for all
        // message types and token types). A recipient is stored in the main map as long as it has at least one
        // registered handler in any of the existing mappings for every message/token type combination.
        // The shared map is used to access the set of all registered handlers for a given recipient, without having
        // to know in advance the type of message or token being used for the registration, and without having to
        // use reflection. This is the same approach used in the types map, just with a more specific exposed reference
        // type as in this case we have at least some partial knowledge of the generic type parameters of the mapped types.
        // Note that each dictionary stored in the associated set for each recipient also implements
        // IDictionarySlim<Recipient, Token>, with any token type currently in use by that recipient. This allows to retrieve
        // the type-closed mappings of registered handlers with a given token type, for an message type, for every receiver,
        // again without having to use reflection. This shared map is used to unregister messages from a given recipients
        // either unconditionally, by message type, by token, or for a specific pair of message type and token value.

        /// <summary>
        /// The collection of currently registered recipients, with a link to their linked message receivers.
        /// </summary>
        /// <remarks>
        /// This collection is used to allow reflection-free access to all the existing
        /// registered recipients from <see cref="Unregister(object)"/> and other overloads,
        /// so that all the existing handlers can be removed without having to dynamically create
        /// the generic types for the containers of the various dictionaries mapping the handlers.
        /// </remarks>
        private readonly DictionarySlim<Recipient, HashSet<IMapping>> recipientsMap = new DictionarySlim<Recipient, HashSet<IMapping>>();

        /// <summary>
        /// The <see cref="Mapping{TMessage,TToken}"/> instance for types combination.
        /// </summary>
        /// <remarks>
        /// The values are just of type <see cref="IDictionarySlim{T}"/> as we don't know the type parameters in advance.
        /// Each method relies on <see cref="GetOrAddMapping{TMessage,TToken}"/> to get the type-safe instance
        /// of the <see cref="Mapping{TMessage,TToken}"/> class for each pair of generic arguments in use.
        /// </remarks>
        private readonly DictionarySlim<Type2, IMapping> typesMap = new DictionarySlim<Type2, IMapping>();

        /// <summary>
        /// Gets the default <see cref="Messenger"/> instance.
        /// </summary>
        public static Messenger Default { get; } = new Messenger();

        /// <inheritdoc/>
        public bool IsRegistered<TMessage>(object recipient)
            where TMessage : class
        {
            return IsRegistered<TMessage, Unit>(recipient, default);
        }

        /// <inheritdoc/>
        [Pure]
        public bool IsRegistered<TMessage, TToken>(object recipient, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            lock (this.recipientsMap)
            {
                if (!TryGetMapping(out Mapping<TMessage, TToken>? mapping))
                {
                    return false;
                }

                var key = new Recipient(recipient);

                return mapping!.ContainsKey(key);
            }
        }

        /// <inheritdoc/>
        public void Register<TMessage>(object recipient, Action<TMessage> action)
            where TMessage : class
        {
            Register(recipient, default(Unit), action);
        }

        /// <inheritdoc/>
        public void Register<TMessage, TToken>(object recipient, TToken token, Action<TMessage> action)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            lock (this.recipientsMap)
            {
                // Get the <TMessage, TToken> registration list for this recipient
                Mapping<TMessage, TToken> values = GetOrAddMapping<TMessage, TToken>();
                var key = new Recipient(recipient);
                ref DictionarySlim<TToken, Action<TMessage>> map = ref values.GetOrAddValueRef(key);

                map ??= new DictionarySlim<TToken, Action<TMessage>>();

                // Add the new registration entry
                ref Action<TMessage> handler = ref map.GetOrAddValueRef(token);

                if (!(handler is null))
                {
                    ThrowInvalidOperationExceptionForDuplicateRegistration();
                }

                handler = action;

                // Make sure this registration map is tracked for the current recipient
                ref HashSet<IMapping> set = ref this.recipientsMap.GetOrAddValueRef(key);

                set ??= new HashSet<IMapping>();

                set.Add(values);
            }
        }

        /// <inheritdoc/>
        public void Unregister(object recipient)
        {
            lock (this.recipientsMap)
            {
                // If the recipient has no registered messages at all, ignore
                var key = new Recipient(recipient);

                if (!this.recipientsMap.TryGetValue(key, out HashSet<IMapping> set))
                {
                    return;
                }

                // Removes all the lists of registered handlers for the recipient
                foreach (IMapping map in set)
                {
                    map.Remove(key);

                    if (map.Count == 0)
                    {
                        /* Maps here are really of type Mapping<,> and with unknown type arguments.
                         * If after removing the current recipient a given map becomes empty, it means
                         * that there are no registered recipients at all for a given pair of message
                         * and token types. In that case, we also remove the map from the types map.
                         * The reason for keeping a key in each mapping is that removing items from a
                         * dictionary (a hashed collection) only costs O(1) in the best case, while
                         * if we had tried to iterate the whole dictionary every time we would have
                         * paid an O(n) minimum cost for each single remove operation. */
                        this.typesMap.Remove(map.TypeArguments);
                    }
                }

                // Remove the associated set in the recipients map
                this.recipientsMap.Remove(key);
            }
        }

        /// <inheritdoc/>
        public void Unregister<TMessage>(object recipient)
            where TMessage : class
        {
            Unregister<TMessage, Unit>(recipient, default);
        }

        /// <inheritdoc/>
        public void Unregister<TToken>(object recipient, TToken token)
            where TToken : IEquatable<TToken>
        {
            lock (this.recipientsMap)
            {
                // Get the shared set of mappings for the recipient, if present
                var key = new Recipient(recipient);

                if (!this.recipientsMap.TryGetValue(key, out HashSet<IMapping> set))
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
                    foreach (IMapping item in set)
                    {
                        /* This is technically a "suspicious cast" as there's no type that inherits
                         * from both IMapping and IDictionarySlim<Recipient, IDictionarySlim<TToken>>.
                         * But this is fine since IMapping instances are really instances of
                         * Mapping<TMessage, TToken>, which in turn inherits from
                         * DictionarySlim<Recipient, DictionarySlim<TToken, Action<TMessage>>>, which
                         * then implements IDictionarySlim<Recipient, IDictionarySlim<TToken>>. */
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
                        /* We don't need whether or not the map contains the recipient, as the
                         * sequence of maps has already been copied from the set containing all
                         * the mappings for the target recipiets: it is guaranteed to be here. */
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
                                set.Remove(Unsafe.As<IMapping>(map));

                                if (set.Count == 0)
                                {
                                    this.recipientsMap.Remove(key);
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

        /// <inheritdoc/>
        public void Unregister<TMessage, TToken>(object recipient, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            lock (this.recipientsMap)
            {
                // Get the registration list, if available
                if (!TryGetMapping(out Mapping<TMessage, TToken>? mapping))
                {
                    return;
                }

                var key = new Recipient(recipient);

                if (!mapping!.TryGetValue(key, out DictionarySlim<TToken, Action<TMessage>> map))
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
                    mapping.Remove(key);

                    HashSet<IMapping> set = this.recipientsMap[key];

                    set.Remove(mapping);

                    if (set.Count == 0)
                    {
                        this.recipientsMap.Remove(key);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Send<TMessage>()
            where TMessage : class, new()
        {
            Send(new TMessage(), default(Unit));
        }

        /// <inheritdoc/>
        public void Send<TMessage>(TMessage message)
            where TMessage : class
        {
            Send(message, default(Unit));
        }

        /// <inheritdoc/>
        public void Send<TMessage, TToken>(TToken token)
            where TMessage : class, new()
            where TToken : IEquatable<TToken>
        {
            Send(new TMessage(), token);
        }

        /// <inheritdoc/>
        public void Send<TMessage, TToken>(TMessage message, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            Action<TMessage>[] entries;
            int i = 0;

            lock (this.recipientsMap)
            {
                /* We need to make a local copy of the currently registered handlers,
                 * since users might try to unregister (or register) new handlers from
                 * inside one of the currently existing handlers. We can use memory pooling
                 * to reuse arrays, to minimize the average memory usage. In practice,
                 * we usually just need to pay the small overhead of copying the items. */
                if (!TryGetMapping(out Mapping<TMessage, TToken>? mapping))
                {
                    return;
                }

                // Count the total number of recipients (including with different tokens)
                foreach (var pair in mapping!)
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
                foreach (var pair in mapping)
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

        /// <inheritdoc/>
        public void Reset()
        {
            lock (this.recipientsMap)
            {
                foreach (var pair in this.recipientsMap)
                {
                    // Clear all the typed maps, as they're assigned to static fields
                    foreach (var map in pair.Value)
                    {
                        map.Clear();
                    }
                }

                // Clear the shared map too
                this.recipientsMap.Clear();
            }
        }

        /// <summary>
        /// Tries to get the <see cref="Mapping{TMessage,TToken}"/> instance of currently registered recipients
        /// for the combination of types <typeparamref name="TMessage"/> and <typeparamref name="TToken"/>.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <param name="mapping">The resulting <see cref="Mapping{TMessage,TToken}"/> instance, if found.</param>
        /// <returns>Whether or not the required <see cref="Mapping{TMessage,TToken}"/> instance was found.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetMapping<TMessage, TToken>(out Mapping<TMessage, TToken>? mapping)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            var key = new Type2(typeof(TMessage), typeof(TToken));

            if (this.typesMap.TryGetValue(key, out IMapping target))
            {
                /* This method and the ones above are the only ones handling values in the types map,
                 * and here we are sure that the object reference we have points to an instance of the
                 * right type. Using an unsafe cast skips two conditional branches and is faster. */
                mapping = Unsafe.As<Mapping<TMessage, TToken>>(target);

                return true;
            }

            mapping = null;

            return false;
        }

        /// <summary>
        /// Gets the <see cref="Mapping{TMessage,TToken}"/> instance of currently registered recipients
        /// for the combination of types <typeparamref name="TMessage"/> and <typeparamref name="TToken"/>.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send.</typeparam>
        /// <typeparam name="TToken">The type of token to identify what channel to use to send the message.</typeparam>
        /// <returns>A <see cref="Mapping{TMessage,TToken}"/> instance with the requested type arguments.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Mapping<TMessage, TToken> GetOrAddMapping<TMessage, TToken>()
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            var key = new Type2(typeof(TMessage), typeof(TToken));
            ref IMapping target = ref this.typesMap.GetOrAddValueRef(key);

            target ??= new Mapping<TMessage, TToken>();

            return Unsafe.As<Mapping<TMessage, TToken>>(target);
        }

        /// <summary>
        /// A mapping type representing a link to recipients and their view of handlers per communication channel.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to receive.</typeparam>
        /// <typeparam name="TToken">The type of token to use to pick the messages to receive.</typeparam>
        /// <remarks>
        /// This type is defined for simplicity and as a workaround for the lack of support for using type aliases
        /// over open generic types in C# (using type aliases can only be used for concrete, closed types).
        /// </remarks>
        private sealed class Mapping<TMessage, TToken> : DictionarySlim<Recipient, DictionarySlim<TToken, Action<TMessage>>>, IMapping
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Mapping{TMessage, TToken}"/> class.
            /// </summary>
            public Mapping()
            {
                TypeArguments = new Type2(typeof(TMessage), typeof(TToken));
            }

            /// <inheritdoc/>
            public Type2 TypeArguments { get; }
        }

        /// <summary>
        /// An interface for the <see cref="Mapping{TMessage,TToken}"/> type which allows to retrieve the type
        /// arguments from a given generic instance without having any prior knowledge about those arguments.
        /// </summary>
        private interface IMapping : IDictionarySlim<Recipient>
        {
            /// <summary>
            /// Gets the <see cref="Type2"/> instance representing the current type arguments.
            /// </summary>
            Type2 TypeArguments { get; }
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
        /// A simple type representing an immutable pair of types.
        /// </summary>
        /// <remarks>
        /// This type replaces a simple <see cref="ValueTuple{T1,T2}"/> as it's faster in its
        /// <see cref="GetHashCode"/> and <see cref="IEquatable{T}.Equals(T)"/> methods, and because
        /// unlike a value tuple it exposes its fields as immutable. Additionally, the
        /// <see cref="tMessage"/> and <see cref="tToken"/> fields provide additional clarity reading
        /// the code compared to <see cref="ValueTuple{T1,T2}.Item1"/> and <see cref="ValueTuple{T1,T2}.Item2"/>.
        /// </remarks>
        public readonly struct Type2 : IEquatable<Type2>
        {
            /// <summary>
            /// The type of registered message.
            /// </summary>
            private readonly Type tMessage;

            /// <summary>
            /// The type of registration token.
            /// </summary>
            private readonly Type tToken;

            /// <summary>
            /// Initializes a new instance of the <see cref="Type2"/> struct.
            /// </summary>
            /// <param name="tMessage">The type of registered message.</param>
            /// <param name="tToken">The type of registration token.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Type2(Type tMessage, Type tToken)
            {
                this.tMessage = tMessage;
                this.tToken = tToken;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Equals(Type2 other)
            {
                return
                    ReferenceEquals(this.tMessage, other.tMessage) &&
                    ReferenceEquals(this.tToken, other.tToken);
            }

            /// <inheritdoc/>
            public override bool Equals(object? obj)
            {
                return obj is Type2 other && Equals(other);
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetHashCode()
            {
                unchecked
                {
                    /* To combine the two hashes, we can simply use the fast djb2 hash algorithm.
                     * This is not a problem in this case since we already know that the base
                     * RuntimeHelpers.GetHashCode method is providing hashes with a good enough distribution. */
                    int hash = RuntimeHelpers.GetHashCode(this.tMessage);

                    hash = (hash << 5) + hash;

                    hash += RuntimeHelpers.GetHashCode(this.tToken);

                    return hash;
                }
            }
        }

        /// <summary>
        /// An empty type representing a generic token with no specific value.
        /// </summary>
        internal struct Unit : IEquatable<Unit>
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
