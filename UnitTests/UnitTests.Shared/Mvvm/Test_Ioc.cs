// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_Ioc
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_ServicesNotConfigured()
        {
            var ioc = new Ioc();

            Assert.ThrowsException<InvalidOperationException>(() => ioc.GetService<IServiceProvider>());
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

            var service = ioc.GetRequiredService<INameService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(AliceService));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_LambdaInitialization_ConcreteType()
        {
            var ioc = new Ioc();

            ioc.ConfigureServices(services =>
            {
                services.AddSingleton<AliceService, AliceService>();
            });

            var service = ioc.GetRequiredService<AliceService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(AliceService));
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_LambdaInitialization_ConstructorInjection()
        {
            var ioc = new Ioc();
            var messenger = new Messenger();

            ioc.ConfigureServices(services =>
            {
                services.AddSingleton<INameService, AliceService>();
                services.AddSingleton<IMessenger>(messenger);
                services.AddTransient<MyRecipient>();
            });

            var service = ioc.GetRequiredService<MyRecipient>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(MyRecipient));
            Assert.IsNotNull(service.NameService);
            Assert.IsInstanceOfType(service.NameService, typeof(AliceService));
            Assert.IsNotNull(service.MessengerService);
            Assert.AreSame(service.MessengerService, messenger);
        }

        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_CollectionInitialization()
        {
            var ioc = new Ioc();

            var services = new ServiceCollection();

            services.AddSingleton<INameService, AliceService>();

            ioc.ConfigureServices(services);

            var service = ioc.GetRequiredService<INameService>();

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

        public class MyRecipient : ObservableRecipient
        {
            public MyRecipient(INameService nameService, IMessenger messengerService)
                : base(messengerService)
            {
                NameService = nameService;
            }

            public INameService NameService { get; }

            public IMessenger MessengerService => Messenger;
        }
    }
}
