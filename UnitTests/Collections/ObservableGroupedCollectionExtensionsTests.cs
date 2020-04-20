// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FluentAssertions;
using Microsoft.Toolkit.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTests.Collections
{
    [TestClass]
    public class ObservableGroupedCollectionExtensionsTests
    {
        [TestCategory("Collections")]
        [TestMethod]
        public void AddGroup_WithItem_ShouldAddGroup()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();

            var addedGroup = groupedCollection.AddGroup("new key", 23);

            addedGroup.Should().NotBeNull();
            addedGroup.Key.Should().Be("new key");
            addedGroup.Should().ContainSingle();
            addedGroup.Should().ContainInOrder(23);

            groupedCollection.Should().ContainSingle();
            groupedCollection.Should().HaveElementAt(0, addedGroup);
        }

        [TestCategory("Collections")]
        [TestMethod]
        public void AddGroup_WithCollection_ShouldAddGroup()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();

            var addedGroup = groupedCollection.AddGroup("new key", new[] { 23, 10, 42 });

            addedGroup.Should().NotBeNull();
            addedGroup.Key.Should().Be("new key");
            addedGroup.Should().HaveCount(3);
            addedGroup.Should().ContainInOrder(23, 10, 42);

            groupedCollection.Should().ContainSingle();
            groupedCollection.Should().HaveElementAt(0, addedGroup);
        }

        [TestCategory("Collections")]
        [TestMethod]
        public void AddGroup_WithParamsCollection_ShouldAddGroup()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();

            var addedGroup = groupedCollection.AddGroup("new key", 23, 10, 42);

            addedGroup.Should().NotBeNull();
            addedGroup.Key.Should().Be("new key");
            addedGroup.Should().HaveCount(3);
            addedGroup.Should().ContainInOrder(23, 10, 42);

            groupedCollection.Should().ContainSingle();
            groupedCollection.Should().HaveElementAt(0, addedGroup);
        }

        [TestCategory("Collections")]
        [TestMethod]
        public void AddItem_WhenTargetGroupDoesNotExists_ShouldCreateAndAddNewGroup()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();

            var addedGroup = groupedCollection.AddItem("new key", 23);

            addedGroup.Should().NotBeNull();
            addedGroup.Key.Should().Be("new key");
            addedGroup.Should().ContainSingle();
            addedGroup.Should().ContainInOrder(23);

            groupedCollection.Should().ContainSingle();
            groupedCollection.Should().HaveElementAt(0, addedGroup);
        }

        [TestCategory("Collections")]
        [TestMethod]
        public void AddItem_WhenSingleTargetGroupAlreadyExists_ShouldAddItemToExistingGroup()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);
            var targetGroup = groupedCollection.AddGroup("B", 4, 5, 6);
            groupedCollection.AddGroup("C", 7, 8);

            var addedGroup = groupedCollection.AddItem("B", 23);

            addedGroup.Should().BeSameAs(targetGroup);
            addedGroup.Key.Should().Be("B");
            addedGroup.Should().HaveCount(4);
            addedGroup.Should().ContainInOrder(4, 5, 6, 23);

            groupedCollection.Should().HaveCount(3);
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);

            groupedCollection.ElementAt(1).Key.Should().Be("B");
            groupedCollection.ElementAt(1).Should().HaveCount(4);
            groupedCollection.ElementAt(1).Should().ContainInOrder(4, 5, 6, 23);

            groupedCollection.ElementAt(2).Key.Should().Be("C");
            groupedCollection.ElementAt(2).Should().HaveCount(2);
            groupedCollection.ElementAt(2).Should().ContainInOrder(7, 8);
        }

        [TestCategory("Collections")]
        [TestMethod]
        public void AddItem_WhenSeveralTargetGroupsAlreadyExist_ShouldAddItemToFirstExistingGroup()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);
            var targetGroup = groupedCollection.AddGroup("B", 4, 5, 6);
            groupedCollection.AddGroup("B", 7, 8, 9);
            groupedCollection.AddGroup("C", 10, 11);

            var addedGroup = groupedCollection.AddItem("B", 23);

            addedGroup.Should().BeSameAs(targetGroup);
            addedGroup.Key.Should().Be("B");
            addedGroup.Should().HaveCount(4);
            addedGroup.Should().ContainInOrder(4, 5, 6, 23);

            groupedCollection.Should().HaveCount(4);
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);

            groupedCollection.ElementAt(1).Key.Should().Be("B");
            groupedCollection.ElementAt(1).Should().HaveCount(4);
            groupedCollection.ElementAt(1).Should().ContainInOrder(4, 5, 6, 23);

            groupedCollection.ElementAt(2).Key.Should().Be("B");
            groupedCollection.ElementAt(2).Should().HaveCount(3);
            groupedCollection.ElementAt(2).Should().ContainInOrder(7, 8, 9);

            groupedCollection.ElementAt(3).Key.Should().Be("C");
            groupedCollection.ElementAt(3).Should().HaveCount(2);
            groupedCollection.ElementAt(3).Should().ContainInOrder(10, 11);
        }

        [TestCategory("Collections")]
        [TestMethod]
        public void RemoveGroup_WhenGroupDoesNotExists_ShouldDoNothing()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);

            groupedCollection.RemoveGroup("I do not exist");

            groupedCollection.Should().ContainSingle();
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);
        }

        [TestCategory("Collections")]
        [TestMethod]
        public void RemoveGroup_WhenSingleGroupExists_ShouldRemoveGroup()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);
            groupedCollection.AddGroup("B", 4, 5, 6);

            groupedCollection.RemoveGroup("B");

            groupedCollection.Should().ContainSingle();
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);
        }

        [TestCategory("Collections")]
        [TestMethod]
        public void RemoveGroup_WhenSeveralGroupsExist_ShouldRemoveFirstGroup()
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);
            groupedCollection.AddGroup("B", 4, 5, 6);
            groupedCollection.AddGroup("B", 7, 8);

            groupedCollection.RemoveGroup("B");

            groupedCollection.Should().HaveCount(2);
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);

            groupedCollection.ElementAt(1).Key.Should().Be("B");
            groupedCollection.ElementAt(1).Should().HaveCount(2);
            groupedCollection.ElementAt(1).Should().ContainInOrder(7, 8);
        }

        [TestCategory("Collections")]
        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void RemoveItem_WhenGroupDoesNotExist_ShouldDoNothing(bool removeGroupIfEmpty)
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);
            groupedCollection.AddGroup("B", 4, 5, 6);

            groupedCollection.RemoveItem("I do not exist", 8, removeGroupIfEmpty);

            groupedCollection.Should().HaveCount(2);
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);

            groupedCollection.ElementAt(1).Key.Should().Be("B");
            groupedCollection.ElementAt(1).Should().HaveCount(3);
            groupedCollection.ElementAt(1).Should().ContainInOrder(4, 5, 6);
        }

        [TestCategory("Collections")]
        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void RemoveItem_WhenGroupExistsAndItemDoesNotExist_ShouldDoNothing(bool removeGroupIfEmpty)
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);
            groupedCollection.AddGroup("B", 4, 5, 6);

            groupedCollection.RemoveItem("B", 8, removeGroupIfEmpty);

            groupedCollection.Should().HaveCount(2);
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);

            groupedCollection.ElementAt(1).Key.Should().Be("B");
            groupedCollection.ElementAt(1).Should().HaveCount(3);
            groupedCollection.ElementAt(1).Should().ContainInOrder(4, 5, 6);
        }

        [TestCategory("Collections")]
        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void RemoveItem_WhenGroupAndItemExist_ShouldRemoveItemFromGroup(bool removeGroupIfEmpty)
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);
            groupedCollection.AddGroup("B", 4, 5, 6);

            groupedCollection.RemoveItem("B", 5, removeGroupIfEmpty);

            groupedCollection.Should().HaveCount(2);
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);

            groupedCollection.ElementAt(1).Key.Should().Be("B");
            groupedCollection.ElementAt(1).Should().HaveCount(2);
            groupedCollection.ElementAt(1).Should().ContainInOrder(4, 6);
        }

        [TestCategory("Collections")]
        [DataTestMethod]
        [DataRow(true, true)]
        [DataRow(false, false)]
        public void RemoveItem_WhenRemovingLastItem_ShouldRemoveGroupIfRequired(bool removeGroupIfEmpty, bool expectGroupRemoved)
        {
            var groupedCollection = new ObservableGroupedCollection<string, int>();
            groupedCollection.AddGroup("A", 1, 2, 3);
            groupedCollection.AddGroup("B", 4);

            groupedCollection.RemoveItem("B", 4, removeGroupIfEmpty);

            groupedCollection.Should().HaveCount(expectGroupRemoved ? 1 : 2);
            groupedCollection.ElementAt(0).Key.Should().Be("A");
            groupedCollection.ElementAt(0).Should().HaveCount(3);
            groupedCollection.ElementAt(0).Should().ContainInOrder(1, 2, 3);

            if (!expectGroupRemoved)
            {
                groupedCollection.ElementAt(1).Key.Should().Be("B");
                groupedCollection.ElementAt(1).Should().BeEmpty();
            }
        }
    }
}