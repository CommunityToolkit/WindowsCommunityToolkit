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
using Windows.ApplicationModel;
using Windows.Storage;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_StreamHelper
    {
        private const string SampleText = "Lorem ipsum dolor sit amet";
        private const string Filename = "filename.txt";
        private const string PackagedFilePath = @"Assets\Samples\lorem.txt";

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
            StorageFile file = await folder.CreateFileAsync(Filename, CreationCollisionOption.ReplaceExisting);

            await StreamHelper.GetHttpStreamToStorageFileAsync(new Uri("http://dev.windows.com"), file);

            var properties = await file.GetBasicPropertiesAsync();
            Assert.IsTrue(properties.Size > 0);

            await file.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_PackagedFile()
        {
            var packageFolder = Package.Current.InstalledLocation;
            Assert.IsTrue(await StorageFileHelper.FileExistsAsync(packageFolder, PackagedFilePath));

            using (var stream = await StreamHelper.GetPackagedFileStreamAsync(PackagedFilePath))
            {
                var loadedText = await stream.ReadTextAsync(Encoding.Unicode);
                StringAssert.Contains(loadedText, SampleText);
            }
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_LocalFolder()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            Assert.IsFalse(await StorageFileHelper.FileExistsAsync(localFolder, Filename));

            var storageFile = await StorageFileHelper.WriteTextToLocalFileAsync(SampleText, Filename);
            Assert.IsNotNull(storageFile);

            Assert.IsTrue(await StorageFileHelper.FileExistsAsync(localFolder, Filename));

            using (var stream = await StreamHelper.GetLocalFileStreamAsync(Filename))
            {
                var loadedText = await stream.ReadTextAsync(Encoding.UTF8);
                StringAssert.Contains(loadedText, SampleText);
            }

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_LocalCacheFolder()
        {
            var localCacheFolder = ApplicationData.Current.LocalCacheFolder;
            Assert.IsFalse(await StorageFileHelper.FileExistsAsync(localCacheFolder, Filename));

            var storageFile = await StorageFileHelper.WriteTextToLocalCacheFileAsync(SampleText, Filename);
            Assert.IsNotNull(storageFile);

            Assert.IsTrue(await StorageFileHelper.FileExistsAsync(localCacheFolder, Filename));

            using (var stream = await StreamHelper.GetLocalCacheFileStreamAsync(Filename))
            {
                var loadedText = await stream.ReadTextAsync(Encoding.UTF8);
                StringAssert.Contains(loadedText, SampleText);
            }

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_StreamHelper_KnownFolder()
        {
            var knownFolder = KnownFolders.PicturesLibrary;
            var folder = KnownFolderId.PicturesLibrary;

            Assert.IsFalse(await StorageFileHelper.FileExistsAsync(knownFolder, Filename));

            var storageFile = await StorageFileHelper.WriteTextToKnownFolderFileAsync(folder, SampleText, Filename);
            Assert.IsNotNull(storageFile);

            Assert.IsTrue(await StorageFileHelper.FileExistsAsync(knownFolder, Filename));

            using (var stream = await StreamHelper.GetKnowFoldersFileStreamAsync(folder, Filename))
            {
                var loadedText = await stream.ReadTextAsync(Encoding.UTF8);
                StringAssert.Contains(loadedText, SampleText);
            }

            await storageFile.DeleteAsync(StorageDeleteOption.Default);
        }
    }
}
