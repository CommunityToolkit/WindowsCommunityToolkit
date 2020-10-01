// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.Mvvm.Messaging.Internals
{
    /// <summary>
    /// A simple type representing an immutable pair of objects.
    /// </summary>
    /// <remarks>
    /// This type replaces a simple <see cref="ValueTuple{T1,T2}"/> when specifically dealing with a
    /// pair of handler and recipient. Unlike <see cref="ValueTuple{T1,T2}"/>, this type is readonly.
    /// </remarks>
    internal readonly struct Object2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Object2"/> struct.
        /// </summary>
        /// <param name="handler">The input handler.</param>
        /// <param name="recipient">The input recipient.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Object2(object handler, object recipient)
        {
            Handler = handler;
            Recipient = recipient;
        }

        /// <summary>
        /// The current handler (which is actually some <see cref="MessageHandler{TRecipient,TMessage}"/> instance.
        /// </summary>
        public readonly object Handler;

        /// <summary>
        /// The current recipient (guaranteed to match the type constraints for <see cref="MessageHandler{TRecipient,TMessage}"/>.
        /// </summary>
        public readonly object Recipient;
    }
}
