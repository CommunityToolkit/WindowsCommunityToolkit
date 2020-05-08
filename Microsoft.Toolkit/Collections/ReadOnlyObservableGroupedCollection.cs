// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Toolkit.Collections
{
    /// <summary>
    /// A read-only list of groups.
    /// </summary>
    /// <typeparam name="TKey">The type of the group key.</typeparam>
    /// <typeparam name = "TValue" > The type of the items in the collection.</typeparam>
    public sealed class ReadOnlyObservableGroupedCollection<TKey, TValue> : ReadOnlyObservableCollection<ReadOnlyObservableGroup<TKey, TValue>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyObservableGroupedCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="collection">The source collection to wrap.</param>
        public ReadOnlyObservableGroupedCollection(ObservableGroupedCollection<TKey, TValue> collection)
            : this(collection.Select(g => new ReadOnlyObservableGroup<TKey, TValue>(g)))
        {
            ((INotifyCollectionChanged)collection).CollectionChanged += this.OnSourceCollectionChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyObservableGroupedCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="collection">The initial data to add in the grouped collection.</param>
        public ReadOnlyObservableGroupedCollection(IEnumerable<ReadOnlyObservableGroup<TKey, TValue>> collection)
            : base(new ObservableCollection<ReadOnlyObservableGroup<TKey, TValue>>(collection))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyObservableGroupedCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="collection">The initial data to add in the grouped collection.</param>
        public ReadOnlyObservableGroupedCollection(IEnumerable<IGrouping<TKey, TValue>> collection)
            : this(collection.Select(g => new ReadOnlyObservableGroup<TKey, TValue>(g.Key, g)))
        {
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Even if the NotifyCollectionChangedEventArgs allows multiple items, the actual implementation is only
            // reporting the changes one by one. We consider only this case for now.
            if (e.OldItems?.Count > 1 || e.NewItems?.Count > 1)
            {
                Debug.Fail("OldItems and NewItems should contain at most 1 item");
                throw new NotSupportedException();
            }

            var newItem = e.NewItems?.Cast<ObservableGroup<TKey, TValue>>()?.FirstOrDefault();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Items.Insert(e.NewStartingIndex, new ReadOnlyObservableGroup<TKey, TValue>(newItem));
                    break;
                case NotifyCollectionChangedAction.Move:
                    // Our inner Items list is our own ObservableCollection<ReadOnlyObservableGroup<TKey, TValue>> so we can safely cast Items to its concrete type here.
                    ((ObservableCollection<ReadOnlyObservableGroup<TKey, TValue>>)Items).Move(e.OldStartingIndex, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Items.RemoveAt(e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Items[e.OldStartingIndex] = new ReadOnlyObservableGroup<TKey, TValue>(newItem);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Items.Clear();
                    break;
                default:
                    Debug.Fail("unsupported value");
                    break;
            }
        }
    }
}
