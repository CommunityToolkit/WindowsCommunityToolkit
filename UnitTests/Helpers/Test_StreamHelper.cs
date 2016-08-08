using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_StreamHelper
    {
        private const string SAMPLETEXT = "Lorem ipsum dolor sit amet";
        private const string FILENAME = "filename.txt";
        private const string PACKAGEDFILEPATH = @"Assets\Samples\lorem.txt";

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_GetHttpStream()
        {
            using (var stream = await StreamHelper.GetHttpStreamAsync(new Uri("http://dev.windows.com")))
            {
                Assert.IsNotNull(stream);
            }
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_GetHttpStreamToStorageFile()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(FILENAME, CreationCollisionOption.ReplaceExisting);

            await StreamHelper.GetHttpStreamToStorageFileAsync(new Uri("http://dev.windows.com"), file);

            var properties = await file.GetBasicPropertiesAsync();
            Assert.IsTrue(properties.Size > 0);

            await file.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_PackagedFile()
        {
            Assert.IsTrue(await StreamHelper.IsPackagedFileExistsAsync(PACKAGEDFILEPATH));

            using (var stream = await StreamHelper.GetPackagedFileStreamAsync(PACKAGEDFILEPATH))
            {
                var loadedText = await stream.ReadTextAsync(Encoding.Unicode);
                StringAssert.Contains(loadedText, SAMPLETEXT);
            }
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_LocalFolder()
        {
            Assert.IsFalse(await StreamHelper.IsLocalFileExistsAsync(FILENAME));

            var storageFile = await StorageFileHelper.WriteTextToLocalFileAsync(SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            Assert.IsTrue(await StreamHelper.IsLocalFileExistsAsync(FILENAME));

            using (var stream = await StreamHelper.GetLocalFileStreamAsync(FILENAME))
            {
                var loadedText = await stream.ReadTextAsync(Encoding.UTF8);
                StringAssert.Contains(loadedText, SAMPLETEXT);
            }

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_LocalCacheFolder()
        {
            Assert.IsFalse(await StreamHelper.IsLocalCacheFileExistsAsync(FILENAME));

            var storageFile = await StorageFileHelper.WriteTextToLocalCacheFileAsync(SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            Assert.IsTrue(await StreamHelper.IsLocalCacheFileExistsAsync(FILENAME));

            using (var stream = await StreamHelper.GetLocalCacheFileStreamAsync(FILENAME))
            {
                var loadedText = await stream.ReadTextAsync(Encoding.UTF8);
                StringAssert.Contains(loadedText, SAMPLETEXT);
            }

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_KnownFolder()
        {
            var folder = KnownFolderId.PicturesLibrary;

            Assert.IsFalse(await StreamHelper.IsKnownFolderFileExistsAsync(folder, FILENAME));

            var storageFile = await StorageFileHelper.WriteTextToKnownFolderFileAsync(folder, SAMPLETEXT, FILENAME);
            Assert.IsNotNull(storageFile);

            Assert.IsTrue(await StreamHelper.IsKnownFolderFileExistsAsync(folder, FILENAME));

            using (var stream = await StreamHelper.GetKnowFoldersFileStreamAsync(folder, FILENAME))
            {
                var loadedText = await stream.ReadTextAsync(Encoding.UTF8);
                StringAssert.Contains(loadedText, SAMPLETEXT);
            }

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }
    }
}
