// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETSTANDARD2_1

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Collections.Extensions;
using Microsoft.Toolkit.Mvvm.Messaging.Internals;

namespace Microsoft.Toolkit.Mvvm.Messaging
{
    /// <summary>
    /// A class providing a reference implementation for the <see cref="IMessenger"/> interface.
    /// </summary>
    /// <remarks>
    /// This <see cref="IMessenger"/> implementation uses weak references to track the registered
    /// recipients, so it is not necessary to manually unregister them when they're no longer needed.
    /// </remarks>
    public sealed class WeakRefMessenger : IMessenger
    {
        // The WeakRefMessenger class uses the following logic to link stored instances together:
        // --------------------------------------------------------------------------------------------------------
        //                          DictionarySlim<TToken, MessageHandler<TRecipient, TMessage>> mapping
        //                                           /                                   /          /
        //                      ___(Type2.TToken)___/                                   /          /
        //                     /_________________(Type2.TMessage)______________________/          /
        //                    /                                       ___________________________/
        //                   /                                       /
        // DictionarySlim<Type2, ConditionalWeakTable<object, IDictionarySlim>> recipientsMap;
        // --------------------------------------------------------------------------------------------------------
        // Just like in the strong reference variant, each pair of message and token types is used as a key in the
        // recipients map. In this case, the values in the dictionary are ConditionalWeakTable<,> instances, that
        // link each registered recipient to a map of currently registered handlers, through a weak reference.
        // The value in each conditional table is Dictionary<TToken, MessageHandler<TRecipient, TMessage>>, using
        // the same unsafe cast as before to allow the generic handler delegates to be invoked without knowing
        // what type each recipient was registered with, and without the need to use reflection.

        /// <summary>
        /// The map of currently registered recipients for all message types.
        /// </summary>
        private readonly DictionarySlim<Type2, ConditionalWeakTable<object, IDictionarySlim>> recipientsMap
            = new DictionarySlim<Type2, ConditionalWeakTable<object, IDictionarySlim>>();

        /// <summary>
        /// Gets the default <see cref="WeakRefMessenger"/> instance.
        /// </summary>
        public static WeakRefMessenger Default { get; } = new WeakRefMessenger();

        /// <inheritdoc/>
        public bool IsRegistered<TMessage, TToken>(object recipient, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            lock (this.recipientsMap)
            {
                Type2 type2 = new Type2(typeof(TMessage), typeof(TToken));

                // Get the conditional table associated with the target recipient, for the current pair
                // of token and message types. If it exists, check if there is a matching token.
                return
                    this.recipientsMap.TryGetValue(type2, out ConditionalWeakTable<object, IDictionarySlim>? table) &&
                    table!.TryGetValue(recipient, out IDictionarySlim mapping) &&
                    Unsafe.As<IDictionarySlim<TToken>>(mapping).ContainsKey(token);
            }
        }

        /// <inheritdoc/>
        public void Register<TRecipient, TMessage, TToken>(TRecipient recipient, TToken token, MessageHandler<TRecipient, TMessage> handler)
            where TRecipient : class
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            lock (this.recipientsMap)
            {
                Type2 type2 = new Type2(typeof(TMessage), typeof(TToken));

                // Get the conditional table for the pair of type arguments, or create it if it doesn't exist
                ref ConditionalWeakTable<object, IDictionarySlim>? mapping = ref this.recipientsMap.GetOrAddValueRef(type2);

                mapping ??= new ConditionalWeakTable<object, IDictionarySlim>();

                // Get or create the handlers dictionary for the target recipient
                var map = Unsafe.As<DictionarySlim<TToken, object>>(mapping.GetValue(recipient, _ => new DictionarySlim<TToken, object>()));

                // Add the new registration entry
                ref object? registeredHandler = ref map.GetOrAddValueRef(token);

                if (!(registeredHandler is null))
                {
                    ThrowInvalidOperationExceptionForDuplicateRegistration();
                }

                // Store the input handler
                registeredHandler = handler;
            }
        }

        /// <inheritdoc/>
        public void UnregisterAll(object recipient)
        {
            lock (this.recipientsMap)
            {
                var enumerator = this.recipientsMap.GetEnumerator();

                // Traverse all the existing conditional tables and remove all the ones
                // with the target recipient as key. We don't perform a cleanup here,
                // as that is responsability of a separate method defined below.
                while (enumerator.MoveNext())
                {
                    enumerator.Value.Remove(recipient);
                }
            }
        }

        /// <inheritdoc/>
        public void UnregisterAll<TToken>(object recipient, TToken token)
            where TToken : IEquatable<TToken>
        {
            lock (this.recipientsMap)
            {
                var enumerator = this.recipientsMap.GetEnumerator();

                // Same as above, with the difference being that this time we only go through
                // the conditional tables having a matching token type as key, and that we
                // only try to remove handlers with a matching token, if any.
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.TToken == typeof(TToken) &&
                        enumerator.Value.TryGetValue(recipient, out IDictionarySlim mapping))
                    {
                        Unsafe.As<IDictionarySlim<TToken>>(mapping).TryRemove(token, out _);
                    }
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
                var type2 = new Type2(typeof(TMessage), typeof(TToken));
                var enumerator = this.recipientsMap.GetEnumerator();

                // Traverse all the existing token and message pairs matching the current type
                // arguments, and remove all the handlers with a matching token, as above.
                while (enumerator.MoveNext())
                {
                    if (enumerator.Key.Equals(type2) &&
                        enumerator.Value.TryGetValue(recipient, out IDictionarySlim mapping))
                    {
                        Unsafe.As<IDictionarySlim<TToken>>(mapping).TryRemove(token, out _);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public TMessage Send<TMessage, TToken>(TMessage message, TToken token)
            where TMessage : class
            where TToken : IEquatable<TToken>
        {
            ArrayPoolBufferWriter<object> recipients;
            ArrayPoolBufferWriter<object> handlers;

            lock (this.recipientsMap)
            {
                Type2 type2 = new Type2(typeof(TMessage), typeof(TToken));

                // Try to get the target table
                if (!this.recipientsMap.TryGetValue(type2, out ConditionalWeakTable<object, IDictionarySlim>? table))
                {
                    return message;
                }

                recipients = new ArrayPoolBufferWriter<object>();
                handlers = new ArrayPoolBufferWriter<object>();

                // We need a local, temporary copy of all the pending recipients and handlers to
                // invoke, to avoid issues with handlers unregistering from messages while we're
                // holding the lock. To do this, we can just traverse the conditional table in use
                // to enumerate all the existing recipients for the token and message types pair
                // corresponding to the generic arguments for this invocation, and then track the
                // handlers with a matching token, and their corresponding recipients.
                foreach (KeyValuePair<object, IDictionarySlim> pair in table!)
                {
                    var map = Unsafe.As<DictionarySlim<TToken, object>>(pair.Value);

                    if (map.TryGetValue(token, out object? handler))
                    {
                        recipients.Add(pair.Key);
                        handlers.Add(handler!);
                    }
                }
            }

            try
            {
                ReadOnlySpan<object>
                    recipientsSpan = recipients.Span,
                    handlersSpan = handlers.Span;

                for (int i = 0; i < recipientsSpan.Length; i++)
                {
                    // Just like in the other messenger, here we need an unsafe cast to be able to
                    // invoke a generic delegate with a contravariant input argument, with a less
                    // derived reference, without reflection. This is guaranteed to work by how the
                    // messenger tracks registered recipients and their associated handlers, so the
                    // type conversion will always be valid (the recipients are the rigth instances).
                    Unsafe.As<MessageHandler<object, TMessage>>(handlersSpan[i])(recipientsSpan[i], message);
                }
            }
            finally
            {
                recipients.Dispose();
                handlers.Dispose();
            }

            return message;
        }

        /// <inheritdoc/>
        void IMessenger.Cleanup()
        {
            lock (this.recipientsMap)
            {
                using ArrayPoolBufferWriter<Type2> type2s = new ArrayPoolBufferWriter<Type2>();
                using ArrayPoolBufferWriter<object> emptyRecipients = new ArrayPoolBufferWriter<object>();

                var enumerator = this.recipientsMap.GetEnumerator();

                // First, we go through all the currently registered pairs of token and message types.
                // These represents all the combinations of generic arguments with at least one registered
                // handler, with the exception of those with recipients that have already been collected.
                while (enumerator.MoveNext())
                {
                    emptyRecipients.Reset();

                    bool hasAtLeastOneHandler = false;

                    // Go through the currently alive recipients to look for those with no handlers left. We track
                    // the ones we find to remove them outside of the loop (can't modify during enumeration).
                    foreach (KeyValuePair<object, IDictionarySlim> pair in enumerator.Value)
                    {
                        if (pair.Value.Count == 0)
                        {
                            emptyRecipients.Add(pair.Key);
                        }
                        else
                        {
                            hasAtLeastOneHandler = true;
                        }
                    }

                    // Remove the handler maps for recipients that are still alive but with no handlers
                    foreach (object recipient in emptyRecipients.Span)
                    {
                        enumerator.Value.Remove(recipient);
                    }

                    // Track the type combinations with no recipients or handlers left
                    if (!hasAtLeastOneHandler)
                    {
                        type2s.Add(enumerator.Key);
                    }
                }

                // Remove all the mappings with no handlers left
                foreach (Type2 key in type2s.Span)
                {
                    this.recipientsMap.Remove(key);
                }
            }
        }

        /// <inheritdoc/>
        public void Reset()
        {
            lock (this.recipientsMap)
            {
                this.recipientsMap.Clear();
            }
        }

        /// <summary>
        /// A simple buffer writer implementation using pooled arrays.
        /// </summary>
        /// <typeparam name="T">The type of items to store in the list.</typeparam>
        public sealed class ArrayPoolBufferWriter<T> : IDisposable
        {
            /// <summary>
            /// The default buffer size to use to expand empty arrays.
            /// </summary>
            private const int DefaultInitialBufferSize = 128;

            /// <summary>
            /// The underlying <typeparamref name="T"/> array.
            /// </summary>
            private T[] array;

            /// <summary>
            /// The starting offset within <see cref="array"/>.
            /// </summary>
            private int index;

            /// <summary>
            /// Initializes a new instance of the <see cref="ArrayPoolBufferWriter{T}"/> class.
            /// </summary>
            public ArrayPoolBufferWriter()
            {
                this.array = ArrayPool<T>.Shared.Rent(DefaultInitialBufferSize);
                this.index = 0;
            }

            /// <summary>
            /// Gets a <see cref="ReadOnlySpan{T}"/> with the current items.
            /// </summary>
            public ReadOnlySpan<T> Span
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    T[] array = this.array;

                    if (this.index > 0)
                    {
                        return MemoryMarshal.CreateReadOnlySpan(ref array[0], this.index);
                    }

                    return default;
                }
            }

            /// <summary>
            /// Gets the current number of items.
            /// </summary>
            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => this.index;
            }

            /// <summary>
            /// Adds a new item to the current collection.
            /// </summary>
            /// <param name="item">The item to add.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(T item)
            {
                if (this.index == this.array.Length)
                {
                    ResizeBuffer();
                }

                this.array[this.index++] = item;
            }

            /// <summary>
            /// Resets the underlying array and the stored items.
            /// </summary>
            public void Reset()
            {
                Array.Clear(this.array, 0, this.index);

                this.index = 0;
            }

            /// <summary>
            /// Resizes <see cref="array"/> when there is no space left for new items.
            /// </summary>
            [MethodImpl(MethodImplOptions.NoInlining)]
            private void ResizeBuffer()
            {
                T[] rent = ArrayPool<T>.Shared.Rent(this.index << 2);

                Array.Copy(this.array, 0, rent, 0, this.index);
                Array.Clear(this.array, 0, this.index);

                ArrayPool<T>.Shared.Return(this.array);

                this.array = rent;
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                Array.Clear(this.array, 0, this.index);

                ArrayPool<T>.Shared.Return(this.array);
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

#endif
