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

using Microsoft.Toolkit.Services.OneDrive;
using Microsoft.Toolkit.Services.OneDrive.Platform;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive.Platform
{
    /// <summary>
    /// Platform implementation of initializer.
    /// </summary>
    public class OneDriveServicePlatformInitializer : IOneDriveServicePlatformInitializer
    {
        /// <summary>
        /// Creates an instance of platform-specific implementation.
        /// </summary>
        /// <param name="oneDriveService">Instance of the service.</param>
        /// <returns>Returns concrete type.</returns>
        public IOneDriveServicePlatform CreateOneDriveServicePlatformInstance(Toolkit.Services.OneDrive.OneDriveService oneDriveService)
        {
            return new OneDriveServicePlatform(oneDriveService);
        }

        /// <summary>
        /// Creates an instance of platform-specific implementation.
        /// </summary>
        /// <param name="oneDriveService">Instance of the service.</param>
        /// <param name="oneDriveStorageFile">Instance of the storage file.</param>
        /// <returns>Returns concrete type.</returns>
        public IOneDriveStorageFilePlatform CreateOneDriveStorageFilePlatformInstance(Toolkit.Services.OneDrive.OneDriveService oneDriveService, Toolkit.Services.OneDrive.OneDriveStorageFile oneDriveStorageFile)
        {
            return new OneDriveStorageFilePlatform(oneDriveService, oneDriveStorageFile);
        }

        /// <summary>
        /// Creates an instance of platform-specific implementation.
        /// </summary>
        /// <param name="oneDriveService">Instance of the service.</param>
        /// <param name="oneDriveStorageFolder">Instance of the storage folder.</param>
        /// <returns>Returns concrete type.</returns>
        public IOneDriveStorageFolderPlatform CreateOneDriveStorageFolderPlatformInstance(Toolkit.Services.OneDrive.OneDriveService oneDriveService, Toolkit.Services.OneDrive.OneDriveStorageFolder oneDriveStorageFolder)
        {
            return new OneDriveStorageFolderPlatform(oneDriveService, oneDriveStorageFolder);
        }

        /// <summary>
        /// Creates an instance of platform-specific implementation.
        /// </summary>
        /// <param name="oneDriveService">Instance of the service.</param>
        /// <param name="oneDriveStorageItem">Instance of the storage item.</param>
        /// <returns>Returns concrete type.</returns>
        public IOneDriveStorageItemPlatform CreateOneDriveStorageItemPlatformInstance(Toolkit.Services.OneDrive.OneDriveService oneDriveService, Toolkit.Services.OneDrive.OneDriveStorageItem oneDriveStorageItem)
        {
            return new OneDriveStorageItemPlatform(oneDriveService, oneDriveStorageItem);
        }
    }
}
