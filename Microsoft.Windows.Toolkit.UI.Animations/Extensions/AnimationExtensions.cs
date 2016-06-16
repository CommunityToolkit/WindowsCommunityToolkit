// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************

using System;
using System.Threading.Tasks;

using Microsoft.Windows.Toolkit.UI;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Windows.Toolkit
{
    /// <summary>
    /// Defines a collection of helper methods for UI <see cref="Storyboard"/> animation.
    /// </summary>
    public static class AnimationExtensions
    {
        /// <summary>
        /// Animates a <see cref="FrameworkElement"/> on the X axis.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="x">
        /// The target <see cref="double"/> value on the X axis.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Storyboard"/>.
        /// </returns>
        public static Storyboard AnimateX(this FrameworkElement element, double x, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.GetTranslateX() != x)
            {
                return AnimateDoubleProperty(element.GetCompositeTransform(), "TranslateX", element.GetTranslateX(), x, duration, easingFunction);
            }

            return null;
        }

        /// <summary>
        /// Animates a <see cref="FrameworkElement"/> on the X axis.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="x">
        /// The target <see cref="double"/> value on the X axis.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>

        public static async Task AnimateXAsync(this FrameworkElement element, double x, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.GetTranslateX() != x)
            {
                await AnimateDoublePropertyAsync(element.GetCompositeTransform(), "TranslateX", element.GetTranslateX(), x, duration, easingFunction);
            }
        }

        /// <summary>
        /// Animates a <see cref="FrameworkElement"/> on the Y axis.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="y">
        /// The target <see cref="double"/> value on the Y axis.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Storyboard"/>.
        /// </returns>
        public static Storyboard AnimateY(this FrameworkElement element, double y, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.GetTranslateY() != y)
            {
                return AnimateDoubleProperty(element.GetCompositeTransform(), "TranslateY", element.GetTranslateY(), y, duration, easingFunction);
            }

            return null;
        }

        /// <summary>
        /// Animates a <see cref="FrameworkElement"/> on the Y axis.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="y">
        /// The target <see cref="double"/> value on the Y axis.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        public static async Task AnimateYAsync(this FrameworkElement element, double y, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.GetTranslateY() != y)
            {
                await AnimateDoublePropertyAsync(element.GetCompositeTransform(), "TranslateY", element.GetTranslateY(), y, duration, easingFunction);
            }
        }

        /// <summary>
        /// Animates width of a <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="width">
        /// The target <see cref="double"/> width value.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Storyboard"/>.
        /// </returns>
        public static Storyboard AnimateWidth(this FrameworkElement element, double width, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.ActualWidth != width)
            {
                return AnimateDoubleProperty(element, "Width", element.ActualWidth, width, duration, easingFunction);
            }

            return null;
        }

        /// <summary>
        /// Animates width of a <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="width">
        /// The target <see cref="double"/> width value.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        public static async Task AnimateWidthAsync(this FrameworkElement element, double width, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.ActualWidth != width)
            {
                await AnimateDoublePropertyAsync(element, "Width", element.ActualWidth, width, duration, easingFunction);
            }
        }

        /// <summary>
        /// Animates height of a <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="height">
        /// The target <see cref="double"/> height value.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Storyboard"/>.
        /// </returns>
        public static Storyboard AnimateHeight(this FrameworkElement element, double height, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Height != height)
            {
                return AnimateDoubleProperty(element, "Height", element.ActualHeight, height, duration, easingFunction);
            }

            return null;
        }

        /// <summary>
        /// Animates height of a <see cref="FrameworkElement"/>.
        /// </summary>
        /// <param name="element">
        /// The target <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="height">
        /// The target <see cref="double"/> height value.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        public static async Task AnimateHeightAsync(this FrameworkElement element, double height, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Height != height)
            {
                await AnimateDoublePropertyAsync(element, "Height", element.ActualHeight, height, duration, easingFunction);
            }
        }

        /// <summary>
        /// Fade in a <see cref="FrameworkElement"/> from current opacity to 1.0.
        /// </summary>
        /// <param name="element">The target <see cref="FrameworkElement"/>.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="easingFunction">An optional easing function.</param>
        /// <returns>Returns a <see cref="Storyboard"/>.</returns>
        public static Storyboard FadeIn(this UIElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Opacity < 1.0)
            {
                return AnimateDoubleProperty(element, "Opacity", element.Opacity, 1.0, duration, easingFunction);
            }

            return null;
        }

        /// <summary>
        /// Fade in a <see cref="FrameworkElement"/> from current opacity to 1.0.
        /// </summary>
        /// <param name="element">The target <see cref="FrameworkElement"/>.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="easingFunction">An optional easing function.</param>
        /// <returns>Returns an await-able task.</returns>
        public static async Task FadeInAsync(this UIElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Opacity < 1.0)
            {
                await AnimateDoublePropertyAsync(element, "Opacity", element.Opacity, 1.0, duration, easingFunction);
            }
        }

        /// <summary>
        /// Fade out a <see cref="FrameworkElement"/> from current opacity to 0.0.
        /// </summary>
        /// <param name="element">The target <see cref="FrameworkElement"/>.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="easingFunction">An optional easing function.</param>
        /// <returns>Returns a <see cref="Storyboard"/>.</returns>
        public static Storyboard FadeOut(this UIElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Opacity > 0.0)
            {
                return AnimateDoubleProperty(element, "Opacity", element.Opacity, 0.0, duration, easingFunction);
            }

            return null;
        }

        /// <summary>
        /// Fade out a <see cref="FrameworkElement"/> from current opacity to 0.0.
        /// </summary>
        /// <param name="element">The target <see cref="FrameworkElement"/>.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="easingFunction">An optional easing function.</param>
        /// <returns>Returns an await-able task.</returns>
        public static async Task FadeOutAsync(this UIElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Opacity > 0.0)
            {
                await AnimateDoublePropertyAsync(element, "Opacity", element.Opacity, 0.0, duration, easingFunction);
            }
        }

        /// <summary>
        /// Animates a <see cref="double"/> property on a given target asynchronously.
        /// </summary>
        /// <param name="target">
        /// The target <see cref="DependencyObject"/>.
        /// </param>
        /// <param name="property">
        /// The name of the property to animate.
        /// </param>
        /// <param name="from">
        /// A starting <see cref="double"/> value.
        /// </param>
        /// <param name="to">
        /// The resulting <see cref="double"/> value.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns an await-able task.
        /// </returns>
        public static Task AnimateDoublePropertyAsync(
            this DependencyObject target, 
            string property, 
            double from, 
            double to, 
            double duration = 250, 
            EasingFunctionBase easingFunction = null)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Storyboard storyboard = AnimateDoubleProperty(target, property, from, to, duration, easingFunction);
            storyboard.Completed += (sender, e) =>
                {
                    tcs.SetResult(true);
                };
            return tcs.Task;
        }

        /// <summary>
        /// Animates a <see cref="double"/> property on a given target.
        /// </summary>
        /// <param name="target">
        /// The target <see cref="DependencyObject"/>.
        /// </param>
        /// <param name="property">
        /// The name of the property to animate.
        /// </param>
        /// <param name="from">
        /// A starting <see cref="double"/> value.
        /// </param>
        /// <param name="to">
        /// The resulting <see cref="double"/> value.
        /// </param>
        /// <param name="duration">
        /// The duration of the animation.
        /// </param>
        /// <param name="easingFunction">
        /// An optional easing function.
        /// </param>
        /// <returns>
        /// Returns the <see cref="Storyboard"/> for the animation.
        /// </returns>
        public static Storyboard AnimateDoubleProperty(
            this DependencyObject target, 
            string property, 
            double from, 
            double to, 
            double duration = 250, 
            EasingFunctionBase easingFunction = null)
        {
            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = from, 
                To = to, 
                Duration = TimeSpan.FromMilliseconds(duration), 
                EasingFunction = easingFunction ?? new SineEase(), 
                FillBehavior = FillBehavior.HoldEnd, 
                EnableDependentAnimation = true
            };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, property);

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Begin();

            return storyboard;
        }
    }
}