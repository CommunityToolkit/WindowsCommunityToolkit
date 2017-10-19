// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An ObservableCollection of <see cref="AnimationBase"/>
    /// </summary>
    public class AnimationCollection : ObservableCollection<AnimationBase>
    {
        internal UIElement Element { get; set; }

        private void AnimationChanged(object sender, EventArgs e)
        {
            AnimationCollectionChanged?.Invoke(this, null);
        }

        /// <inheritdoc/>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                return;
            }

            if (e.NewItems != null)
            {
                foreach (AnimationBase newAnim in e.NewItems)
                {
                    newAnim.AnimationChanged += AnimationChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (AnimationBase oldAnim in e.OldItems)
                {
                    oldAnim.AnimationChanged -= AnimationChanged;
                }
            }

            AnimationCollectionChanged?.Invoke(this, null);
        }

        /// <summary>
        /// Gets fired when an animation has been added/removed or modified
        /// </summary>
        public event EventHandler AnimationCollectionChanged;

        /// <summary>
        /// Creates a <see cref="CompositionAnimationGroup"/> that can be used to animate a visual on the
        /// Composition layer
        /// </summary>
        /// <param name="element">The element used to get the <see cref="Compositor"/></param>
        /// <returns><see cref="CompositionAnimationGroup"/></returns>
        public CompositionAnimationGroup GetCompositionAnimationGroup(UIElement element)
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
        public ImplicitAnimationCollection GetImplicitAnimationCollection(UIElement element)
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

        /// <summary>
        /// Gets a value indicating whether the collection contains an animation that targets the Translation property
        /// </summary>
        public bool ContainsTranslationAnimation => this.Where(anim => anim.Target.StartsWith("Translation")).Count() > 0;
    }
}
