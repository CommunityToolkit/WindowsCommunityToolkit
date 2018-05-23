// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An ObservableCollection of <see cref="AnimationBase"/>
    /// </summary>
    public class AnimationCollection : IList<AnimationBase>
    {
        private readonly List<AnimationBase> _internalList = new List<AnimationBase>();

        // needed in order to be able to update animations when a animations are added/removed or
        // animation properties change (for example in binding)
        private WeakReference<UIElement> _parent;

        internal UIElement Parent
        {
            get
            {
                _parent.TryGetTarget(out var element);
                return element;
            }

            set => _parent = new WeakReference<UIElement>(value);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains an animation that targets the Translation property
        /// </summary>
        public bool ContainsTranslationAnimation => this.Count(anim => !string.IsNullOrWhiteSpace(anim.Target) && anim.Target.StartsWith("Translation")) > 0;

        /// <inheritdoc/>
        public int Count => _internalList.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public AnimationBase this[int index] { get => _internalList[index]; set => Insert(index, value); }

        /// <summary>
        /// Raised when an animation has been added/removed or modified
        /// </summary>
        public event EventHandler AnimationCollectionChanged;

        /// <summary>
        /// Starts the animations in the collection
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to be animated</param>
        public void StartAnimation(UIElement element)
        {
            foreach (var animation in this)
            {
                animation.StartAnimation(element);
            }
        }

        /// <summary>
        /// Creates a <see cref="CompositionAnimationGroup"/> that can be used to animate a visual on the
        /// Composition layer
        /// </summary>
        /// <param name="element">The element used to get the <see cref="Compositor"/></param>
        /// <returns><see cref="CompositionAnimationGroup"/></returns>
        internal CompositionAnimationGroup GetCompositionAnimationGroup(UIElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = visual.Compositor;
            var animationGroup = compositor.CreateAnimationGroup();

            foreach (var cAnim in this)
            {
                var compositionAnimation = cAnim.GetCompositionAnimation(compositor);
                if (compositionAnimation != null)
                {
                    animationGroup.Add(compositionAnimation);
                }
            }

            return animationGroup;
        }

        /// <summary>
        /// Creates a <see cref="ImplicitAnimationCollection"/> that can be used to apply implicit animation on a
        /// visual on the Composition layer
        /// </summary>
        /// <param name="element">The element used to get the <see cref="Compositor"/></param>
        /// <returns><see cref="ImplicitAnimationCollection"/></returns>
        internal ImplicitAnimationCollection GetImplicitAnimationCollection(UIElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = visual.Compositor;
            var implicitAnimations = compositor.CreateImplicitAnimationCollection();

            var animations = new Dictionary<string, CompositionAnimationGroup>();

            foreach (var cAnim in this)
            {
                CompositionAnimation animation;
                if (!string.IsNullOrWhiteSpace(cAnim.Target)
                    && (animation = cAnim.GetCompositionAnimation(compositor)) != null)
                {
                    var target = cAnim.ImplicitTarget ?? cAnim.Target;
                    if (!animations.ContainsKey(target))
                    {
                        animations[target] = compositor.CreateAnimationGroup();
                    }

                    animations[target].Add(animation);
                }
            }

            foreach (var kv in animations)
            {
                implicitAnimations[kv.Key] = kv.Value;
            }

            return implicitAnimations;
        }

        private void AnimationChanged(object sender, EventArgs e)
        {
            AnimationCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public int IndexOf(AnimationBase item)
        {
            return _internalList.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, AnimationBase item)
        {
            item.AnimationChanged += AnimationChanged;
            _internalList.Insert(index, item);
            AnimationCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _internalList.Count)
            {
                var animation = _internalList[index];
                animation.AnimationChanged -= AnimationChanged;
            }

            _internalList.RemoveAt(index);
            AnimationCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void Add(AnimationBase item)
        {
            item.AnimationChanged += AnimationChanged;
            _internalList.Add(item);
            AnimationCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (var animation in _internalList)
            {
                animation.AnimationChanged -= AnimationChanged;
            }

            _internalList.Clear();
            AnimationCollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public bool Contains(AnimationBase item)
        {
            return _internalList.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(AnimationBase[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public bool Remove(AnimationBase item)
        {
            var result = _internalList.Remove(item);
            if (result)
            {
                item.AnimationChanged -= AnimationChanged;
                AnimationCollectionChanged?.Invoke(this, EventArgs.Empty);
            }

            return result;
        }

        /// <inheritdoc/>
        public IEnumerator<AnimationBase> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }
    }
}
