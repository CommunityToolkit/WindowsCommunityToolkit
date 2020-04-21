// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Collections
{
    /// <summary>
    /// The extensions methods to simplify the usage of <see cref="ObservableGroupedCollection{TKey, TValue}"/>.
    /// </summary>
    public static class ObservableGroupedCollectionExtensions
    {
        /// <summary>
        /// Adds a key-value <see cref="ObservableGroup{TKey, TValue}"/> item into a target <see cref="ObservableGroupedCollection{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
        /// <param name="source">The source <see cref="ObservableGroupedCollection{TKey, TValue}"/> instance.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>The added <see cref="ObservableGroup{TKey, TValue}"/>.</returns>
        public static ObservableGroup<TKey, TValue> AddGroup<TKey, TValue>(
            this ObservableGroupedCollection<TKey, TValue> source,
            TKey key,
            TValue value)
            => AddGroup(source, key, new[] { value });

        /// <summary>
        /// Adds a key-collection <see cref="ObservableGroup{TKey, TValue}"/> item into a target <see cref="ObservableGroupedCollection{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
        /// <param name="source">The source <see cref="ObservableGroupedCollection{TKey, TValue}"/> instance.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="collection">The collection to add.</param>
        /// <returns>The added <see cref="ObservableGroup{TKey, TValue}"/>.</returns>
        public static ObservableGroup<TKey, TValue> AddGroup<TKey, TValue>(
            this ObservableGroupedCollection<TKey, TValue> source,
            TKey key,
            params TValue[] collection)
            => source.AddGroup(key, (IEnumerable<TValue>)collection);

        /// <summary>
        /// Adds a key-collection <see cref="ObservableGroup{TKey, TValue}"/> item into a target <see cref="ObservableGroupedCollection{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
        /// <param name="source">The source <see cref="ObservableGroupedCollection{TKey, TValue}"/> instance.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="collection">The collection to add.</param>
        /// <returns>The added <see cref="ObservableGroup{TKey, TValue}"/>.</returns>
        public static ObservableGroup<TKey, TValue> AddGroup<TKey, TValue>(
            this ObservableGroupedCollection<TKey, TValue> source,
            TKey key,
            IEnumerable<TValue> collection)
        {
            var group = new ObservableGroup<TKey, TValue>(key, collection);
            source.Add(group);

            return group;
        }

        /// <summary>
        /// Add <paramref name="item"/> into the first group with <paramref name="key"/> key.
        /// If the group does not exist, it will be added.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
        /// <param name="source">The source <see cref="ObservableGroupedCollection{TKey, TValue}"/> instance.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="item">The item to add.</param>
        /// <returns>The instance of the <see cref="ObservableGroup{TKey, TValue}"/> which will receive the value. It will either be an existing group or a new group.</returns>
        public static ObservableGroup<TKey, TValue> AddItem<TKey, TValue>(
            this ObservableGroupedCollection<TKey, TValue> source,
            TKey key,
            TValue item)
        {
            var existingGroup = source.FirstOrDefault(group => GroupKeyPredicate(group, key));
            if (existingGroup is null)
            {
                existingGroup = new ObservableGroup<TKey, TValue>(key);
                source.Add(existingGroup);
            }

            existingGroup.Add(item);
            return existingGroup;
        }

        /// <summary>
        /// Insert <paramref name="item"/> into the first group with <paramref name="key"/> key at <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
        /// <param name="source">The source <see cref="ObservableGroupedCollection{TKey, TValue}"/> instance.</param>
        /// <param name="key">The key to add.</param>
        /// <param name="index">The index where to insert <paramref name="item"/>.</param>
        /// <param name="item">The item to add.</param>
        /// <returns>The instance of the <see cref="ObservableGroup{TKey, TValue}"/> which will receive the value.</returns>
        /// <exception cref="InvalidOperationException">The target group does not exist.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero or <paramref name="index"/> is greater than the group elements' count.</exception>
        public static ObservableGroup<TKey, TValue> InsertItem<TKey, TValue>(
            this ObservableGroupedCollection<TKey, TValue> source,
            TKey key,
            int index,
            TValue item)
        {
            var existingGroup = source.First(group => GroupKeyPredicate(group, key));
            if (existingGroup is null)
            {
                throw new InvalidOperationException();
            }

            existingGroup.Insert(index, item);
            return existingGroup;
        }

        /// <summary>
        /// Remove the first occurrence of the group with <paramref name="key"/> from the <paramref name="source"/> grouped collection.
        /// It will not do anything if the group does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
        /// <param name="source">The source <see cref="ObservableGroupedCollection{TKey, TValue}"/> instance.</param>
        /// <param name="key">The key of the item to remove.</param>
        public static void RemoveGroup<TKey, TValue>(
            this ObservableGroupedCollection<TKey, TValue> source,
            TKey key)
        {
            var index = 0;
            foreach (var group in source)
            {
                if (GroupKeyPredicate(group, key))
                {
                    source.RemoveAt(index);
                    return;
                }

                index++;
            }
        }

        /// <summary>
        /// Remove the first <paramref name="item"/> from the first group with <paramref name="key"/> from the <paramref name="source"/> grouped collection.
        /// It will not do anything if the group or the item does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
        /// <param name="source">The source <see cref="ObservableGroupedCollection{TKey, TValue}"/> instance.</param>
        /// <param name="key">The key of the item to remove.</param>
        /// <param name="item">The item to remove.</param>
        /// <param name="removeGroupIfEmpty">If true (default value), the group will be removed once it becomes empty.</param>
        public static void RemoveItem<TKey, TValue>(
            this ObservableGroupedCollection<TKey, TValue> source,
            TKey key,
            TValue item,
            bool removeGroupIfEmpty = true)
        {
            var index = 0;
            foreach (var group in source)
            {
                if (GroupKeyPredicate(group, key))
                {
                    group.Remove(item);

                    if (removeGroupIfEmpty && group.Count == 0)
                    {
                        source.RemoveAt(index);
                    }

                    return;
                }

                index++;
            }
        }

        /// <summary>
        /// Remove the item at <paramref name="index"/> from the first group with <paramref name="key"/> from the <paramref name="source"/> grouped collection.
        /// It will not do anything if the group or the item does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
        /// <param name="source">The source <see cref="ObservableGroupedCollection{TKey, TValue}"/> instance.</param>
        /// <param name="key">The key of the item to remove.</param>
        /// <param name="index">The index of the item to remove in the group.</param>
        /// <param name="removeGroupIfEmpty">If true (default value), the group will be removed once it becomes empty.</param>
        public static void RemoveItemAt<TKey, TValue>(
            this ObservableGroupedCollection<TKey, TValue> source,
            TKey key,
            int index,
            bool removeGroupIfEmpty = true)
        {
            var groupIndex = 0;
            foreach (var group in source)
            {
                if (GroupKeyPredicate(group, key))
                {
                    group.RemoveAt(index);

                    if (removeGroupIfEmpty && group.Count == 0)
                    {
                        source.RemoveAt(groupIndex);
                    }

                    return;
                }

                groupIndex++;
            }
        }

        private static bool GroupKeyPredicate<TKey, TValue>(ObservableGroup<TKey, TValue> group, TKey expectedKey)
            => EqualityComparer<TKey>.Default.Equals(group.Key, expectedKey);
    }
}
