using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.OneDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.Services.OneDrive
{
    public interface IOneDriveStorageItem
    {
        IRandomAccessStream ThumbNail { get; }

        DateTimeOffset? DateCreated { get;  }

        string DisplayName { get; }

        string DisplayType { get; }

        string FolderRelativeId { get; }

        string Name { get; }

        string Path { get; }
        
        bool IsFile();

        bool IsOneNote();

        bool IsFolder();

        IBaseClient Provider { get; set; }

        IBaseRequestBuilder RequestBuilder { get; }

        Task<bool> CopyAsync(OneDriveStorageFolder destinationFolder, string desiredNewName = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<IOneDriveStorageItem> RenameAsync(string desiredName, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> MoveAsync(OneDriveStorageFolder destinationFolder, string desiredNewName = null, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken));

    }
}
