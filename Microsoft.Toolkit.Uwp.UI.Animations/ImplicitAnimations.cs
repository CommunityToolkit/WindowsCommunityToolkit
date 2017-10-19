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
                                                typeof(AnimationCollection),
                                                typeof(Implicit),
                                                new PropertyMetadata(null, ShowAnimationsChanged));

        public static readonly DependencyProperty HideAnimationsProperty =
            DependencyProperty.RegisterAttached("HideAnimations",
                                                typeof(AnimationCollection),
                                                typeof(Implicit),
                                                new PropertyMetadata(null, HideAnimationsChanged));

        // Using a DependencyProperty as the backing store for Animations.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationsProperty =
            DependencyProperty.RegisterAttached("Animations",
                                                typeof(AnimationCollection),
                                                typeof(Implicit),
                                                new PropertyMetadata(null, AnimationsChanged));

        public static AnimationCollection GetShowAnimations(DependencyObject obj)
        {
            var collection = (AnimationCollection)obj.GetValue(ShowAnimationsProperty);

            if (collection == null)
            {
                collection = new AnimationCollection();
                obj.SetValue(ShowAnimationsProperty, collection);
            }

            return collection;
        }

        public static void SetShowAnimations(DependencyObject obj, AnimationCollection value)
        {
            obj.SetValue(ShowAnimationsProperty, value);
        }

        public static AnimationCollection GetHideAnimations(DependencyObject obj)
        {
            var collection = (AnimationCollection)obj.GetValue(HideAnimationsProperty);

            if (collection == null)
            {
                collection = new AnimationCollection();
                obj.SetValue(HideAnimationsProperty, collection);
            }
            return collection;
        }

        public static void SetHideAnimations(DependencyObject obj, AnimationCollection value)
        {
            obj.SetValue(HideAnimationsProperty, value);
        }

        public static AnimationCollection GetAnimations(DependencyObject obj)
        {
            var collection = (AnimationCollection)obj.GetValue(AnimationsProperty);

            if (collection == null)
            {
                collection = new AnimationCollection();
                obj.SetValue(AnimationsProperty, collection);
            }
            return collection;
        }

        public static void SetAnimations(DependencyObject obj, AnimationCollection value)
        {
            obj.SetValue(AnimationsProperty, value);
        }

        private static void ShowAnimationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is AnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= ShowCollectionChanged;
            }

            if (!(e.NewValue is AnimationCollection animationCollection))
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
            if (e.OldValue is AnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= HideCollectionChanged;
            }

            if (!(e.NewValue is AnimationCollection animationCollection))
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
            if (e.OldValue is AnimationCollection oldCollection)
            {
                oldCollection.AnimationCollectionChanged -= AnimationsCollectionChanged;
            }

            if (!(e.NewValue is AnimationCollection animationCollection))
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
            var collection = sender as AnimationCollection;
            if (collection.Element == null)
            {
                return;
            }

            ElementCompositionPreview.SetImplicitShowAnimation(collection.Element, GetCompositionAnimationGroup(collection, collection.Element));
        }

        private static void HideCollectionChanged(object sender, EventArgs e)
        {
            var collection = sender as AnimationCollection;
            if (collection.Element == null)
            {
                return;
            }

            ElementCompositionPreview.SetImplicitHideAnimation(collection.Element, GetCompositionAnimationGroup(collection, collection.Element));

        }

        private static void AnimationsCollectionChanged(object sender, EventArgs e)
        {
            var collection = sender as AnimationCollection;
            if (collection.Element == null)
            {
                return;
            }

            ElementCompositionPreview.GetElementVisual(collection.Element).ImplicitAnimations =
                                            GetImplicitAnimationCollection(collection, collection.Element);
        }

        private static CompositionAnimationGroup GetCompositionAnimationGroup(AnimationCollection collection, UIElement element)
        {
            if (collection.ContainsTranslationAnimation)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            }

            return collection.GetCompositionAnimationGroup(element);
        }

        private static ImplicitAnimationCollection GetImplicitAnimationCollection(AnimationCollection collection, UIElement element)
        {
            if (collection.ContainsTranslationAnimation)
            {
                ElementCompositionPreview.SetIsTranslationEnabled(element, true);
            }

            return collection.GetImplicitAnimationCollection(element);
        }
    }
}
