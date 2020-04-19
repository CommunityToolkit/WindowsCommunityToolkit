// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_Ioc
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_DefaultServiceProvider()
        {
            var ioc = new Ioc();

            IServiceProvider
                providerA = ioc.ServiceProvider,
                providerB = ioc.ServiceProvider;

            Assert.IsNotNull(providerA);
            Assert.IsNotNull(providerB);
            Assert.AreSame(providerA, providerB);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_LambdaInitialization()
        {
            var ioc = new Ioc();

            ioc.ConfigureServices(services =>
            {
                services.AddSingleton<INameService, AliceService>();
            });

            var service = ioc.ServiceProvider.GetRequiredService<INameService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(AliceService));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_CollectionInitialization()
        {
            var ioc = new Ioc();

            var services = new ServiceCollection();

            services.AddSingleton<INameService, AliceService>();

            ioc.ConfigureServices(services);

            var service = ioc.ServiceProvider.GetRequiredService<INameService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(AliceService));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_RepeatedLambdaInitialization()
        {
            var ioc = new Ioc();

            ioc.ConfigureServices(services => { });

            Assert.ThrowsException<InvalidOperationException>(() => ioc.ConfigureServices(services => { }));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_RepeatedCollectionInitialization()
        {
            var ioc = new Ioc();

            var services = new ServiceCollection();

            ioc.ConfigureServices(services);

            Assert.ThrowsException<InvalidOperationException>(() => ioc.ConfigureServices(services));
        }

        public interface INameService
        {
            string GetName();
        }

        public class BobService : INameService
        {
            public string GetName() => "Bob";
        }

        public class AliceService : INameService
        {
            public string GetName() => "Alice";
        }
    }
}
