// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="SpinLock"/> type.
    /// </summary>
    public static class SpinLockExtensions
    {
#if NETSTANDARD2_0
        /// <summary>
        /// Enters a specified <see cref="SpinLock"/> instance and returns a wrapper to use to release the lock.
        /// This extension should be used though a <see langword="using"/> block or statement:
        /// <code>
        /// SpinLock spinLock = new SpinLock();
        ///
        /// using (SpinLockExtensions.Enter(&amp;spinLock))
        /// {
        ///     // Thread-safe code here...
        /// }
        /// </code>
        /// The compiler will take care of releasing the SpinLock when the code goes out of that <see langword="using"/> scope.
        /// </summary>
        /// <param name="spinLock">A pointer to the target <see cref="SpinLock"/> to use</param>
        /// <returns>A wrapper type that will release <paramref name="spinLock"/> when its <see cref="System.IDisposable.Dispose"/> method is called.</returns>
        /// <remarks>The returned <see cref="__Lock"/> value shouldn't be used directly: use this extension in a <see langword="using"/> block or statement.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe __Lock Enter(SpinLock* spinLock)
        {
            return new __Lock(spinLock);
        }

        /// <summary>
        /// A <see langword="struct"/> that is used to enter and hold a <see cref="SpinLock"/> through a <see langword="using"/> block or statement.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300", Justification = "The type is not meant to be used directly by users")]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
        public unsafe ref struct __Lock
        {
            /// <summary>
            /// The <see cref="SpinLock"/>* pointer to the target <see cref="SpinLock"/> value to use.
            /// </summary>
            private readonly SpinLock* ptr;

            /// <summary>
            /// A value indicating whether or not the lock is taken by this <see cref="__Lock"/> instance.
            /// </summary>
            private readonly bool lockTaken;

            /// <summary>
            /// Initializes a new instance of the <see cref="__Lock"/> struct.
            /// </summary>
            /// <param name="spinLock">The target <see cref="SpinLock"/> to use.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __Lock(SpinLock* spinLock)
            {
                ptr = spinLock;
                lockTaken = false;

                spinLock->Enter(ref lockTaken);
            }

            /// <summary>
            /// Implements the duck-typed <see cref="System.IDisposable.Dispose"/> method and releases the current <see cref="SpinLock"/> instance.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                if (lockTaken)
                {
                    ptr->Exit();
                }
            }
        }
#else
        /// <summary>
        /// Enters a specified <see cref="SpinLock"/> instance and returns a wrapper to use to release the lock.
        /// This extension should be used though a <see langword="using"/> block or statement:
        /// <code>
        /// SpinLock spinLock = new SpinLock();
        ///
        /// using (spinLock.Enter())
        /// {
        ///     // Thread-safe code here...
        /// }
        /// </code>
        /// The compiler will take care of releasing the SpinLock when the code goes out of that <see langword="using"/> scope.
        /// </summary>
        /// <param name="spinLock">The target <see cref="SpinLock"/> to use</param>
        /// <returns>A wrapper type that will release <paramref name="spinLock"/> when its <see cref="System.IDisposable.Dispose"/> method is called.</returns>
        /// <remarks>The returned <see cref="__Lock"/> value shouldn't be used directly: use this extension in a <see langword="using"/> block or statement.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __Lock Enter(ref this SpinLock spinLock)
        {
            return new __Lock(ref spinLock);
        }

        /// <summary>
        /// A <see langword="struct"/> that is used to enter and hold a <see cref="SpinLock"/> through a <see langword="using"/> block or statement.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300", Justification = "The type is not meant to be used directly by users")]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
        public ref struct __Lock
        {
            /// <summary>
            /// The <see cref="ByReference{T}"/> instance pointing to the target <see cref="SpinLock"/> value to use.
            /// </summary>
            private readonly ByReference<SpinLock> r0;

            /// <summary>
            /// A value indicating whether or not the lock is taken by this <see cref="__Lock"/> instance.
            /// </summary>
            private readonly bool lockTaken;

            /// <summary>
            /// Initializes a new instance of the <see cref="__Lock"/> struct.
            /// </summary>
            /// <param name="spinLock">The target <see cref="SpinLock"/> to use.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __Lock(ref SpinLock spinLock)
            {
                r0 = new ByReference<SpinLock>(ref spinLock);
                lockTaken = false;

                spinLock.Enter(ref lockTaken);
            }

            /// <summary>
            /// Implements the duck-typed <see cref="System.IDisposable.Dispose"/> method and releases the current <see cref="SpinLock"/> instance.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                if (lockTaken)
                {
                    r0.Value.Exit();
                }
            }
        }
#endif
    }
}
