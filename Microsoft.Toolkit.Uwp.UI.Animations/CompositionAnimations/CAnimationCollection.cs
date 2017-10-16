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
    public class CAnimationCollection : ObservableCollection<CAnimation>
    {
        public UIElement Element { get; set; }

        public event EventHandler AnimationCollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                return;
            }

            if (e.NewItems != null)
            {
                foreach (CAnimation newAnim in e.NewItems)
                {
                    newAnim.AnimationChanged += AnimationChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (CAnimation oldAnim in e.OldItems)
                {
                    oldAnim.AnimationChanged -= AnimationChanged;
                }
            }

            AnimationCollectionChanged?.Invoke(this, null);
        }

        private void AnimationChanged(object sender, EventArgs e)
        {
            AnimationCollectionChanged?.Invoke(this, null);
        }

        public CompositionAnimationGroup GetCompositionAnimationGroup(UIElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);

            var compositor = visual.Compositor;
            var animationGroup = compositor.CreateAnimationGroup();

            foreach (var cAnim in this)
            {
                var compositionAnimation = cAnim.GetCompositionAnimation(visual);
                if (compositionAnimation != null)
                {
                    animationGroup.Add(compositionAnimation);
                }
            }

            return animationGroup;
        }

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
                    && (animation = cAnim.GetCompositionAnimation(visual)) != null)
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

        public bool ContainsTranslationAnimation => this.Where(anim => anim.Target.StartsWith("Translation")).Count() > 0;
    }
}
