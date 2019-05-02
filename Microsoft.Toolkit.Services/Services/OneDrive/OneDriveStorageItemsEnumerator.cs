// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    ///  Class OneDriveStorageItemsEnumerator
    /// </summary>
    public class OneDriveStorageItemsEnumerator : IEnumerator<OneDriveStorageItem>
    {
        private List<OneDriveStorageItem> _items;
        private int position = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageItemsEnumerator"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="items">Items's list to store in the collection</param>
        public OneDriveStorageItemsEnumerator(List<OneDriveStorageItem> items)
        {
            _items = items;
        }

        /// <summary>
        /// Gets the current OneDrive Item
        /// </summary>
        public OneDriveStorageItem Current
        {
            get
            {
                return CurrentItem;
            }
        }

        /// <summary>
        /// Gets the current OneDrive Item
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return CurrentItem;
            }
        }

        /// <summary>
        /// Gets the current OneDrive Item
        /// </summary>
        public OneDriveStorageItem CurrentItem
        {
            get
            {
                try
                {
                    var currentItem = _items[position];
                    if (currentItem.IsFile() || currentItem.IsOneNote())
                    {
                        return new OneDriveStorageFile(currentItem.Provider, currentItem.RequestBuilder, currentItem.OneDriveItem);
                    }

                    return new OneDriveStorageFolder(currentItem.Provider, currentItem.RequestBuilder, currentItem.OneDriveItem);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Clear list of items
        /// </summary>
        public void Dispose()
        {
            _items.Clear();
        }

        /// <summary>
        /// Move to the nex position in the list
        /// </summary>
        /// <returns>Success of failure</returns>
        public bool MoveNext()
        {
            position++;
            return position < _items.Count;
        }

        /// <summary>
        /// Reset the item position in the list
        /// </summary>
        public void Reset()
        {
            position = -1;
        }
    }
}
