using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Cache
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
        }
    }
}
