// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.Mvvm.Messaging.Internals
{
    /// <summary>
    /// A simple type representing an immutable pair of types.
    /// </summary>
    /// <remarks>
    /// This type replaces a simple <see cref="ValueTuple{T1,T2}"/> as it's faster in its
    /// <see cref="GetHashCode"/> and <see cref="IEquatable{T}.Equals(T)"/> methods, and because
    /// unlike a value tuple it exposes its fields as immutable. Additionally, the
    /// <see cref="TMessage"/> and <see cref="TToken"/> fields provide additional clarity reading
    /// the code compared to <see cref="ValueTuple{T1,T2}.Item1"/> and <see cref="ValueTuple{T1,T2}.Item2"/>.
    /// </remarks>
    internal readonly struct Type2 : IEquatable<Type2>
    {
        /// <summary>
        /// The type of registered message.
        /// </summary>
        public readonly Type TMessage;

        /// <summary>
        /// The type of registration token.
        /// </summary>
        public readonly Type TToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="Type2"/> struct.
        /// </summary>
        /// <param name="tMessage">The type of registered message.</param>
        /// <param name="tToken">The type of registration token.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Type2(Type tMessage, Type tToken)
        {
            TMessage = tMessage;
            TToken = tToken;
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
                TMessage == other.TMessage &&
                TToken == other.TToken;
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
                int hash = RuntimeHelpers.GetHashCode(TMessage);

                hash = (hash << 5) + hash;

                hash += RuntimeHelpers.GetHashCode(TToken);

                return hash;
            }
        }
    }
}
