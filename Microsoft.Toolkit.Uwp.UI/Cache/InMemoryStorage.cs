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
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Generic in-memory storage of items
    /// </summary>
    /// <typeparam name="T">T defines the type of item stored</typeparam>
    public class InMemoryStorage<T>
    {
        private int _maxItemCount;
        private OrderedDictionary _inMemoryStorage = new OrderedDictionary();

        /// <summary>
        /// Gets or sets the maximum count of Items that can be stored in this InMemoryStorage instance.
        /// </summary>
        public int MaxItemCount
        {
            get
            {
                return _maxItemCount;
            }

            set
            {
                if (_maxItemCount == value)
                {
                    return;
                }

                _maxItemCount = value;

                lock (this)
                {
                    EnsureStorageBounds(value);
                }
            }
        }

        /// <summary>
        /// Clears all items stored in memory
        /// </summary>
        public void Clear()
        {
            lock (this)
            {
                _inMemoryStorage.Clear();
            }
        }

        /// <summary>
        /// Clears items stored in memory based on duration passed
        /// </summary>
        /// <param name="duration">TimeSpan to identify expired items</param>
        public void Clear(TimeSpan duration)
        {
            lock (this)
            {
                DateTime expirationDate = DateTime.Now.Subtract(duration);

                // clears expired items in in-memory cache
                var keysToDelete = new List<object>();

                foreach (var k in _inMemoryStorage.Keys)
                {
                    if (((InMemoryStorageItem<T>)_inMemoryStorage[k]).LastUpdated <= expirationDate)
                    {
                        keysToDelete.Add(k);
                    }
                }

                foreach (var key in keysToDelete)
                {
                    _inMemoryStorage.Remove(key);
                }
            }
        }

        /// <summary>
        /// Add new item to in-memory storage
        /// </summary>
        /// <param name="item">item to be stored</param>
        public void SetItem(InMemoryStorageItem<T> item)
        {
            lock (this)
            {
                if (MaxItemCount == 0)
                {
                    return;
                }

                _inMemoryStorage[item.Id] = item;

                // ensure max limit is maintained. trim older entries first
                while (_inMemoryStorage.Count > MaxItemCount)
                {
                    _inMemoryStorage.RemoveAt(0);
                }
               }
        }

        /// <summary>
        /// Get item from in-memory storage as long as it has not ex
        /// </summary>
        /// <param name="id">id of the in-memory storage item</param>
        /// <param name="duration">timespan denoting expiration</param>
        /// <returns>Valid item if not out of date or return null if out of date or item does not exist</returns>
        public InMemoryStorageItem<T> GetItem(string id, TimeSpan duration)
        {
            lock (this)
            {
                object key = (object)id;
                object entry = _inMemoryStorage[key];

                if (entry == null)
                {
                    return null;
                }

                var item = (InMemoryStorageItem<T>)entry;

                DateTime expirationDate = DateTime.Now.Subtract(duration);

                if (item.LastUpdated > expirationDate)
                {
                    return item;
                }

                _inMemoryStorage.Remove(key);

                return null;
            }
        }

        private void EnsureStorageBounds(int maxCount)
        {
            lock (this)
            {
                if (_inMemoryStorage.Count == 0)
                {
                    return;
                }

                if (maxCount == 0)
                {
                    _inMemoryStorage.Clear();
                    return;
                }

                while (_inMemoryStorage.Count > maxCount)
                {
                    _inMemoryStorage.RemoveAt(0);
                }
            }
        }
    }
}
