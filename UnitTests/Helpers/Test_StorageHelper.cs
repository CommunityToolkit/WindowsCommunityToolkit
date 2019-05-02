// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using UnitTests.UI;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_StorageHelper
    {
        private LocalObjectStorageHelper storageHelper = new LocalObjectStorageHelper();

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_LegacyIntTest()
        {
            string key = "LifeUniverseAndEverything";

            int input = 42;

            // simulate previous version by generating json and manually inserting it as string
            string jsonInput = JsonConvert.SerializeObject(input);

            storageHelper.Save<string>(key, jsonInput);

            // now read it as int to valid that the change works
            int output = storageHelper.Read<int>(key, 0);

            Assert.AreEqual(input, output);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_LegacyDateTest()
        {
            string key = "ChristmasDay";

            DateTime input = new DateTime(2017, 12, 25);

            // simulate previous version by generating json and manually inserting it as string
            string jsonInput = JsonConvert.SerializeObject(input);

            storageHelper.Save<string>(key, jsonInput);

            // now read it as int to valid that the change works
            DateTime output = storageHelper.Read<DateTime>(key, DateTime.Today);

            Assert.AreEqual(input, output);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_LegacyPersonTest()
        {
            string key = "Contact";

            Person input = new Person() { Name = "Joe Bloggs", Age = 42 };

            // simulate previous version by generating json and manually inserting it as string
            string jsonInput = JsonConvert.SerializeObject(input);

            storageHelper.Save<string>(key, jsonInput);

            // now read it as int to valid that the change works
            Person output = storageHelper.Read<Person>(key, null);

            Assert.IsNotNull(output);
            Assert.AreEqual(input.Name, output.Name);
            Assert.AreEqual(input.Age, output.Age);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_IntTest()
        {
            string key = "NewLifeUniverseAndEverything";

            int input = 42;

            storageHelper.Save<int>(key, input);

            // now read it as int to valid that the change works
            int output = storageHelper.Read<int>(key, 0);

            Assert.AreEqual(input, output);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_NewDateTest()
        {
            string key = "NewChristmasDay";

            DateTime input = new DateTime(2017, 12, 25);

            storageHelper.Save<DateTime>(key, input);

            // now read it as int to valid that the change works
            DateTime output = storageHelper.Read<DateTime>(key, DateTime.Today);

            Assert.AreEqual(input, output);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_NewPersonTest()
        {
            string key = "Contact";

            Person input = new Person() { Name = "Joe Bloggs", Age = 42 };

            storageHelper.Save<Person>(key, input);

            // now read it as int to valid that the change works
            Person output = storageHelper.Read<Person>(key, null);

            Assert.IsNotNull(output);
            Assert.AreEqual(input.Name, output.Name);
            Assert.AreEqual(input.Age, output.Age);
        }
    }
}
