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
using System.Runtime.Serialization;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Class DataItem
    /// </summary>
    public class DataItem
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="DataItem"/> class.
        /// </summary>
        /// <param name="item"> OneDrive Item</param>
        public DataItem(Microsoft.OneDrive.Sdk.Item item)
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

        /// <summary>
        ///  Initializes a new instance of the <see cref="DataItem"/> class.
        /// </summary>
        /// <param name="item"> Graph Item</param>
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

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public Createdby CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public DateTimeOffset? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public DateTimeOffset? LastModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or Sets the item id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets the item name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the size of the item
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// Gets or Sets the web url
        /// </summary>
        public string WebUrl { get; set; }

        /// <summary>
        /// Gets or Sets a reference the OneDrive item's parent
        /// </summary>
        public ParentReference ParentReference { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public object ContentDownloadUrl { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public string CTag { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public Folder Folder { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public File File { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        [JsonProperty("eTag")]
        public string ETag { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public object SpecialFolder { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public object ThumbnailUrl { get; set; }
    }

    /// <summary>
    /// Class Createdby
    /// </summary>
    public class Createdby
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="DataItem"/> class.
        /// </summary>
        public Createdby() => User = new User();

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public object Application { get; set; }
    }

    /// <summary>
    /// Class User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or Sets
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public string DisplayName { get; set; }
    }

    /// <summary>
    /// Class ParentReference
    /// </summary>
    public class ParentReference
    {
        /// <summary>
        /// Gets or Sets
        /// </summary>
        public string DriveId { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// Class Folder
    /// </summary>
    public class Folder
    {
        /// <summary>
        /// Gets or Sets
        /// </summary>
        public Folder()
        {
            ChildCount = 0;
        }

        /// <summary>
        /// Gets or Sets the number of Folder's children
        /// </summary>
        [JsonProperty("childCount")]
        public int? ChildCount { get; set; }
    }

    /// <summary>
    /// Class Folder
    /// </summary>
    public class File
    {

        public File()
        {
            Hashes = new Hashes();
        }

        /// <summary>
        /// Gets or Sets
        /// </summary>
        public Hashes Hashes { get; set; }
    }

    /// <summary>
    /// Class Folder
    /// </summary>
    public class Hashes
    {
        /// <summary>
        /// Gets or Sets
        /// </summary>
        public string quickXorHash { get; set; }
    }
}
