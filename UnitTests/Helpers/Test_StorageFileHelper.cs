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
        private const string SAMPLETEXT = "Lorem ipsum dolor sit amet";
        private const string FILENAME = "filename.txt";
        private const string PACKAGEDFILEPATH = @"Assets\Samples\lorem.txt";

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_PackagedFile()
        {
            string loadedText = await StorageFileHelper.ReadTextFromPackagedFileAsync(PACKAGEDFILEPATH);
            StringAssert.Contains(loadedText, SAMPLETEXT);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_LocalFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToLocalFileAsync(SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromLocalFileAsync(FILENAME);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_LocalCacheFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToLocalCacheFileAsync(SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromLocalCacheFileAsync(FILENAME);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_KnownFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToKnownFolderFileAsync(KnownFolderId.PicturesLibrary, SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromKnownFoldersFileAsync(KnownFolderId.PicturesLibrary, FILENAME);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Text_StorageFolder()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var storageFile = await StorageFileHelper.WriteTextToFileAsync(folder, SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            var loadedText = await StorageFileHelper.ReadTextFromFileAsync(folder, FILENAME);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_PackagedFile()
        {
            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromPackagedFileAsync(PACKAGEDFILEPATH);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            StringAssert.Contains(loadedText, SAMPLETEXT);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_LocalFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(SAMPLETEXT);

            var storageFile = await StorageFileHelper.WriteBytesToLocalFileAsync(unicodeBytes, FILENAME);
            Assert.IsNotNull(storageFile);

            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromLocalFileAsync(FILENAME);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_LocalCacheFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(SAMPLETEXT);

            var storageFile = await StorageFileHelper.WriteBytesToLocalCacheFileAsync(unicodeBytes, FILENAME);
            Assert.IsNotNull(storageFile);

            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromLocalCacheFileAsync(FILENAME);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_KnownFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(SAMPLETEXT);

            var storageFile = await StorageFileHelper.WriteBytesToKnownFolderFileAsync(KnownFolderId.PicturesLibrary, unicodeBytes, FILENAME);
            Assert.IsNotNull(storageFile);

            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromKnownFoldersFileAsync(KnownFolderId.PicturesLibrary, FILENAME);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_StorageFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(SAMPLETEXT);

            var folder = ApplicationData.Current.LocalFolder;
            var storageFile = await StorageFileHelper.WriteBytesToFileAsync(folder, unicodeBytes, FILENAME);
            Assert.IsNotNull(storageFile);

            byte[] loadedBytes = await StorageFileHelper.ReadBytesFromFileAsync(folder, FILENAME);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }
    }
}
