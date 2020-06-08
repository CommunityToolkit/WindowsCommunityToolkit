// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Toolkit.Collections
{
    /// <summary>
    /// An observable group.
    /// It associates a <see cref="Key"/> to an <see cref="ObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the group key.</typeparam>
    /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
    [DebuggerDisplay("Key = {Key}, Count = {Count}")]
    public sealed class ObservableGroup<TKey, TValue> : ObservableCollection<TValue>, IGrouping<TKey, TValue>, IReadOnlyObservableGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableGroup{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="key">The key for the group.</param>
        public ObservableGroup(TKey key)
        {
            Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableGroup{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="grouping">The grouping to fill the group.</param>
        public ObservableGroup(IGrouping<TKey, TValue> grouping)
            : base(grouping)
        {
            Key = grouping.Key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableGroup{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="key">The key for the group.</param>
        /// <param name="collection">The initial collection of data to add to the group.</param>
        public ObservableGroup(TKey key, IEnumerable<TValue> collection)
            : base(collection)
        {
            Key = key;
        }

        /// <summary>
        /// Gets the key of the group.
        /// </summary>
        public TKey Key { get; }

        /// <inheritdoc/>
        object IReadOnlyObservableGroup.Key => Key;
    }
}
