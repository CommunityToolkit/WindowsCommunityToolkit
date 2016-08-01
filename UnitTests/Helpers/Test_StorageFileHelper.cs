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

        [TestMethod]
        public async Task Test_StorageFileHelper_Text_PackagedFile()
        {
            string loadedText = await StorageFileHelper.ReadTextFromPackagedFileAsync(PACKAGEDFILEPATH);
            StringAssert.StartsWith(loadedText, SAMPLETEXT);
        }

        [TestMethod]
        public async Task Test_StorageFileHelper_Text_LocalFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToLocalFileAsync(SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromLocalFileAsync(FILENAME);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestMethod]
        public async Task Test_StorageFileHelper_Text_LocalCacheFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToLocalCacheFileAsync(SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromLocalCacheFileAsync(FILENAME);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestMethod]
        public async Task Test_StorageFileHelper_Text_KnownFolder()
        {
            var storageFile = await StorageFileHelper.WriteTextToKnownFolderFileAsync(KnownFolderId.PicturesLibrary, SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            string loadedText = await StorageFileHelper.ReadTextFromKnownFoldersFileAsync(KnownFolderId.PicturesLibrary, FILENAME);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestMethod]
        public async Task Test_StorageFileHelper_Text_StorageFolder()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var storageFile = await StorageFileHelper.WriteTextToFileAsync(folder, SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            // Question - StorageFileHelper.ReadTextToFileAsync !?
            var file = await folder.GetFileAsync(FILENAME);
            var loadedText = await FileIO.ReadTextAsync(file);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_LocalFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(SAMPLETEXT);

            var storageFile = await StorageFileHelper.WriteBytesToLocalFileAsync(unicodeBytes, FILENAME);
            Assert.IsNotNull(storageFile);

            // Question - StorageFileHelper.ReadBytesFromLocalFileAsync !?
            byte[] loadedBytes = await StorageFileHelper.GetBytesFromFileAsync(storageFile);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_LocalCacheFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(SAMPLETEXT);

            var storageFile = await StorageFileHelper.WriteBytesToLocalCacheFileAsync(unicodeBytes, FILENAME);
            Assert.IsNotNull(storageFile);

            // Question - StorageFileHelper.ReadBytesFromLocalCacheFileAsync !?
            byte[] loadedBytes = await StorageFileHelper.GetBytesFromFileAsync(storageFile);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_KnownFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(SAMPLETEXT);

            var storageFile = await StorageFileHelper.WriteBytesToKnownFolderFileAsync(KnownFolderId.PicturesLibrary, unicodeBytes, FILENAME);
            Assert.IsNotNull(storageFile);

            // Question - StorageFileHelper.ReadBytesFromKnownFolderAsync !?
            byte[] loadedBytes = await StorageFileHelper.GetBytesFromFileAsync(storageFile);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestMethod]
        public async Task Test_StorageFileHelper_Bytes_StorageFolder()
        {
            byte[] unicodeBytes = Encoding.Unicode.GetBytes(SAMPLETEXT);

            var folder = ApplicationData.Current.LocalFolder;
            var storageFile = await StorageFileHelper.WriteBytesToFileAsync(folder, unicodeBytes, FILENAME);
            Assert.IsNotNull(storageFile);

            // Question - Convention de nommage - Get/Read
            byte[] loadedBytes = await StorageFileHelper.GetBytesFromFileAsync(storageFile);

            string loadedText = Encoding.Unicode.GetString(loadedBytes);
            Assert.AreEqual(SAMPLETEXT, loadedText);

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }
    }
}
