﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.Mvvm.Messaging
{
    /// <summary>
    /// Extensions for the <see cref="IMessenger"/> type.
    /// </summary>
    public static partial class MessengerExtensions
    {
        /// <summary>
        /// An empty type representing a generic token with no specific value.
        /// </summary>
        private readonly struct Unit : IEquatable<Unit>
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
    }
}
