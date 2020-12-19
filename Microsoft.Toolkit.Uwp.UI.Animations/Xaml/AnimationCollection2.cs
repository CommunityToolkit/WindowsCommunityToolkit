// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A collection of animations that can be grouped together.
    /// </summary>
    public sealed class AnimationCollection2 : DependencyObject, IList<Animation>, ITimeline
    {
        /// <summary>
        /// The underlying list of animations.
        /// </summary>
        private readonly List<Animation> list = new();

        /// <summary>
        /// The reference to the parent that owns the current animation collection.
        /// </summary>
        private WeakReference<UIElement>? parent;

        /// <summary>
        /// Raised whenever the current animation is started.
        /// </summary>
        public event EventHandler? Started;

        /// <summary>
        /// Raised whenever the current animation ends.
        /// </summary>
        public event EventHandler? Ended;

        /// <summary>
        /// Raised whenever the current collection changes.
        /// </summary>
        public event EventHandler? CollectionChanged;

        /// <summary>
        /// Gets or sets the parent <see cref="UIElement"/> for the current animation collection.
        /// </summary>
        internal UIElement? Parent
        {
            get
            {
                UIElement? element = null;

                _ = this.parent?.TryGetTarget(out element);

                return element;
            }
            set => this.parent = new(value!);
        }

        /// <inheritdoc/>
        public int Count => this.list.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public Animation this[int index]
        {
            get => this.list[index];
            set
            {
                this.list[index] = value;

                CollectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public void Add(Animation item)
        {
            this.list.Add(item);

            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.list.Clear();

            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public bool Contains(Animation item)
        {
            return this.list.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(Animation[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<Animation> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(Animation item)
        {
            return this.list.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, Animation item)
        {
            this.list.Insert(index, item);

            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public bool Remove(Animation item)
        {
            bool removed = this.list.Remove(item);

            if (removed)
            {
                CollectionChanged?.Invoke(this, EventArgs.Empty);
            }

            return removed;
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);

            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public void Start()
        {
            _ = StartAsync();
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public Task StartAsync()
        {
            return StartAsync(Parent!);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public void Start(UIElement element)
        {
            _ = StartAsync(element);
        }

        /// <inheritdoc cref="AnimationBuilder.Start(UIElement)"/>
        public async Task StartAsync(UIElement element)
        {
            Started?.Invoke(this, EventArgs.Empty);

            await ((ITimeline)this).AppendToBuilder(new AnimationBuilder()).StartAsync(element);

            Ended?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint)
        {
            foreach (ITimeline element in this)
            {
                builder = element.AppendToBuilder(builder, delayHint, durationHint);
            }

            return builder;
        }
    }
}
