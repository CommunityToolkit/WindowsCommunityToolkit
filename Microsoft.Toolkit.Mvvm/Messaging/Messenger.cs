// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
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
    /// Messenger.Default.Register&lt;LoginCompletedMessage&gt;(this, m =>
    /// {
    ///     // Handle the message here...
    /// });
    /// </code>
    /// Finally, send a message when needed, like so:
    /// <code>
    /// Messenger.Default.Send&lt;LoginCompletedMessage&gt;();
    /// </code>
    /// Additionally, the method group syntax can also be used to specify the action
    /// to invoke when receiving a message, if a method with the right signature is available
    /// in the current scope. This is helpful to keep the registration and handling logic separate.
    /// Following up from the previous example, consider a class having this method:
    /// <code>
    /// private void Receive(LoginCompletedMessage message)
    /// {
    ///     // Handle the message there
    /// }
    /// </code>
    /// The registration can then be performed in a single line like so:
    /// <code>
    /// Messenger.Default.Register&lt;LoginCompletedMessage&gt;(this, Receive);
    /// </code>
    /// The C# compiler will automatically convert that expression to an <see cref="Action{T}"/> instance
    /// compatible with the <see cref="MessengerExtensions.Register{T}(IMessenger,object,Action{T})"/> method.
    /// This will also work if multiple overloads of that method are available, each handling a different
    /// message type: the C# compiler will automatically pick the right one for the current message type.
    /// For info on the other available features, check the <see cref="IMessenger"/> interface.
    /// </summary>
    public sealed class Messenger : IMessenger
    {
        // The Messenger class uses the following logic to link stored instances together:
        // --------------------------------------------------------------------------------------------------------
        // DictionarySlim<Recipient, HashSet<IMapping>> recipientsMap;
        //                    |                   \________________[*]IDictionarySlim<Recipient, IDictionarySlim<TToken>>
        //                    |                                            \___          /            /            /
        //                    |   ________(recipients registrations)___________\________/            /          __/
        //                    |  /           _______(channel registrations)_____\___________________/          /
        //                    | /           /                                    \                            /
        // DictionarySlim<Recipient, DictionarySlim<TToken, Action<TMessage>>> mapping = Mapping<TMessage, TToken>
        //                                            /               / \        /                   /
        //                      ___(Type2.tToken)____/               /   \______/___________________/
        //                     /________________(Type2.tMessage)____/          /
        //                    /       ________________________________________/
        //                   /       /
        // DictionarySlim<Type2, IMapping> typesMap;
        // --------------------------------------------------------------------------------------------------------
        // Each combination of <TMessage, TToken> results in a concrete Mapping<TMessage, TToken> type, which holds
        // the references from registered recipients to handlers. The handlers are stored in a <TToken, Action<TMessage>>
        // dictionary, so that each recipient can have up to one registered handler for a given token, for each
        // message type. Each mapping is stored in the types map, which associates each pair of concrete types to its
        // mapping instance. Mapping instances are exposed as IMapping items, as each will be a closed type over
        // a different combination of TMessage and TToken generic type parameters. Each existing recipient is also stored in
        // the main recipients map, along with a set of all the existing dictionaries of handlers for that recipient (for all
        // message types and token types). A recipient is stored in the main map as long as it has at least one
        // registered handler in any of the existing mappings for every message/token type combination.
        // The shared map is used to access the set of all registered handlers for a given recipient, without having
        // to know in advance the type of message or token being used for the registration, and without having to
        // use reflection. This is the same approach used in the types map, as we expose saved items as IMapping values too.
        // Note that each mapping stored in the associated set for each recipient also indirectly implements
        // IDictionarySlim<Recipient, Token>, with any token type currently in use by that recipient. This allows to retrieve
        // the type-closed mappings of registered handlers with a given token type, for any message type, for every receiver,
        // again without having to use reflection. This shared map is used to unregister messages from a given recipients
        // either unconditionally, by message type, by token, or for a specific pair of message type and token value.

        /// <summary>
        /// The collection of currently registered recipients, with a link to their linked message receivers.
        /// </summary>
        /// <remarks>
        /// This collection is used to allow reflection-free access to all the existing
        /// registered recipients from <see cref="UnregisterAll"/> and other methods in this type,
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
        public void Register<TMessage, TToken>(object recipient, TToken token, Action<TMessage> action)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            lock (this.recipientsMap)
            {
                // Get the <TMessage, TToken> registration list for this recipient
                Mapping<TMessage, TToken> mapping = GetOrAddMapping<TMessage, TToken>();
                var key = new Recipient(recipient);
                ref DictionarySlim<TToken, Action<TMessage>>? map = ref mapping.GetOrAddValueRef(key);

                map ??= new DictionarySlim<TToken, Action<TMessage>>();

                // Add the new registration entry
                ref Action<TMessage>? handler = ref map.GetOrAddValueRef(token);

                if (!(handler is null))
                {
                    ThrowInvalidOperationExceptionForDuplicateRegistration();
                }

                handler = action;

                // Update the total counter for handlers for the current type parameters
                mapping.TotalHandlersCount++;

                // Make sure this registration map is tracked for the current recipient
                ref HashSet<IMapping>? set = ref this.recipientsMap.GetOrAddValueRef(key);

                set ??= new HashSet<IMapping>();

                set.Add(mapping);
            }
        }

        /// <inheritdoc/>
        public void UnregisterAll(object recipient)
        {
            lock (this.recipientsMap)
            {
                // If the recipient has no registered messages at all, ignore
                var key = new Recipient(recipient);

                if (!this.recipientsMap.TryGetValue(key, out HashSet<IMapping>? set))
                {
                    return;
                }

                // Removes all the lists of registered handlers for the recipient
                foreach (IMapping mapping in set!)
                {
                    if (mapping.TryRemove(key, out object? handlersMap))
                    {
                        // If this branch is taken, it means the target recipient to unregister
                        // had at least one registered handler for the current <TToken, TMessage>
                        // pair of type parameters, which here is masked out by the IMapping interface.
                        // Before removing the handlers, we need to retrieve the count of how many handlers
                        // are being removed, in order to update the total counter for the mapping.
                        // Just casting the dictionary to the base interface and accessing the Count
                        // property directly gives us O(1) access time to retrieve this count.
                        // The handlers map is the IDictionary<TToken, TMessage> instance for the mapping.
                        int handlersCount = Unsafe.As<IDictionarySlim>(handlersMap).Count;

                        mapping.TotalHandlersCount -= handlersCount;

                        if (mapping.Count == 0)
                        {
                            // Maps here are really of type Mapping<,> and with unknown type arguments.
                            // If after removing the current recipient a given map becomes empty, it means
                            // that there are no registered recipients at all for a given pair of message
                            // and token types. In that case, we also remove the map from the types map.
                            // The reason for keeping a key in each mapping is that removing items from a
                            // dictionary (a hashed collection) only costs O(1) in the best case, while
                            // if we had tried to iterate the whole dictionary every time we would have
                            // paid an O(n) minimum cost for each single remove operation.
                            this.typesMap.Remove(mapping.TypeArguments);
                        }
                    }
                }

                // Remove the associated set in the recipients map
                this.recipientsMap.Remove(key);
            }
        }

        /// <inheritdoc/>
        public void UnregisterAll<TToken>(object recipient, TToken token)
            where TToken : IEquatable<TToken>
        {
            bool lockTaken = false;
            object[]? maps = null;
            int i = 0;

            // We use an explicit try/finally block here instead of the lock syntax so that we can use a single
            // one both to release the lock and to clear the rented buffer and return it to the pool. The reason
            // why we're declaring the buffer here and clearing and returning it in this outer finally block is
            // that doing so doesn't require the lock to be kept, and releasing it before performing this last
            // step reduces the total time spent while the lock is acquired, which in turn reduces the lock
            // contention in multi-threaded scenarios where this method is invoked concurrently.
            try
            {
                Monitor.Enter(this.recipientsMap, ref lockTaken);

                // Get the shared set of mappings for the recipient, if present
                var key = new Recipient(recipient);

                if (!this.recipientsMap.TryGetValue(key, out HashSet<IMapping>? set))
                {
                    return;
                }

                // Copy the candidate mappings for the target recipient to a local array, as we can't modify the
                // contents of the set while iterating it. The rented buffer is oversized and will also include
                // mappings for handlers of messages that are registered through a different token. Note that
                // we're using just an object array to minimize the number of total rented buffers, that would
                // just remain in the shared pool unused, other than when they are rented here. Instead, we're
                // using a type that would possibly also be used by the users of the library, which increases
                // the opportunities to reuse existing buffers for both. When we need to reference an item
                // stored in the buffer with the type we know it will have, we use Unsafe.As<T> to avoid the
                // expensive type check in the cast, since we already know the assignment will be valid.
                maps = ArrayPool<object>.Shared.Rent(set!.Count);

                foreach (IMapping item in set)
                {
                    // Select all mappings using the same token type
                    if (item is IDictionarySlim<Recipient, IDictionarySlim<TToken>> mapping)
                    {
                        maps[i++] = mapping;
                    }
                }

                // Iterate through all the local maps. These are all the currently
                // existing maps of handlers for messages of any given type, with a token
                // of the current type, for the target recipient. We heavily rely on
                // interfaces here to be able to iterate through all the available mappings
                // without having to know the concrete type in advance, and without having
                // to deal with reflection: we can just check if the type of the closed interface
                // matches with the token type currently in use, and operate on those instances.
                foreach (object obj in maps.AsSpan(0, i))
                {
                    var map = Unsafe.As<IDictionarySlim<Recipient, IDictionarySlim<TToken>>>(obj);

                    // We don't need whether or not the map contains the recipient, as the
                    // sequence of maps has already been copied from the set containing all
                    // the mappings for the target recipients: it is guaranteed to be here.
                    IDictionarySlim<TToken> holder = map[key];

                    // Try to remove the registered handler for the input token,
                    // for the current message type (unknown from here).
                    if (holder.Remove(token))
                    {
                        // As above, we need to update the total number of registered handlers for the map.
                        // In this case we also know that the current TToken type parameter is of interest
                        // for the current method, as we're only unsubscribing handlers using that token.
                        // This is because we're already working on the final <TToken, TMessage> mapping,
                        // which associates a single handler with a given token, for a given recipient.
                        // This means that we don't have to retrieve the count to subtract in this case,
                        // we're just removing a single handler at a time. So, we just decrement the total.
                        Unsafe.As<IMapping>(map).TotalHandlersCount--;

                        if (holder.Count == 0)
                        {
                            // If the map is empty, remove the recipient entirely from its container
                            map.Remove(key);

                            if (map.Count == 0)
                            {
                                // If no handlers are left at all for the recipient, across all
                                // message types and token types, remove the set of mappings
                                // entirely for the current recipient, and lost the strong
                                // reference to it as well. This is the same situation that
                                // would've been achieved by just calling UnregisterAll(recipient).
                                set.Remove(Unsafe.As<IMapping>(map));

                                if (set.Count == 0)
                                {
                                    this.recipientsMap.Remove(key);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                // Release the lock, if we did acquire it
                if (lockTaken)
                {
                    Monitor.Exit(this.recipientsMap);
                }

                // If we got to renting the array of maps, return it to the shared pool.
                // Remove references to avoid leaks coming from the shared memory pool.
                // We manually create a span and clear it as a small optimization, as
                // arrays rented from the pool can be larger than the requested size.
                if (!(maps is null))
                {
                    maps.AsSpan(0, i).Clear();

                    ArrayPool<object>.Shared.Return(maps);
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

                if (!mapping!.TryGetValue(key, out DictionarySlim<TToken, Action<TMessage>>? dictionary))
                {
                    return;
                }

                // Remove the target handler
                if (dictionary!.Remove(token))
                {
                    // Decrement the total count, as above
                    mapping.TotalHandlersCount--;

                    // If the map is empty, it means that the current recipient has no remaining
                    // registered handlers for the current <TMessage, TToken> combination, regardless,
                    // of the specific token value (ie. the channel used to receive messages of that type).
                    // We can remove the map entirely from this container, and remove the link to the map itself
                    // to the current mapping between existing registered recipients (or entire recipients too).
                    if (dictionary.Count == 0)
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
        }

        /// <inheritdoc/>
        public TMessage Send<TMessage, TToken>(TMessage message, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            object[] entries;
            int i = 0;

            lock (this.recipientsMap)
            {
                // Check whether there are any registered recipients
                if (!TryGetMapping(out Mapping<TMessage, TToken>? mapping))
                {
                    return message;
                }

                // We need to make a local copy of the currently registered handlers,
                // since users might try to unregister (or register) new handlers from
                // inside one of the currently existing handlers. We can use memory pooling
                // to reuse arrays, to minimize the average memory usage. In practice,
                // we usually just need to pay the small overhead of copying the items.
                entries = ArrayPool<object>.Shared.Rent(mapping!.TotalHandlersCount);

                ref object entriesRef = ref MemoryMarshal.GetReference(entries.AsSpan());

                // Copy the handlers to the local collection.
                // Both types being enumerate expose a struct enumerator,
                // so we're not actually allocating the enumerator here.
                // The array is oversized at this point, since it also includes
                // handlers for different tokens. We can reuse the same variable
                // to count the number of matching handlers to invoke later on.
                // This will be the array slice with valid actions in the rented buffer.
                var mappingEnumerator = mapping.GetEnumerator();

                // Explicit enumerator usage here as we're using a custom one
                // that doesn't expose the single standard Current property.
                while (mappingEnumerator.MoveNext())
                {
                    var pairsEnumerator = mappingEnumerator.Value.GetEnumerator();

                    while (pairsEnumerator.MoveNext())
                    {
                        // Only select the ones with a matching token
                        if (pairsEnumerator.Key.Equals(token))
                        {
                            unsafe
                            {
                                // We spend quite a bit of time in these two busy loops as we go through all the
                                // existing mappings and registrations to find the handlers we're interested in.
                                // We can manually offset here to skip the bounds checks in this inner loop when
                                // indexing the array (the size is already verified and guaranteed to be enough).
                                Unsafe.Add(ref entriesRef, (IntPtr)(void*)(uint)i++) = pairsEnumerator.Value;
                            }
                        }
                    }
                }
            }

            try
            {
                // Invoke all the necessary handlers on the local copy of entries
                foreach (var entry in entries.AsSpan(0, i))
                {
                    // We're doing an unsafe cast to skip the type checks again.
                    // See the comments in the UnregisterAll method for more info.
                    Unsafe.As<Action<TMessage>>(entry)(message);
                }
            }
            finally
            {
                // As before, we also need to clear it first to avoid having potentially long
                // lasting memory leaks due to leftover references being stored in the pool.
                entries.AsSpan(0, i).Clear();

                ArrayPool<object>.Shared.Return(entries);
            }

            return message;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            lock (this.recipientsMap)
            {
                this.recipientsMap.Clear();
                this.typesMap.Clear();
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetMapping<TMessage, TToken>(out Mapping<TMessage, TToken>? mapping)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            var key = new Type2(typeof(TMessage), typeof(TToken));

            if (this.typesMap.TryGetValue(key, out IMapping? target))
            {
                // This method and the ones above are the only ones handling values in the types map,
                // and here we are sure that the object reference we have points to an instance of the
                // right type. Using an unsafe cast skips two conditional branches and is faster.
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
            ref IMapping? target = ref this.typesMap.GetOrAddValueRef(key);

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

            /// <inheritdoc/>
            public int TotalHandlersCount { get; set; }
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

            /// <summary>
            /// Gets or sets the total number of handlers in the current instance.
            /// </summary>
            int TotalHandlersCount { get; set; }
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
        private readonly struct Type2 : IEquatable<Type2>
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
                // We can't just use reference equality, as that's technically not guaranteed
                // to work and might fail in very rare cases (eg. with type forwarding between
                // different assemblies). Instead, we can use the == operator to compare for
                // equality, which still avoids the callvirt overhead of calling Type.Equals,
                // and is also implemented as a JIT intrinsic on runtimes such as .NET Core.
                return
                    this.tMessage == other.tMessage &&
                    this.tToken == other.tToken;
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
                    // To combine the two hashes, we can simply use the fast djb2 hash algorithm.
                    // This is not a problem in this case since we already know that the base
                    // RuntimeHelpers.GetHashCode method is providing hashes with a good enough distribution.
                    int hash = RuntimeHelpers.GetHashCode(this.tMessage);

                    hash = (hash << 5) + hash;

                    hash += RuntimeHelpers.GetHashCode(this.tToken);

                    return hash;
                }
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when trying to add a duplicate handler.
        /// </summary>
        private static void ThrowInvalidOperationExceptionForDuplicateRegistration()
        {
            throw new InvalidOperationException("The target recipient has already subscribed to the target message");
        }
    }
}
