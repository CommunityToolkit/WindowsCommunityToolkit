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
    /// An interface for a type implementing an Inversion of Control service provider.
    /// </summary>
    public interface IIoc
    {
        /// <summary>
        /// Checks whether or not a service of type <typeparamref name="TService"/> has already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of service to check for registration.</typeparam>
        /// <returns><see langword="true"/> if the service <typeparamref name="TService"/> has already been registered, <see langword="false"/> otherwise.</returns>
        [Pure]
        bool IsRegistered<TService>()
            where TService : class;

        /// <summary>
        /// Registers a singleton instance of service <typeparamref name="TService"/> through the type <typeparamref name="TProvider"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TProvider">The type of service provider for type <typeparamref name="TService"/> to register.</typeparam>
        /// <remarks>This method will create a new <typeparamref name="TProvider"/> instance for future use.</remarks>
        void Register<TService, TProvider>()
            where TService : class
            where TProvider : class, TService, new();

        /// <summary>
        /// Registers a singleton instance of service <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="provider">The <typeparamref name="TService"/> instance to register.</param>
        void Register<TService>(TService provider)
            where TService : class;

        /// <summary>
        /// Registers a service <typeparamref name="TService"/> through a <see cref="Func{TResult}"/> factory.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="factory">The factory of instances implementing the <typeparamref name="TService"/> service to use.</param>
        void Register<TService>(Func<TService> factory)
            where TService : class;

        /// <summary>
        /// Unregisters a service of a specified type.
        /// </summary>
        /// <typeparam name="TService">The type of service to unregister.</typeparam>
        void Unregister<TService>()
            where TService : class;

        /// <summary>
        /// Resets the internal state of the <see cref="Ioc"/> type and unregisters all services.
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets all the currently registered services.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyMemory{T}"/> collection of all the currently registered services.</returns>
        /// <remarks>
        /// This will also cause the <see cref="Ioc"/> service to resolve instances of
        /// registered services that were setup to use lazy initialization.
        /// </remarks>
        [Pure]
        ReadOnlyMemory<object> GetAllServices();

        /// <summary>
        /// Gets all the currently registered and instantiated services.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyMemory{T}"/> collection of all the currently registered and instantiated services.</returns>
        [Pure]
        ReadOnlyMemory<object> GetAllCreatedServices();

        /// <summary>
        /// Tries to resolve an instance of a registered type implementing the <typeparamref name="TService"/> service.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>An instance of a registered type implementing <typeparamref name="TService"/>, if registered.</returns>
        [Pure]
        bool TryGetInstance<TService>(out TService? service)
            where TService : class;

        /// <summary>
        /// Resolves an instance of a registered type implementing the <typeparamref name="TService"/> service.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>An instance of a registered type implementing <typeparamref name="TService"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the requested service was not registered.</exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        TService GetInstance<TService>()
            where TService : class;

        /// <summary>
        /// Creates an instance of a registered type implementing the <typeparamref name="TService"/> service.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>A new instance of a registered type implementing <typeparamref name="TService"/>.</returns>
        [Pure]
        TService GetInstanceWithoutCaching<TService>()
            where TService : class;
    }
}
