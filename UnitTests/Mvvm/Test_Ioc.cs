// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Mvvm;
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
            Assert.IsFalse(Ioc.IsRegistered<INameService>());
            Assert.AreEqual(Ioc.GetAllServices().Count, 0);

            Ioc.Register<INameService, BobService>();

            Assert.IsTrue(Ioc.IsRegistered<INameService>());

            var services = Ioc.GetAllServices();

            Assert.AreEqual(services.Count, 1);
            Assert.IsInstanceOfType(services.First(), typeof(INameService));
            Assert.IsInstanceOfType(services.First(), typeof(BobService));

            Ioc.Unregister<INameService>();

            Assert.IsFalse(Ioc.IsRegistered<INameService>());
            Assert.AreEqual(Ioc.GetAllServices().Count, 0);

            Ioc.Register<INameService>(() => new AliceService());

            Assert.IsTrue(Ioc.IsRegistered<INameService>());

            services = Ioc.GetAllServices();

            Assert.AreEqual(services.Count, 1);
            Assert.IsInstanceOfType(services.First(), typeof(INameService));
            Assert.IsInstanceOfType(services.First(), typeof(AliceService));

            Ioc.Reset();

            Assert.IsFalse(Ioc.IsRegistered<INameService>());
            Assert.AreEqual(Ioc.GetAllServices().Count, 0);
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
