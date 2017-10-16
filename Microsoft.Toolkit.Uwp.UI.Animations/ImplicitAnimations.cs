using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    public class Implicit
    {
        public static readonly DependencyProperty ShowAnimationsProperty =
            DependencyProperty.RegisterAttached("ShowAnimations",
                                                typeof(CAnimationCollection),
                                                typeof(Implicit),
                                                new PropertyMetadata(null, ShowAnimationsChanged));

        public static readonly DependencyProperty HideAnimationsProperty =
            DependencyProperty.RegisterAttached("HideAnimations",
                                                typeof(CAnimationCollection),
                                                typeof(Implicit),
                                                new PropertyMetadata(null, HideAnimationsChanged));

        // Using a DependencyProperty as the backing store for Animations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationsProperty =
            DependencyProperty.RegisterAttached("Animations",
                                                typeof(CAnimationCollection),
                                                typeof(Implicit),
                                                new PropertyMetadata(null, AnimationsChanged));

        public static CAnimationCollection GetShowAnimations(DependencyObject obj)
        {
            var collection = (CAnimationCollection)obj.GetValue(ShowAnimationsProperty);

            if (collection == null)
            {
                collection = new CAnimationCollection();
                obj.SetValue(ShowAnimationsProperty, collection);
            }

            return collection;
        }

        public static void SetShowAnimations(DependencyObject obj, CAnimationCollection value)
        {
            obj.SetValue(ShowAnimationsProperty, value);
        }

        public static CAnimationCollection GetHideAnimations(DependencyObject obj)
        {
            var collection = (CAnimationCollection)obj.GetValue(HideAnimationsProperty);

            if (collection == null)
            {
                collection = new CAnimationCollection();
                obj.SetValue(HideAnimationsProperty, collection);
            }
            return collection;
        }

        public static void SetHideAnimations(DependencyObject obj, CAnimationCollection value)
        {
            obj.SetValue(HideAnimationsProperty, value);
        }

        public static CAnimationCollection GetAnimations(DependencyObject obj)
        {
            var collection = (CAnimationCollection)obj.GetValue(AnimationsProperty);

            if (collection == null)
            {
                collection = new CAnimationCollection();
                obj.SetValue(AnimationsProperty, collection);
            }
            return collection;
        }

        public static void SetAnimations(DependencyObject obj, CAnimationCollection value)
        {
            obj.SetValue(AnimationsProperty, value);
        }

        private static void ShowAnimationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is CAnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= ShowCollectionChanged;
            }

            if (!(e.NewValue is CAnimationCollection animationCollection))
            {
                return;
            }

            if (!(d is UIElement element))
            {
                return;
            }

            animationCollection.Element = element;
            animationCollection.AnimationCollectionChanged -= ShowCollectionChanged;
            animationCollection.AnimationCollectionChanged += ShowCollectionChanged;

            ElementCompositionPreview.SetImplicitShowAnimation(element, GetCompositionAnimationGroup(animationCollection, element));
        }

        private static void HideAnimationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is CAnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= HideCollectionChanged;
            }

            if (!(e.NewValue is CAnimationCollection animationCollection))
            {
                return;
            }

            if (!(d is UIElement element))
            {
                return;
            }

            animationCollection.Element = element;
            animationCollection.AnimationCollectionChanged -= HideCollectionChanged;
            animationCollection.AnimationCollectionChanged += HideCollectionChanged;

            ElementCompositionPreview.SetImplicitHideAnimation(element, GetCompositionAnimationGroup(animationCollection, element));
        }

        private static void AnimationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is CAnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= AnimationsCollectionChanged;
            }

            if (!(e.NewValue is CAnimationCollection animationCollection))
            {
                return;
            }

            if (!(d is UIElement element))
            {
                return;
            }

            animationCollection.Element = element;
            animationCollection.AnimationCollectionChanged -= AnimationsCollectionChanged;
            animationCollection.AnimationCollectionChanged += AnimationsCollectionChanged;

            ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = GetImplicitAnimationCollection(animationCollection, element);
        }

        private static void ShowCollectionChanged(object sender, EventArgs e)
        {
            var collection = sender as CAnimationCollection;
            if (collection.Element == null)
            {
                return;
            }

            ElementCompositionPreview.SetImplicitShowAnimation(collection.Element, GetCompositionAnimationGroup(collection, collection.Element));
        }

        private static void HideCollectionChanged(object sender, EventArgs e)
        {
            var collection = sender as CAnimationCollection;
            if (collection.Element == null)
            {
                return;
            }

            ElementCompositionPreview.SetImplicitHideAnimation(collection.Element, GetCompositionAnimationGroup(collection, collection.Element));

        }

        private static void AnimationsCollectionChanged(object sender, EventArgs e)
        {
            var collection = sender as CAnimationCollection;
            if (collection.Element == null)
            {
                return;
            }

            ElementCompositionPreview.GetElementVisual(collection.Element).ImplicitAnimations =
                                            GetImplicitAnimationCollection(collection, collection.Element);
        }

        private static CompositionAnimationGroup GetCompositionAnimationGroup(CAnimationCollection collection, UIElement element)
        {
            if (collection.ContainsTranslationAnimation)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            }

            return collection.GetCompositionAnimationGroup(element);
        }

        private static ImplicitAnimationCollection GetImplicitAnimationCollection(CAnimationCollection collection, UIElement element)
        {
            if (collection.ContainsTranslationAnimation)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            }

            return collection.GetImplicitAnimationCollection(element);
        }
    }
}
