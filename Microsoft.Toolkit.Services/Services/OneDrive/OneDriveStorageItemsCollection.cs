// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    ///  Class OneDriveStorageItemsCollection
    /// </summary>
    public class OneDriveStorageItemsCollection : IReadOnlyList<OneDriveStorageItem>
    {
        private List<OneDriveStorageItem> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageItemsCollection"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="items">Items's list to store in the collection</param>
        public OneDriveStorageItemsCollection(List<OneDriveStorageItem> items)
        {
            _items = items;
        }

        /// <summary>
        /// Gets the element at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the collection. </returns>
        public OneDriveStorageItem this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        /// <summary>
        /// Gets the number of items in the collection
        /// </summary>
        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<OneDriveStorageItem> GetEnumerator()
        {
            return new OneDriveStorageItemsEnumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new OneDriveStorageItemsEnumerator(_items);
        }
    }
}
