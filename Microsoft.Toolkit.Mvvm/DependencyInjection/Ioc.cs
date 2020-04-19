// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

#nullable enable

namespace Microsoft.Toolkit.Mvvm.DependencyInjection
{
    /// <summary>
    /// A type that facilitates the use of the <see cref="IServiceProvider"/> type.
    /// The <see cref="Ioc"/> provides the ability to configure services in a singleton, thread-safe
    /// service provider instance, which is then automatically injected into all viewmodels inheriting
    /// from the provided <see cref="ComponentModel.ViewModelBase"/> class. First, you should define
    /// some services you will use in your app. Here is an example:
    /// <code>
    /// public interface ILogger
    /// {
    ///     void Log(string text);
    /// }
    /// </code>
    /// <code>
    /// public class ConsoleLogger : ILogger
    /// {
    ///     void Log(string text) => Console.WriteLine(text);
    /// }
    /// </code>
    /// The <see cref="ServiceProvider"/> configuration should then be done at startup, by calling one of
    /// the available <see cref="ConfigureServices(IServiceCollection)"/> overloads, like so:
    /// <code>
    /// Ioc.Default.ConfigureServices(collection =>
    /// {
    ///     collection.AddSingleton&lt;ILogger, Logger>();
    /// });
    /// </code>
    /// Finally, you can use the <see cref="ServiceProvider"/> property to retrieve the
    /// <see cref="IServiceProvider"/> instance with the collection of all the registered services:
    /// <code>
    /// Ioc.Default.ServiceProvider.GetService&lt;ILogger>().Log("Hello world!");
    /// </code>
    /// </summary>
    public sealed class Ioc
    {
        /// <summary>
        /// Gets the default <see cref="Ioc"/> instance.
        /// </summary>
        public static Ioc Default { get; } = new Ioc();

        /// <summary>
        /// An <see cref="object"/> used to synchronize access to <see cref="ServiceProvider"/>.
        /// </summary>
        private readonly object dummy = new object();

        private IServiceProvider? serviceProvider;

        /// <summary>
        /// Gets the shared <see cref="IServiceProvider"/> instance to use.
        /// </summary>
        /// <remarks>
        /// Make sure to call any of the <see cref="ConfigureServices(IServiceCollection)"/> overloads
        /// before accessing this property, otherwise it'll just fallback to an empty collection.
        /// </remarks>
        public IServiceProvider ServiceProvider
        {
            get
            {
                lock (dummy)
                {
                    return serviceProvider ??= new ServiceCollection().BuildServiceProvider();
                }
            }
        }

        /// <summary>
        /// Initializes the shared <see cref="IServiceProvider"/> instance.
        /// </summary>
        /// <param name="setup">The configuration delegate to use to add services.</param>
        public void ConfigureServices(Action<IServiceCollection> setup)
        {
            ConfigureServices(setup, new ServiceProviderOptions());
        }

        /// <summary>
        /// Initializes the shared <see cref="IServiceProvider"/> instance.
        /// </summary>
        /// <param name="setup">The configuration delegate to use to add services.</param>
        /// <param name="options">The <see cref="ServiceProviderOptions"/> instance to configure the service provider behaviors.</param>
        public void ConfigureServices(Action<IServiceCollection> setup, ServiceProviderOptions options)
        {
            var collection = new ServiceCollection();

            setup(collection);

            ConfigureServices(collection, options);
        }

        /// <summary>
        /// Initializes the shared <see cref="IServiceProvider"/> instance.
        /// </summary>
        /// <param name="collection">The input <see cref="IServiceCollection"/> instance to use.</param>
        public void ConfigureServices(IServiceCollection collection)
        {
            ConfigureServices(collection, new ServiceProviderOptions());
        }

        /// <summary>
        /// Initializes the shared <see cref="IServiceProvider"/> instance.
        /// </summary>
        /// <param name="collection">The input <see cref="IServiceCollection"/> instance to use.</param>
        /// <param name="options">The <see cref="ServiceProviderOptions"/> instance to configure the service provider behaviors.</param>
        public void ConfigureServices(IServiceCollection collection, ServiceProviderOptions options)
        {
            lock (dummy)
            {
                if (!(this.serviceProvider is null))
                {
                    ThrowInvalidOperationExceptionForRepeatedConfiguration();
                }

                this.serviceProvider ??= collection.BuildServiceProvider(options);
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when a configuration is attempted more than once.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowInvalidOperationExceptionForRepeatedConfiguration()
        {
            throw new InvalidOperationException("The default service provider has already been configured");
        }
    }
}
