// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Collections.Extensions;

#nullable enable

namespace Microsoft.Toolkit.Mvvm
{
    /// <summary>
    /// An Inversion of Control container that can be used to register and access instances of types providing services.
    /// It is focused on performance, and offers the ability to register service providers either through eagerly
    /// produced instances, or by using the factory pattern. The type is also fully thread-safe.
    /// In order to use this helper, first define a service and a service provider, like so:
    /// <code>
    /// public interface ILogger
    /// {
    ///     void Log(string text);
    /// }
    ///
    /// public class ConsoleLogger : ILogger
    /// {
    ///     void Log(string text) => Console.WriteLine(text);
    /// }
    /// </code>
    /// Then, register your services at startup, or at some time before using them:
    /// <code>
    /// Ioc.Default.Register&lt;ILogger, ConsoleLogger>();
    /// </code>
    /// Finally, use the <see cref="Ioc"/> type to retrieve service instances to use:
    /// <code>
    /// Ioc.Default.GetInstance&lt;ILogger>().Log("Hello world!");
    /// </code>
    /// The <see cref="Ioc"/> type will make sure to initialize your service instance if needed, or it will
    /// throw an exception in case the requested service has not been registered yet.
    /// </summary>
    public sealed class Ioc
    {
        /// <summary>
        /// The <see cref="IContainer"/> instance for each registered type.
        /// </summary>
        private readonly DictionarySlim<Key, IContainer> typesMap = new DictionarySlim<Key, IContainer>();

        /// <summary>
        /// Gets the default <see cref="Ioc"/> instance.
        /// </summary>
        public static Ioc Default { get; } = new Ioc();

        /// <summary>
        /// Checks whether or not a service of type <typeparamref name="TService"/> has already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of service to check for registration.</typeparam>
        /// <returns><see langword="true"/> if the service <typeparamref name="TService"/> has already been registered, <see langword="false"/> otherwise.</returns>
        [Pure]
        public bool IsRegistered<TService>()
            where TService : class
        {
            lock (this.typesMap)
            {
                return TryGetContainer<TService>(out _);
            }
        }

        /// <summary>
        /// Registers a singleton instance of service <typeparamref name="TService"/> through the type <typeparamref name="TProvider"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TProvider">The type of service provider for type <typeparamref name="TService"/> to register.</typeparam>
        /// <remarks>This method will create a new <typeparamref name="TProvider"/> instance for future use.</remarks>
        public void Register<TService, TProvider>()
            where TService : class
            where TProvider : class, TService, new()
        {
            Register<TService>(new TProvider());
        }

        /// <summary>
        /// Registers a singleton instance of service <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="provider">The <typeparamref name="TService"/> instance to register.</param>
        public void Register<TService>(TService provider)
            where TService : class
        {
            lock (this.typesMap)
            {
                Container<TService> container = GetContainer<TService>();

                container.Factory = null;
                container.Instance = provider;
            }
        }

        /// <summary>
        /// Registers a service <typeparamref name="TService"/> through a <see cref="Func{TResult}"/> factory.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="factory">The factory of instances implementing the <typeparamref name="TService"/> service to use.</param>
        public void Register<TService>(Func<TService> factory)
            where TService : class
        {
            lock (this.typesMap)
            {
                Container<TService> container = GetContainer<TService>();

                container.Factory = factory;
                container.Instance = null;
            }
        }

        /// <summary>
        /// Unregisters a service of a specified type.
        /// </summary>
        /// <typeparam name="TService">The type of service to unregister.</typeparam>
        public void Unregister<TService>()
            where TService : class
        {
            lock (this.typesMap)
            {
                var key = new Key(typeof(TService));

                this.typesMap.Remove(key);
            }
        }

        /// <summary>
        /// Resets the internal state of the <see cref="Ioc"/> type and unregisters all services.
        /// </summary>
        public void Reset()
        {
            lock (this.typesMap)
            {
                this.typesMap.Clear();
            }
        }

        /// <summary>
        /// Gets all the currently registered services.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyMemory{T}"/> collection of all the currently registered services.</returns>
        /// <remarks>
        /// This will also cause the <see cref="Ioc"/> service to resolve instances of
        /// registered services that were setup to use lazy initialization.
        /// </remarks>
        [Pure]
        public ReadOnlyMemory<object> GetAllServices()
        {
            lock (this.typesMap)
            {
                if (this.typesMap.Count == 0)
                {
                    return Array.Empty<object>();
                }

                object[] services = new object[this.typesMap.Count];

                int i = 0;
                foreach (var pair in this.typesMap)
                {
                    services[i++] = pair.Value.Instance ?? pair.Value.Factory!();
                }

                return services;
            }
        }

        /// <summary>
        /// Gets all the currently registered and instantiated services.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyMemory{T}"/> collection of all the currently registered and instantiated services.</returns>
        [Pure]
        public ReadOnlyMemory<object> GetAllCreatedServices()
        {
            lock (this.typesMap)
            {
                if (this.typesMap.Count == 0)
                {
                    return Array.Empty<object>();
                }

                object[] services = new object[this.typesMap.Count];

                int i = 0;
                foreach (var pair in this.typesMap)
                {
                    object? service = pair.Value.Instance;

                    if (service is null)
                    {
                        continue;
                    }

                    services[i++] = service;
                }

                return services.AsMemory(0, i);
            }
        }

        /// <summary>
        /// Tries to resolve an instance of a registered type implementing the <typeparamref name="TService"/> service.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>An instance of a registered type implementing <typeparamref name="TService"/>, if registered.</returns>
        [Pure]
        public bool TryGetInstance<TService>(out TService? service)
            where TService : class
        {
            lock (this.typesMap)
            {
                if (!TryGetContainer(out Container<TService>? container))
                {
                    service = null;

                    return false;
                }

                service = container!.Instance ?? container.Factory?.Invoke();

                return true;
            }
        }

        /// <summary>
        /// Resolves an instance of a registered type implementing the <typeparamref name="TService"/> service.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>An instance of a registered type implementing <typeparamref name="TService"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the requested service was not registered.</exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TService GetInstance<TService>()
            where TService : class
        {
            if (!TryGetInstance(out TService? service))
            {
                ThrowInvalidOperationExceptionOnServiceNotRegistered<TService>();
            }

            return service!;
        }

        /// <summary>
        /// Creates an instance of a registered type implementing the <typeparamref name="TService"/> service.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>A new instance of a registered type implementing <typeparamref name="TService"/>.</returns>
        [Pure]
        public TService GetInstanceWithoutCaching<TService>()
            where TService : class
        {
            lock (this.typesMap)
            {
                if (!TryGetContainer(out Container<TService>? container))
                {
                    ThrowInvalidOperationExceptionOnServiceNotRegistered<TService>();
                }

                Func<TService>? factory = container!.Factory;

                if (!(factory is null))
                {
                    return factory();
                }

                TService service = container.Instance!;

                if (service.GetType().GetConstructor(Type.EmptyTypes) is null)
                {
                    ThrowInvalidOperationExceptionOnServiceWithNoConstructor<TService>(service.GetType());
                }

                Type serviceType = service.GetType();
                Expression[] expressions = { Expression.New(serviceType) };
                Expression body = Expression.Block(serviceType, expressions);
                factory = Expression.Lambda<Func<TService>>(body).Compile();

                // Cache for later use
                container.Factory = factory;

                return factory();
            }
        }

        /// <summary>
        /// Tries to get a <see cref="Container{T}"/> instance for a specific service type.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <param name="container">The resulting <see cref="Container{T}"/>, if found.</param>
        /// <returns>Whether or not the container was present.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryGetContainer<TService>(out Container<TService>? container)
            where TService : class
        {
            var key = new Key(typeof(TService));

            if (this.typesMap.TryGetValue(key, out IContainer local))
            {
                // We can use a fast cast here as the type is always known
                container = Unsafe.As<Container<TService>>(local);

                return true;
            }

            container = null;

            return false;
        }

        /// <summary>
        /// Gets the <see cref="Container{T}"/> instance for a specific service type.
        /// </summary>
        /// <typeparam name="TService">The type of service to look for.</typeparam>
        /// <returns>A <see cref="Container{T}"/> instance with the requested type argument.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Container<TService> GetContainer<TService>()
            where TService : class
        {
            var key = new Key(typeof(TService));
            ref IContainer target = ref this.typesMap.GetOrAddValueRef(key);

            if (target is null)
            {
                target = new Container<TService>();
            }

            return Unsafe.As<Container<TService>>(target);
        }

        /// <summary>
        /// A simple type representing a registered type.
        /// </summary>
        /// <remarks>
        /// This type is used to enable fast indexing in the type mapping, and is used to externally
        /// inject the <see cref="IEquatable{T}"/> interface to <see cref="Type"/>.
        /// </remarks>
        private readonly struct Key : IEquatable<Key>
        {
            /// <summary>
            /// The registered type.
            /// </summary>
            private readonly Type type;

            /// <summary>
            /// Initializes a new instance of the <see cref="Key"/> struct.
            /// </summary>
            /// <param name="type">The input <see cref="Type"/> instance.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Key(Type type)
            {
                this.type = type;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Equals(Key other)
            {
                return ReferenceEquals(this.type, other.type);
            }

            /// <inheritdoc/>
            public override bool Equals(object? obj)
            {
                return obj is Key other && Equals(other);
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetHashCode()
            {
                return RuntimeHelpers.GetHashCode(this.type);
            }
        }

        /// <summary>
        /// Internal container type for services of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of services to store in this type.</typeparam>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401", Justification = "The type is private")]
        private sealed class Container<T> : IContainer
            where T : class
        {
            /// <inheritdoc/>
            public object Lock { get; } = new object();

            /// <summary>
            /// The optional <see cref="Func{T}"/> instance to produce instances of the service <typeparamref name="T"/>.
            /// </summary>
            public Func<T>? Factory;

            /// <summary>
            /// The current <typeparamref name="T"/> instance being produced, if available.
            /// </summary>
            public T? Instance;

            /// <inheritdoc/>
            Func<object>? IContainer.Factory => this.Factory;

            /// <inheritdoc/>
            object? IContainer.Instance => this.Instance;
        }

        /// <summary>
        /// A interface for the <see cref="Container{T}"/> type.
        /// </summary>
        private interface IContainer
        {
            /// <summary>
            /// Gets an <see cref="object"/> used to synchronize accesses to the <see cref="Factory"/> and <see cref="Instance"/> fields.
            /// </summary>
            object Lock { get; }

            /// <summary>
            /// Gets the optional <see cref="Func{T}"/> instance to produce instances of the service.
            /// </summary>
            Func<object>? Factory { get; }

            /// <summary>
            /// Gets the current instance being produced, if available.
            /// </summary>
            object? Instance { get; }
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

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when a service doesn't expose a public parametrless constructor.
        /// </summary>
        /// <typeparam name="TService">The type of service not found.</typeparam>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidOperationExceptionOnServiceWithNoConstructor<TService>(Type type)
        {
            throw new InvalidOperationException($"Type {type} implementing service {typeof(TService)} does not expose a public constructor");
        }
    }
}
