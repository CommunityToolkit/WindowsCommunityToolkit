// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.Mvvm.DependencyInjection
{
    /// <summary>
    /// Extensions for the <see cref="IIoc"/> type.
    /// </summary>
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Resolves an instance of a registered type implementing the <typeparamref name="TService"/> service.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>An instance of a registered type implementing <typeparamref name="TService"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the requested service was not registered.</exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TService GetInstance<TService>(this IServiceProvider provider)
            where TService : class
        {
            return (TService)provider.GetService(typeof(TService));
        }
    }
}
