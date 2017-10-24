﻿// ******************************************************************
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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Services.OneDrive;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  Class OneDriveStorageItemsCollection
    /// </summary>
    public class OneDriveStorageItemsCollection : IReadOnlyList<IOneDriveStorageItem>
    {
        private List<IOneDriveStorageItem> _items;

        private bool _useOneDriveSdk = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveStorageItemsCollection"/> class.
        /// <para>Permissions : Have full access to user files and files shared with user</para>
        /// </summary>
        /// <param name="items">Items's list to store in the collection</param>
        /// <param name="useOneDriveSdk">flag which indicate if we use the OneDrive SDK or the MS Graph SDK</param>
        public OneDriveStorageItemsCollection(List<IOneDriveStorageItem> items, bool useOneDriveSdk = true)
        {
            _items = items;
            _useOneDriveSdk = useOneDriveSdk;
        }

        /// <summary>
        /// Gets the element at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the collection. </returns>
        public IOneDriveStorageItem this[int index]
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
        public IEnumerator<IOneDriveStorageItem> GetEnumerator()
        {
            return new OneDriveStorageItemsEnumerator(_items, _useOneDriveSdk);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new OneDriveStorageItemsEnumerator(_items, _useOneDriveSdk);
        }
    }
}
