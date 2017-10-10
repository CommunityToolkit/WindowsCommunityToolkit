using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{

    public class DataItem
    {
        static DataItem CreateItemFromOneDriveSdk(Microsoft.OneDrive.Sdk.Item item)
        {
            return new DataItem(item);
        }

        public DataItem (Microsoft.OneDrive.Sdk.Item item)
        {
            CreatedBy = new Createdby();
            if (item.CreatedBy != null)
            {
                CreatedBy.User.Id = item.CreatedBy.User.Id;
                if (item.CreatedBy.User != null)
                {
                    CreatedBy.User.DisplayName = item.CreatedBy.User.DisplayName;
                }
            }

            CreatedDateTime = item.CreatedDateTime;
            Id = item.Id;
            Name = item.Name;
            Size = item.Size;
            WebUrl = item.WebUrl;
            ParentReference = new ParentReference();
            ParentReference.DriveId = item.ParentReference.DriveId;
            ParentReference.Id = item.ParentReference.Id;
            ParentReference.Path = item.ParentReference.Path;
            CTag = item.CTag;
            if (item.Folder != null )
            {
                Folder = new Folder();
                Folder.ChildCount = item.Folder.ChildCount;
            }
            else if (item.File != null)
            {
                File = new File();
                File.Hashes.quickXorHash = item.File.Hashes.Crc32Hash;
            }

            LastModifiedDateTime = item.LastModifiedDateTime;
        }

        public DataItem(DriveItem item)
        {
            CreatedBy = new Createdby();
            if (item.CreatedBy != null)
            {
                CreatedBy.User.Id = item.CreatedBy.User.Id;
                if (item.CreatedBy.User != null)
                {
                    CreatedBy.User.DisplayName = item.CreatedBy.User.DisplayName;
                }
            }


            CreatedDateTime = item.CreatedDateTime;
            Id = item.Id;
            Name = item.Name;
            Size = item.Size;
            WebUrl = item.WebUrl;
            ParentReference = new ParentReference();
            ParentReference.DriveId = item.ParentReference.DriveId;
            ParentReference.Id = item.ParentReference.Id;
            ParentReference.Path = item.ParentReference.Path;
            CTag = item.CTag;
            if (item.Folder != null)
            {
                Folder = new Folder();
                Folder.ChildCount = item.Folder.ChildCount;
            }
            else if (item.File != null)
            {
                File = new File();
                File.Hashes.quickXorHash = item.File.Hashes.Crc32Hash;
            }

            LastModifiedDateTime = item.LastModifiedDateTime;
        }

        public Createdby CreatedBy { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? LastModifiedDateTime { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public long? Size { get; set; }
        public string WebUrl { get; set; }
        public ParentReference ParentReference { get; set; }
        public object ContentDownloadUrl { get; set; }
        public string CTag { get; set; }
        public Folder Folder { get; set; }
        public File File { get; set; }
        public string eTag { get; set; }
        public object SpecialFolder { get; set; }
        public object ThumbnailUrl { get; set; }
    }

    public class Createdby
    {
        public Createdby()
        {
            User = new User();
        }
        public User User { get; set; }
        public object Application { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }

    public class ParentReference
    {
        public string DriveId { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }
    }

    public class Folder
    {
        public int? ChildCount { get; set; }
    }

    public class File
    {
        public File()
        {
            Hashes = new Hashes();
        }
        public Hashes Hashes { get; set; }
    }

    public class Hashes
    {
        public string quickXorHash { get; set; }
    }

}
