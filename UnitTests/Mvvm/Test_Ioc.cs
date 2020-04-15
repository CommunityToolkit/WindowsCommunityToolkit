// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Mvvm.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Mvvm
{
    [TestClass]
    public class Test_Ioc
    {
        [TestCategory("Mvvm")]
        [TestMethod]
        public void Test_Ioc_SampleUsage()
        {
            Assert.IsFalse(Ioc.Default.IsRegistered<INameService>());
            Assert.AreEqual(Ioc.Default.GetAllServices().Length, 0);

            Ioc.Default.Register<INameService, BobService>();

            Assert.IsTrue(Ioc.Default.IsRegistered<INameService>());

            var services = Ioc.Default.GetAllServices();

            Assert.AreEqual(services.Length, 1);
            Assert.IsInstanceOfType(services.Span[0], typeof(INameService));
            Assert.IsInstanceOfType(services.Span[0], typeof(BobService));

            Ioc.Default.Unregister<INameService>();

            Assert.IsFalse(Ioc.Default.IsRegistered<INameService>());
            Assert.AreEqual(Ioc.Default.GetAllServices().Length, 0);

            Ioc.Default.Register<INameService>(() => new AliceService());

            Assert.IsTrue(Ioc.Default.IsRegistered<INameService>());

            services = Ioc.Default.GetAllServices();

            Assert.AreEqual(services.Length, 1);
            Assert.IsInstanceOfType(services.Span[0], typeof(INameService));
            Assert.IsInstanceOfType(services.Span[0], typeof(AliceService));

            Ioc.Default.Reset();

            Assert.IsFalse(Ioc.Default.IsRegistered<INameService>());
            Assert.AreEqual(Ioc.Default.GetAllServices().Length, 0);
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
