// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_StorageFileHelper
    {
        private const string Sampletext = "Lorem ipsum dolor sit amet";
        private const string Filename = "filename.txt";
        private const string PackagedFilePath = @"Assets\Samples\lorem.txt";

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_PackagedFile()
        {
            string loadedText = await StorageFileHelper.ReadTextFromPackagedFileAsync(PackagedFilePath);
            StringAssert.Contains(loadedText, Sampletext);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_LocalFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToLocalFileAsync(Sampletext, Filename);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromLocalFileAsync(Filename);
            Assert.AreEqual(Sampletext, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_LocalCacheFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToLocalCacheFileAsync(Sampletext, Filename);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromLocalCacheFileAsync(Filename);
            Assert.AreEqual(Sampletext, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_KnownFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToKnownFolderFileAsync(KnownFolderId.PicturesLibrary, Sampletext, Filename);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromKnownFoldersFileAsync(KnownFolderId.PicturesLibrary, Filename);
            Assert.AreEqual(Sampletext, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_StorageFolder()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var storageFile = await StorageFileHelper.WriteTextToFileAsync(folder, Sampletext, Filename);
            Assert.IsNotNull(storageFile);

            var loadedText = await StorageFileHelper.ReadTextFromFileAsync(folder, Filename);
            Assert.AreEqual(Sampletext, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_PackagedFile()
        {
            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromPackagedFileAsync(PackagedFilePath);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            StringAssert.Contains(loadedText, Sampletext);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_LocalFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(Sampletext);

            var storageFile = await StorageFileHelper.WriteBytesToLocalFileAsync(unicodeBytes, Filename);
            Assert.IsNotNull(storageFile);

            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromLocalFileAsync(Filename);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(Sampletext, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_LocalCacheFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(Sampletext);

            var storageFile = await StorageFileHelper.WriteBytesToLocalCacheFileAsync(unicodeBytes, Filename);
            Assert.IsNotNull(storageFile);

            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromLocalCacheFileAsync(Filename);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(Sampletext, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_KnownFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(Sampletext);

            var storageFile = await StorageFileHelper.WriteBytesToKnownFolderFileAsync(KnownFolderId.PicturesLibrary, unicodeBytes, Filename);
            Assert.IsNotNull(storageFile);

            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromKnownFoldersFileAsync(KnownFolderId.PicturesLibrary, Filename);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(Sampletext, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_StorageFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(Sampletext);

            var folder = ApplicationData.Current.LocalFolder;
            var storageFile = await StorageFileHelper.WriteBytesToFileAsync(folder, unicodeBytes, Filename);
            Assert.IsNotNull(storageFile);

            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromFileAsync(folder, Filename);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(Sampletext, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }
    }
}
