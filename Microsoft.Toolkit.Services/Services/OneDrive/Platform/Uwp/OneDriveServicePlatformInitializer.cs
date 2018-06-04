// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.OneDrive.Uwp
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
