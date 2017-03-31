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

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Generic InMemoryStorageItem holds items for InMemoryStorage.
    /// </summary>
    /// <typeparam name="T">Type is set by consuming cache</typeparam>
    public class InMemoryStorageItem<T>
    {
        /// <summary>
        /// Gets the item identifier
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the item created timestamp.
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Gets the item last updated timestamp.
        /// </summary>
        public DateTime LastUpdated { get; private set; }

        /// <summary>
        /// Gets the item being stored.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryStorageItem{T}"/> class.
        /// Constructor for InMemoryStorageItem
        /// </summary>
        /// <param name="id">uniquely identifies the item</param>
        /// <param name="lastUpdated">last updated timestamp</param>
        /// <param name="item">the item being stored</param>
        public InMemoryStorageItem(string id, DateTime lastUpdated, T item)
        {
            Id = id;
            LastUpdated = lastUpdated;
            Item = item;
            Created = DateTime.Now;
        }
    }
}
