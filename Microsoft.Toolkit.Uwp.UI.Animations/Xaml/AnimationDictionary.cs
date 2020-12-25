// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A collection of animations that can be defined from XAML.
    /// </summary>
    public sealed class AnimationDictionary : DependencyObject, IList<AnimationCollection2>
    {
        /// <summary>
        /// The underlying list of animations.
        /// </summary>
        private readonly List<AnimationCollection2> list = new();

        /// <summary>
        /// The reference to the parent that owns the current animation dictionary.
        /// </summary>
        private WeakReference<UIElement>? parent;

        /// <summary>
        /// Sets the parent <see cref="UIElement"/> for the current animation dictionary.
        /// </summary>
        internal UIElement? Parent
        {
            set
            {
                WeakReference<UIElement> parent = this.parent = new(value!);

                foreach (var item in this.list)
                {
                    item.ParentReference = parent;
                }
            }
        }

        /// <inheritdoc/>
        public int Count => this.list.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public AnimationCollection2 this[int index]
        {
            get => this.list[index];
            set
            {
                this.list[index].ParentReference = null;
                this.list[index] = value;

                value.ParentReference = this.parent;
            }
        }

        /// <inheritdoc/>
        public void Add(AnimationCollection2 item)
        {
            this.list.Add(item);

            item.ParentReference = this.parent;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (var item in this.list)
            {
                item.ParentReference = this.parent;
            }

            this.list.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(AnimationCollection2 item)
        {
            return this.list.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(AnimationCollection2[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<AnimationCollection2> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(AnimationCollection2 item)
        {
            return this.list.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, AnimationCollection2 item)
        {
            this.list.Insert(index, item);

            item.ParentReference = this.parent;
        }

        /// <inheritdoc/>
        public bool Remove(AnimationCollection2 item)
        {
            bool removed = this.list.Remove(item);

            if (removed)
            {
                item.ParentReference = null;
            }

            return removed;
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            this.list[index].ParentReference = null;
            this.list.RemoveAt(index);
        }
    }
}
