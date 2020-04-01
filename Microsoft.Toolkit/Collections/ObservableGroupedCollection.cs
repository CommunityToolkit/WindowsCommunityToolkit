// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.Toolkit.Collections
{
    /// <summary>
    /// An observable list of observable groups.
    /// </summary>
    /// <typeparam name="TKey">The type of the group key.</typeparam>
    /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
    public sealed class ObservableGroupedCollection<TKey, TValue> : ObservableCollection<ObservableGroup<TKey, TValue>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableGroupedCollection{TKey, TValue}"/> class.
        /// </summary>
        public ObservableGroupedCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableGroupedCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="collection">The initial data to add in the grouped collection.</param>
        public ObservableGroupedCollection(IEnumerable<IGrouping<TKey, TValue>> collection)
            : base(collection.Select(c => new ObservableGroup<TKey, TValue>(c)))
        {
        }
    }
}
