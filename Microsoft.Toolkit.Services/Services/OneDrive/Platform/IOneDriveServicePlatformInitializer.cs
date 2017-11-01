namespace Microsoft.Toolkit.Services.OneDrive.Platform
{
    /// <summary>
    /// Platform abstraction.
    /// </summary>
    public interface IOneDriveServicePlatformInitializer
    {
        /// <summary>
        /// Creates an instance of platform-specific implementation.
        /// </summary>
        /// <param name="oneDriveService">Instance of the service.</param>
        /// <returns>Returns concrete type.</returns>
        IOneDriveServicePlatform CreateOneDriveServicePlatformInstance(
            Toolkit.Services.OneDrive.OneDriveService oneDriveService);

        /// <summary>
        /// Creates an instance of platform-specific implementation.
        /// </summary>
        /// <param name="oneDriveService">Instance of the service.</param>
        /// <param name="oneDriveStorageFile">Instance of the storage file.</param>
        /// <returns>Returns concrete type.</returns>
        IOneDriveStorageFilePlatform CreateOneDriveStorageFilePlatformInstance(
            Toolkit.Services.OneDrive.OneDriveService oneDriveService,
            Toolkit.Services.OneDrive.OneDriveStorageFile oneDriveStorageFile);

        /// <summary>
        /// Creates an instance of platform-specific implementation.
        /// </summary>
        /// <param name="oneDriveService">Instance of the service.</param>
        /// <param name="oneDriveStorageFolder">Instance of the storage folder.</param>
        /// <returns>Returns concrete type.</returns>
        IOneDriveStorageFolderPlatform CreateOneDriveStorageFolderPlatformInstance(
            Toolkit.Services.OneDrive.OneDriveService oneDriveService,
            Toolkit.Services.OneDrive.OneDriveStorageFolder oneDriveStorageFolder);

        /// <summary>
        /// Creates an instance of platform-specific implementation.
        /// </summary>
        /// <param name="oneDriveService">Instance of the service.</param>
        /// <param name="oneDriveStorageItem">Instance of the storage item.</param>
        /// <returns>Returns concrete type.</returns>
        IOneDriveStorageItemPlatform CreateOneDriveStorageItemPlatformInstance(
            Toolkit.Services.OneDrive.OneDriveService oneDriveService,
            Toolkit.Services.OneDrive.OneDriveStorageItem oneDriveStorageItem);
    }
}
