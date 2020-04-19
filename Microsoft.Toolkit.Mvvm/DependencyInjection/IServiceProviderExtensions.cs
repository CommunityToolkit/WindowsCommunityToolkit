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
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TService? GetService<TService>(this IServiceProvider provider)
            where TService : class
        {
            return (TService?)provider.GetService(typeof(TService));
        }

        /// <summary>
        /// Resolves an instance of a registered type implementing the <typeparamref name="TService"/> service.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>An instance of a registered type implementing <typeparamref name="TService"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the requested service was not registered.</exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TService GetRequiredService<TService>(this IServiceProvider provider)
            where TService : class
        {
            TService? service = (TService?)provider.GetService(typeof(TService));

            if (service is null)
            {
                ThrowInvalidOperationExceptionOnServiceNotRegistered<TService>();
            }

            return service!;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when a service of a given type is not found.
        /// </summary>
        /// <typeparam name="TService">The type of service not found.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidOperationExceptionOnServiceNotRegistered<TService>()
        {
            throw new InvalidOperationException($"Service {typeof(TService)} not registered");
        }
    }
}
