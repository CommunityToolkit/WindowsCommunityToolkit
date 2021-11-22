// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.Common.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_ApplicationDataStorageHelper
    {
        private readonly ApplicationDataStorageHelper _settingsStorage_System = ApplicationDataStorageHelper.GetCurrent();
        private readonly ApplicationDataStorageHelper _settingsStorage_JsonCompat = ApplicationDataStorageHelper.GetCurrent(new JsonObjectSerializer2());
        private readonly ApplicationDataStorageHelper _settingsStorage_JsonNew = ApplicationDataStorageHelper.GetCurrent(new SystemTextJsonSerializer2());

        /// <summary>
        /// Checks that we're running 10.0.3 version of Newtonsoft.Json package which we used in 6.1.1.
        /// </summary>
        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_CheckNewtonsoftVersion()
        {
            var version = typeof(Newtonsoft.Json.JsonSerializer).Assembly.GetName().Version;
            Assert.AreEqual(10, version.Major);
            Assert.AreEqual(0, version.Minor);
            Assert.AreEqual(0, version.Revision); // Apparently the file revision was not updated for the updated package
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_LegacyIntTest()
        {
            string key = "LifeUniverseAndEverything";

            int input = 42;

            // Use our previous Json layer to store value
            _settingsStorage_JsonCompat.Save(key, input);

            // But try and read from our new system to see if it works
            int output = _settingsStorage_System.Read(key, 0);

            Assert.AreEqual(input, output);
        }

        /// <summary>
        /// If we try and deserialize a complex type with the <see cref="Microsoft.Toolkit.Helpers.SystemSerializer"/>, we do a check ourselves and will throw our own exception.
        /// </summary>
        [TestCategory("Helpers")]
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_StorageHelper_LegacyDateTestFailure()
        {
            string key = "ChristmasDay";

            DateTime input = new DateTime(2017, 12, 25);

            _settingsStorage_JsonCompat.Save(key, input);

            // now read it as int to valid that the change works
            _ = _settingsStorage_System.Read(key, DateTime.Today);
        }

        /// <summary>
        /// The <see cref="Microsoft.Toolkit.Helpers.SystemSerializer"/> doesn't support complex types, since it just passes through directly.
        /// We'll get the argument exception from the <see cref="Windows.Storage.ApplicationDataContainer"/> API.
        /// </summary>
        [TestCategory("Helpers")]
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_StorageHelper_DateTestFailure()
        {
            string key = "Today";

            _settingsStorage_System.Save(key, DateTime.Today);
            _settingsStorage_System.TryRead<DateTime>(key, out _);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_LegacyInternalClassTest()
        {
            string key = "Contact";

            UI.Person input = new UI.Person() { Name = "Joe Bloggs", Age = 42 };

            // simulate previous version by generating json and manually inserting it as string
            _settingsStorage_JsonCompat.Save(key, input);

            // now read it as int to valid that the change works
            UI.Person output = _settingsStorage_JsonCompat.Read<UI.Person>(key, null);

            Assert.IsNotNull(output);
            Assert.AreEqual(input.Name, output.Name);
            Assert.AreEqual(input.Age, output.Age);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_LegacyPublicClassTest()
        {
            string key = "Contact";

            // Here's we're serializing a different class which has the same properties as our other class below.
            UI.Person input = new UI.Person() { Name = "Joe Bloggs", Age = 42 };

            // simulate previous version by generating json and manually inserting it as string
            _settingsStorage_JsonCompat.Save(key, input);

            // now read it as int to valid that the change works
            Person output = _settingsStorage_JsonCompat.Read<Person>(key, null);

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

            _settingsStorage_System.Save<int>(key, input);

            // now read it as int to valid that the change works
            int output = _settingsStorage_System.Read<int>(key, 0);

            Assert.AreEqual(input, output);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_NewDateTest()
        {
            string key = "NewChristmasDay";

            DateTime input = new DateTime(2017, 12, 25);

            _settingsStorage_JsonNew.Save(key, input);

            // now read it as int to valid that the change works
            DateTime output = _settingsStorage_JsonNew.Read(key, DateTime.Today);

            Assert.AreEqual(input, output);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_StorageHelper_NewPersonTest()
        {
            string key = "Contact";

            Person input = new Person() { Name = "Joe Bloggs", Age = 42 };

            _settingsStorage_JsonNew.Save(key, input);

            // now read it as int to valid that the change works
            Person output = _settingsStorage_JsonNew.Read<Person>(key, null);

            Assert.IsNotNull(output);
            Assert.AreEqual(input.Name, output.Name);
            Assert.AreEqual(input.Age, output.Age);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageHelper_FileCRUDTest()
        {
            var fileName = "TestFile.txt";
            var fileName2 = "TestFile2.txt";
            var fileContents = "this is a test";
            var fileContents2 = "this is also a test";
            var storageHelper = ApplicationDataStorageHelper.GetCurrent();

            // Create a file
            await storageHelper.CreateFileAsync(fileName, fileContents);

            // Read a file
            var readContents = await storageHelper.ReadFileAsync<string>(fileName);
            Assert.AreEqual(fileContents, readContents);

            // Update a file
            await storageHelper.CreateFileAsync(fileName, fileContents2);
            var readContents2 = await storageHelper.ReadFileAsync<string>(fileName);
            Assert.AreEqual(fileContents2, readContents2);

            // Rename a file
            var itemRenamed = await storageHelper.TryRenameItemAsync(fileName, fileName2);
            Assert.IsTrue(itemRenamed);

            // Delete a file
            var itemDeleted = await storageHelper.TryDeleteItemAsync(fileName2);
            Assert.IsTrue(itemDeleted);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageHelper_SubFolderCRUDTest()
        {
            var folderName = "TestFolder1";
            var subFolderName = "TestSubFolder";
            var subFolderName2 = "TestSubFolder2";
            var subFolderPath = $"{folderName}/{subFolderName}";
            var subFolderPath2 = $"{folderName}/{subFolderName2}";
            var fileName = "TestFile.txt";
            var fileName2 = "TestFile2.txt";
            var filePath = $"{subFolderPath}/{fileName}";
            var filePath2 = $"{subFolderPath2}/{fileName2}";
            var fileContents = "this is a test";

            var storageHelper = ApplicationDataStorageHelper.GetCurrent();

            // Attempt to delete the folder to clean up from any previously failed test runs.
            await storageHelper.TryDeleteItemAsync(folderName);

            // Create a subfolder
            await storageHelper.CreateFolderAsync(subFolderPath);

            // Create a file in the subfolder
            await storageHelper.CreateFileAsync(filePath, fileContents);

            // Read a file from the subfolder
            var readContents = await storageHelper.ReadFileAsync<string>(filePath);
            Assert.AreEqual(fileContents, readContents);

            // List subfolder contents
            var folderItems = await storageHelper.ReadFolderAsync(subFolderPath);
            var folderItemsList = folderItems.ToList();
            Assert.AreEqual(1, folderItemsList.Count());
            Assert.AreEqual(fileName, folderItemsList[0].Name);
            Assert.AreEqual(DirectoryItemType.File, folderItemsList[0].ItemType);

            // Rename a file in a subfolder
            var itemRenamed = await storageHelper.TryRenameItemAsync(filePath, fileName2);
            Assert.IsTrue(itemRenamed);

            // Rename a subfolder
            var folderRenamed = await storageHelper.TryRenameItemAsync(subFolderPath, subFolderName2);
            Assert.IsTrue(folderRenamed);

            // Delete a file in a subfolder
            var fileDeleted = await storageHelper.TryDeleteItemAsync(filePath2);
            Assert.IsTrue(fileDeleted);

            // Delete a subfolder
            var subFolderDeleted = await storageHelper.TryDeleteItemAsync(subFolderPath2);
            Assert.IsTrue(subFolderDeleted);

            // Delete the folder to clean up.
            var folderDeleted = await storageHelper.TryDeleteItemAsync(folderName);
            Assert.IsTrue(folderDeleted);
        }

        public class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }
    }
}
