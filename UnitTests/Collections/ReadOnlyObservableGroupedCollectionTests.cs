// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FluentAssertions;
using Microsoft.Toolkit.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace UnitTests.Collections
{
    [TestClass]
    public class ReadOnlyObservableGroupedCollectionTests
    {
        [TestCategory("Observables")]
        [TestMethod]
        public void Ctor_WithEmptySource_ShoudInitializeObject()
        {
            var source = new ObservableGroupedCollection<string, int>();
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);

            readOnlyGroup.Should().BeEmpty();
            readOnlyGroup.Count.Should().Be(0);
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void Ctor_WithSource_ShoudInitializeObject()
        {
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", new[] { 1, 3, 5 }),
                new IntGroup("B", new[] { 2, 4, 6 }),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);

            readOnlyGroup.Should().HaveCount(2);
            readOnlyGroup.Count.Should().Be(2);
            readOnlyGroup.ElementAt(0).Key.Should().Be("A");
            readOnlyGroup.ElementAt(0).Should().BeEquivalentTo(new[] { 1, 3, 5 }, o => o.WithoutStrictOrdering());
            readOnlyGroup.ElementAt(1).Key.Should().Be("B");
            readOnlyGroup.ElementAt(1).Should().BeEquivalentTo(new[] { 2, 4, 6 }, o => o.WithoutStrictOrdering());
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void IListImplementation_Properties_ShoudReturnExpectedValues()
        {
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", new[] { 1, 3, 5 }),
                new IntGroup("B", new[] { 2, 4, 6 }),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            var list = (IList)readOnlyGroup;

            list.Count.Should().Be(2);
            var group0 = (ReadOnlyObservableGroup<string, int>)list[0];
            group0.Key.Should().Be("A");
            group0.Should().BeEquivalentTo(new[] { 1, 3, 5 }, o => o.WithoutStrictOrdering());
            var group1 = (ReadOnlyObservableGroup<string, int>)list[1];
            group1.Key.Should().Be("B");
            group1.Should().BeEquivalentTo(new[] { 2, 4, 6 }, o => o.WithoutStrictOrdering());

            list.SyncRoot.Should().BeSameAs(readOnlyGroup);
            list.IsFixedSize.Should().BeFalse();
            list.IsReadOnly.Should().BeTrue();
            list.IsSynchronized.Should().BeFalse();
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void IListImplementation_MutableMethods_ShoudThrow()
        {
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", new[] { 1, 3, 5 }),
                new IntGroup("B", new[] { 2, 4, 6 }),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            var list = (IList)readOnlyGroup;

            var testGroup = new ReadOnlyObservableGroup<string, int>("test", new ObservableCollection<int>());
            Action add = () => list.Add(testGroup);
            add.Should().Throw<NotImplementedException>();

            Action clear = () => list.Clear();
            clear.Should().Throw<NotImplementedException>();

            Action insert = () => list.Insert(2, testGroup);
            insert.Should().Throw<NotImplementedException>();

            Action remove = () => list.Remove(testGroup);
            remove.Should().Throw<NotImplementedException>();

            Action removeAt = () => list.RemoveAt(2);
            removeAt.Should().Throw<NotImplementedException>();

            Action set = () => list[2] = testGroup;
            set.Should().Throw<NotImplementedException>();

            var array = new object[5];
            Action copyTo = () => list.CopyTo(array, 0);
            copyTo.Should().Throw<NotImplementedException>();
        }

        [TestCategory("Observables")]
        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        public void IListImplementation_IndexOf_ShoudReturnExpectedValue(int groupIndex)
        {
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", new[] { 1, 3, 5 }),
                new IntGroup("B", new[] { 2, 4, 6 }),
                new IntGroup("C", new[] { 7, 8, 9 }),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            var list = (IList)readOnlyGroup;

            var groupToSearch = groupIndex >= 0 ? list[groupIndex] : null;

            var index = list.IndexOf(groupToSearch);

            index.Should().Be(groupIndex);
        }

        [TestCategory("Observables")]
        [DataTestMethod]
        [DataRow(-1, false)]
        [DataRow(0, true)]
        [DataRow(1, true)]
        public void IListImplementation_Contains_ShoudReturnExpectedValue(int groupIndex, bool expectedResult)
        {
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", new[] { 1, 3, 5 }),
                new IntGroup("B", new[] { 2, 4, 6 }),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            var list = (IList)readOnlyGroup;

            var groupToSearch = groupIndex >= 0 ? list[groupIndex] : null;

            var result = list.Contains(groupToSearch);

            result.Should().Be(expectedResult);
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void AddGroupInSource_ShouldAddGroup()
        {
            NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;
            var isCountPropertyChangedEventRaised = false;
            var itemsList = new[] { 1, 2, 3 };
            var source = new ObservableGroupedCollection<string, int>();
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            ((INotifyCollectionChanged)readOnlyGroup).CollectionChanged += (s, e) => collectionChangedEventArgs = e;
            ((INotifyPropertyChanged)readOnlyGroup).PropertyChanged += (s, e) => isCountPropertyChangedEventRaised = isCountPropertyChangedEventRaised || e.PropertyName == nameof(readOnlyGroup.Count);

            source.Add(new ObservableGroup<string, int>("Add", itemsList));

            readOnlyGroup.Should().ContainSingle();
            readOnlyGroup.Count.Should().Be(1);
            readOnlyGroup.ElementAt(0).Key.Should().Be("Add");
            readOnlyGroup.ElementAt(0).Should().BeEquivalentTo(itemsList, o => o.WithoutStrictOrdering());

            isCountPropertyChangedEventRaised.Should().BeTrue();
            collectionChangedEventArgs.Should().NotBeNull();
            IsAddEventValid(collectionChangedEventArgs, itemsList).Should().BeTrue();
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void RemoveGroupInSource_ShoudRemoveGroup()
        {
            NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;
            var isCountPropertyChangedEventRaised = false;
            var aItemsList = new[] { 1, 2, 3 };
            var bItemsList = new[] { 2, 4, 6 };
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", aItemsList),
                new IntGroup("B", bItemsList),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            ((INotifyCollectionChanged)readOnlyGroup).CollectionChanged += (s, e) => collectionChangedEventArgs = e;
            ((INotifyPropertyChanged)readOnlyGroup).PropertyChanged += (s, e) => isCountPropertyChangedEventRaised = isCountPropertyChangedEventRaised || e.PropertyName == nameof(readOnlyGroup.Count);

            source.RemoveAt(1);

            readOnlyGroup.Should().ContainSingle();
            readOnlyGroup.Count.Should().Be(1);
            readOnlyGroup.ElementAt(0).Key.Should().Be("A");
            readOnlyGroup.ElementAt(0).Should().BeEquivalentTo(aItemsList, o => o.WithoutStrictOrdering());

            isCountPropertyChangedEventRaised.Should().BeTrue();
            collectionChangedEventArgs.Should().NotBeNull();
            IsRemoveEventValid(collectionChangedEventArgs, bItemsList, 1).Should().BeTrue();
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void MoveGroupInSource_ShoudMoveGroup()
        {
            NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;
            var isCountPropertyChangedEventRaised = false;
            var aItemsList = new[] { 1, 2, 3 };
            var bItemsList = new[] { 2, 4, 6 };
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", aItemsList),
                new IntGroup("B", bItemsList),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            ((INotifyCollectionChanged)readOnlyGroup).CollectionChanged += (s, e) => collectionChangedEventArgs = e;
            ((INotifyPropertyChanged)readOnlyGroup).PropertyChanged += (s, e) => isCountPropertyChangedEventRaised = isCountPropertyChangedEventRaised || e.PropertyName == nameof(readOnlyGroup.Count);

            source.Move(1, 0);

            readOnlyGroup.Should().HaveCount(2);
            readOnlyGroup.Count.Should().Be(2);
            readOnlyGroup.ElementAt(0).Key.Should().Be("B");
            readOnlyGroup.ElementAt(0).Should().BeEquivalentTo(bItemsList, o => o.WithoutStrictOrdering());
            readOnlyGroup.ElementAt(1).Key.Should().Be("A");
            readOnlyGroup.ElementAt(1).Should().BeEquivalentTo(aItemsList, o => o.WithoutStrictOrdering());

            isCountPropertyChangedEventRaised.Should().BeFalse();
            collectionChangedEventArgs.Should().NotBeNull();
            IsMoveEventValid(collectionChangedEventArgs, bItemsList, 1, 0).Should().BeTrue();
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void ClearSource_ShoudClear()
        {
            NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;
            var isCountPropertyChangedEventRaised = false;
            var aItemsList = new[] { 1, 2, 3 };
            var bItemsList = new[] { 2, 4, 6 };
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", aItemsList),
                new IntGroup("B", bItemsList),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            ((INotifyCollectionChanged)readOnlyGroup).CollectionChanged += (s, e) => collectionChangedEventArgs = e;
            ((INotifyPropertyChanged)readOnlyGroup).PropertyChanged += (s, e) => isCountPropertyChangedEventRaised = isCountPropertyChangedEventRaised || e.PropertyName == nameof(readOnlyGroup.Count);

            source.Clear();

            readOnlyGroup.Should().BeEmpty();
            readOnlyGroup.Count.Should().Be(0);

            isCountPropertyChangedEventRaised.Should().BeTrue();
            collectionChangedEventArgs.Should().NotBeNull();
            IsResetEventValid(collectionChangedEventArgs).Should().BeTrue();
        }

        [TestCategory("Observables")]
        [TestMethod]
        public void ReplaceGroupInSource_ShoudReplaceGroup()
        {
            NotifyCollectionChangedEventArgs collectionChangedEventArgs = null;
            var isCountPropertyChangedEventRaised = false;
            var aItemsList = new[] { 1, 2, 3 };
            var bItemsList = new[] { 2, 4, 6 };
            var cItemsList = new[] { 7, 8, 9 };
            var groups = new List<IGrouping<string, int>>
            {
                new IntGroup("A", aItemsList),
                new IntGroup("B", bItemsList),
            };
            var source = new ObservableGroupedCollection<string, int>(groups);
            var readOnlyGroup = new ReadOnlyObservableGroupedCollection<string, int>(source);
            ((INotifyCollectionChanged)readOnlyGroup).CollectionChanged += (s, e) => collectionChangedEventArgs = e;
            ((INotifyPropertyChanged)readOnlyGroup).PropertyChanged += (s, e) => isCountPropertyChangedEventRaised = isCountPropertyChangedEventRaised || e.PropertyName == nameof(readOnlyGroup.Count);

            source[0] = new ObservableGroup<string, int>("C", cItemsList);

            readOnlyGroup.Should().HaveCount(2);
            readOnlyGroup.Count.Should().Be(2);
            readOnlyGroup.ElementAt(0).Key.Should().Be("C");
            readOnlyGroup.ElementAt(0).Should().BeEquivalentTo(cItemsList, o => o.WithoutStrictOrdering());
            readOnlyGroup.ElementAt(1).Key.Should().Be("B");
            readOnlyGroup.ElementAt(1).Should().BeEquivalentTo(bItemsList, o => o.WithoutStrictOrdering());

            isCountPropertyChangedEventRaised.Should().BeFalse();
            collectionChangedEventArgs.Should().NotBeNull();
            IsReplaceEventValid(collectionChangedEventArgs, aItemsList, cItemsList).Should().BeTrue();
        }

        private static bool IsAddEventValid(NotifyCollectionChangedEventArgs args, IEnumerable<int> expectedGroupItems)
        {
            var newItems = args.NewItems?.Cast<IEnumerable<int>>();
            return args.Action == NotifyCollectionChangedAction.Add &&
                    args.OldItems == null &&
                    newItems?.Count() == 1 &&
                    Enumerable.SequenceEqual(newItems.ElementAt(0), expectedGroupItems);
        }

        private static bool IsRemoveEventValid(NotifyCollectionChangedEventArgs args, IEnumerable<int> expectedGroupItems, int oldIndex)
        {
            var oldItems = args.OldItems?.Cast<IEnumerable<int>>();
            return args.Action == NotifyCollectionChangedAction.Remove &&
                    args.NewItems == null &&
                    args.OldStartingIndex == oldIndex &&
                    oldItems?.Count() == 1 &&
                    Enumerable.SequenceEqual(oldItems.ElementAt(0), expectedGroupItems);
        }

        private static bool IsMoveEventValid(NotifyCollectionChangedEventArgs args, IEnumerable<int> expectedGroupItems, int oldIndex, int newIndex)
        {
            var oldItems = args.OldItems?.Cast<IEnumerable<int>>();
            var newItems = args.NewItems?.Cast<IEnumerable<int>>();
            return args.Action == NotifyCollectionChangedAction.Move &&
                    args.OldStartingIndex == oldIndex &&
                    args.NewStartingIndex == newIndex &&
                    oldItems?.Count() == 1 &&
                    Enumerable.SequenceEqual(oldItems.ElementAt(0), expectedGroupItems) &&
                    newItems?.Count() == 1 &&
                    Enumerable.SequenceEqual(newItems.ElementAt(0), expectedGroupItems);
        }

        private static bool IsReplaceEventValid(NotifyCollectionChangedEventArgs args, IEnumerable<int> expectedRemovedItems, IEnumerable<int> expectedAddItems)
        {
            var oldItems = args.OldItems?.Cast<IEnumerable<int>>();
            var newItems = args.NewItems?.Cast<IEnumerable<int>>();
            return args.Action == NotifyCollectionChangedAction.Replace &&
                    oldItems?.Count() == 1 &&
                    Enumerable.SequenceEqual(oldItems.ElementAt(0), expectedRemovedItems) &&
                    newItems?.Count() == 1 &&
                    Enumerable.SequenceEqual(newItems.ElementAt(0), expectedAddItems);
        }

        private static bool IsResetEventValid(NotifyCollectionChangedEventArgs args) => args.Action == NotifyCollectionChangedAction.Reset && args.NewItems == null && args.OldItems == null;
    }
}
