// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

#nullable enable

namespace Microsoft.Toolkit.Mvvm.DependencyInjection
{
    /// <summary>
    /// A type that facilitates the use of the <see cref="IServiceProvider"/> type.
    /// The <see cref="Ioc"/> provides the ability to configure services in a singleton, thread-safe
    /// service provider instance, which can then be used to resolve service instances.
    /// The first step to use this feature is to declare some services, for instance:
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
    /// Then the services configuration should then be done at startup, by calling one of
    /// the available <see cref="ConfigureServices(IServiceCollection)"/> overloads, like so:
    /// <code>
    /// Ioc.Default.ConfigureServices(services =>
    /// {
    ///     services.AddSingleton&lt;ILogger, Logger&gt;();
    /// });
    /// </code>
    /// Finally, you can use the <see cref="Ioc"/> instance (which implements <see cref="IServiceProvider"/>)
    /// to retrieve the service instances from anywhere in your application, by doing as follows:
    /// <code>
    /// Ioc.Default.GetService&lt;ILogger&gt;().Log("Hello world!");
    /// </code>
    /// </summary>
    public sealed class Ioc : IServiceProvider
    {
        /// <summary>
        /// Gets the default <see cref="Ioc"/> instance.
        /// </summary>
        public static Ioc Default { get; } = new Ioc();

        /// <summary>
        /// The <see cref="ServiceProvider"/> instance to use, if initialized.
        /// </summary>
        private ServiceProvider? serviceProvider;

        /// <inheritdoc/>
        object? IServiceProvider.GetService(Type serviceType)
        {
            // As per section I.12.6.6 of the official CLI ECMA-335 spec:
            // "[...] read and write access to properly aligned memory locations no larger than the native
            // word size is atomic when all the write accesses to a location are the same size. Atomic writes
            // shall alter no bits other than those written. Unless explicit layout control is used [...],
            // data elements no larger than the natural word size [...] shall be properly aligned.
            // Object references shall be treated as though they are stored in the native word size."
            // The field being accessed here is of native int size (reference type), and is only ever accessed
            // directly and atomically by a compare exhange instruction (see below), or here. We can therefore
            // assume this read is thread safe with respect to accesses to this property or to invocations to one
            // of the available configuration methods. So we can just read the field directly and make the necessary
            // check with our local copy, without the need of paying the locking overhead from this get accessor.
            ServiceProvider? provider = this.serviceProvider;

            if (provider is null)
            {
                ThrowInvalidOperationExceptionForMissingInitialization();
            }

            return provider!.GetService(serviceType);
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
        /// <param name="services">The input <see cref="IServiceCollection"/> instance to use.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServices(services, new ServiceProviderOptions());
        }

        /// <summary>
        /// Initializes the shared <see cref="IServiceProvider"/> instance.
        /// </summary>
        /// <param name="services">The input <see cref="IServiceCollection"/> instance to use.</param>
        /// <param name="options">The <see cref="ServiceProviderOptions"/> instance to configure the service provider behaviors.</param>
        public void ConfigureServices(IServiceCollection services, ServiceProviderOptions options)
        {
            ServiceProvider newServices = services.BuildServiceProvider(options);

            ServiceProvider? oldServices = Interlocked.CompareExchange(ref this.serviceProvider, newServices, null);

            if (!(oldServices is null))
            {
                ThrowInvalidOperationExceptionForRepeatedConfiguration();
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when the <see cref="ServiceProvider"/> property is used before initialization.
        /// </summary>
        private static void ThrowInvalidOperationExceptionForMissingInitialization()
        {
            throw new InvalidOperationException("The service provider has not been configured yet");
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when a configuration is attempted more than once.
        /// </summary>
        private static void ThrowInvalidOperationExceptionForRepeatedConfiguration()
        {
            throw new InvalidOperationException("The default service provider has already been configured");
        }
    }
}
