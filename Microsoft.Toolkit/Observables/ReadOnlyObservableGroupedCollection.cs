// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Toolkit.Observables.Collections
{
    /// <summary>
    /// A read-only list of groups.
    /// </summary>
    /// <typeparam name="TKey">The type of the group key.</typeparam>
    /// <typeparam name = "TValue" > The type of the items in the collection.</typeparam>
    public sealed class ReadOnlyObservableGroupedCollection<TKey, TValue> :
        IReadOnlyCollection<ReadOnlyObservableGroup<TKey, TValue>>,
        IReadOnlyList<ReadOnlyObservableGroup<TKey, TValue>>,
        INotifyPropertyChanged,
        INotifyCollectionChanged,
        ICollection,    // Implementing IColletion and IList is needed to allow ListView to monitor the INotifyCollectionChanged events...
        IList
    {
        private readonly ObservableGroupedCollection<TKey, TValue> _collection;
        private readonly IDictionary<ObservableGroup<TKey, TValue>, ReadOnlyObservableGroup<TKey, TValue>> _mapping;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyObservableGroupedCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="collection">The source collection to wrap.</param>
        public ReadOnlyObservableGroupedCollection(ObservableGroupedCollection<TKey, TValue> collection)
        {
            _collection = collection;
            _mapping = new Dictionary<ObservableGroup<TKey, TValue>, ReadOnlyObservableGroup<TKey, TValue>>(capacity: _collection.Count);

            ((INotifyPropertyChanged)_collection).PropertyChanged += OnCollectionPropertyChanged;
            ((INotifyCollectionChanged)_collection).CollectionChanged += OnCollectionChanged;
        }

        /// <inheritdoc/>
        public ReadOnlyObservableGroup<TKey, TValue> this[int index] => GetGroupAt(index);

        /// <inheritdoc/>
        public int Count => _collection.Count;

        /// <inheritdoc/>
        public IEnumerator<ReadOnlyObservableGroup<TKey, TValue>> GetEnumerator() => _collection.Select(CreateOrGetReadOnlyObservableGroup).GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection.Select(CreateOrGetReadOnlyObservableGroup)).GetEnumerator();

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // We force the evaluation to have all our instances ready before deleting the mapping.
            var sourceOldItems = (e.OldItems?.Cast<ObservableGroup<TKey, TValue>>() ?? Enumerable.Empty<ObservableGroup<TKey, TValue>>()).ToList();
            var oldItems = (IList)sourceOldItems.Select(CreateOrGetReadOnlyObservableGroup).ToList();
            var newItems = (IList)(e.NewItems?.Cast<ObservableGroup<TKey, TValue>>().Select(CreateOrGetReadOnlyObservableGroup) ?? Enumerable.Empty<ReadOnlyObservableGroup<TKey, TValue>>()).ToList();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, e.NewStartingIndex));
                    break;
                case NotifyCollectionChangedAction.Move:
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, newItems, e.NewStartingIndex, e.OldStartingIndex));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // We unmap the removed or replaced items.
                    foreach (var sourceOldItem in sourceOldItems)
                    {
                        _mapping.Remove(sourceOldItem);
                    }

                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems, e.OldStartingIndex));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // We unmap the removed or replaced items.
                    foreach (var sourceOldItem in sourceOldItems)
                    {
                        _mapping.Remove(sourceOldItem);
                    }

                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _mapping.Clear();
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    break;
                default:
                    Debug.Fail("unsupported value");
                    break;
            }
        }

        private void OnCollectionPropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        private ReadOnlyObservableGroup<TKey, TValue> CreateOrGetReadOnlyObservableGroup(ObservableGroup<TKey, TValue> observableGroup)
        {
            if (_mapping.TryGetValue(observableGroup, out var readOnlyGroup))
            {
                return readOnlyGroup;
            }

            readOnlyGroup = new ReadOnlyObservableGroup<TKey, TValue>(observableGroup);
            _mapping.Add(observableGroup, readOnlyGroup);

            return readOnlyGroup;
        }

        private ReadOnlyObservableGroup<TKey, TValue> GetGroupAt(int index) => CreateOrGetReadOnlyObservableGroup(_collection[index]);

        int ICollection.Count => _collection.Count;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        bool IList.IsFixedSize => ((IList)_collection).IsFixedSize;

        bool IList.IsReadOnly => true;

        object IList.this[int index]
        {
            get => GetGroupAt(index);
            set => throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index) => throw new NotImplementedException();

        int IList.Add(object value) => throw new NotImplementedException();

        void IList.Clear() => throw new NotImplementedException();

        bool IList.Contains(object value) => this.Any(group => group == value);

        int IList.IndexOf(object value) => this.Select((group, i) => (group == value) ? i : -1).Max();

        void IList.Insert(int index, object value) => throw new NotImplementedException();

        void IList.Remove(object value) => throw new NotImplementedException();

        void IList.RemoveAt(int index) => throw new NotImplementedException();
    }
}
