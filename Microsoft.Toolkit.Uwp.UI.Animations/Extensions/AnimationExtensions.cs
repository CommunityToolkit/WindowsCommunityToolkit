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
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods perform animation on UIElements
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Gets or sets the default EasingType used for storyboard animations
        /// </summary>
        public static EasingType DefaultEasingType { get; set; } = EasingType.Cubic;

        /// <summary>
        /// Begins a Storyboard animation and returns a task that completes when the
        /// animaton is complete
        /// </summary>
        /// <param name="storyboard">The storyoard to be started</param>
        /// <returns>Task that completes when the animation is complete</returns>
        public static Task BeginAsync(this Storyboard storyboard)
        {
            var taskSource = new TaskCompletionSource<object>();
            EventHandler<object> completed = null;
            completed += (s, e) =>
            {
                storyboard.Completed -= completed;
                taskSource.SetResult(null);
            };

            storyboard.Completed += completed;
            storyboard.Begin();

            return taskSource.Task;
        }

        /// <summary>
        /// Gets the EasingFunction from EasingType to be used with Storyboard animations
        /// </summary>
        /// <param name="easingType">The EasingType used to determine the EasingFunction</param>
        /// <returns>Return the appropriate EasingFuntion or null if the EasingType is Linear</returns>
        public static EasingFunctionBase GetEasingFunction(EasingType easingType)
        {
            if (easingType == EasingType.Default)
            {
                easingType = DefaultEasingType;
            }

            switch (easingType)
            {
                case EasingType.Linear:
                    return null;
                case EasingType.Cubic:
                    return new CubicEase();
                case EasingType.Back:
                    return new BackEase();
                case EasingType.Bounce:
                    return new BounceEase();
                case EasingType.Elastic:
                    return new ElasticEase();
                case EasingType.Circle:
                    return new CircleEase();
                case EasingType.Quadratic:
                    return new QuadraticEase();
                case EasingType.Quartic:
                    return new QuarticEase();
                case EasingType.Quintic:
                    return new QuinticEase();
                case EasingType.Sine:
                    return new SineEase();
                default:
                    throw new ArgumentOutOfRangeException(nameof(easingType), easingType, null);
            }
        }

        /// <summary>
        /// Generates an <see cref="CompositionEasingFunction"/> to be used with Composition animations
        /// </summary>
        /// <param name="easingType">The <see cref="EasingType"/> used to generate the CompositionEasingFunction</param>
        /// <param name="compositor">The <see cref="Compositor"/></param>
        /// <returns><see cref="CompositionEasingFunction"/></returns>
        public static CompositionEasingFunction GetCompositionEasingFunction(EasingType easingType, Compositor compositor)
        {
            if (DesignTimeHelpers.IsRunningInLegacyDesignerMode)
            {
                return compositor.CreateLinearEasingFunction();
            }

            return GenerateCompositionEasingFunctionFromEasingType(easingType, compositor);
        }

        private static CompositionEasingFunction GenerateCompositionEasingFunctionFromEasingType(EasingType easingType, Compositor compositor)
        {
            if (easingType == EasingType.Default)
            {
                easingType = DefaultEasingType;
            }

            switch (easingType)
            {
                case EasingType.Linear:
                    return compositor.CreateLinearEasingFunction();
                case EasingType.Cubic:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.215f, 0.61f), new Vector2(0.355f, 1f));
                case EasingType.Back:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.175f, 0.885f), new Vector2(0.32f, 1.275f));
                case EasingType.Bounce:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.58f, 1.93f), new Vector2(.08f, .36f));
                case EasingType.Elastic:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.37f, 2.68f), new Vector2(0f, 0.22f));
                case EasingType.Circle:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.075f, 0.82f), new Vector2(0.165f, 1f));
                case EasingType.Quadratic:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.25f, 0.46f), new Vector2(0.45f, 0.94f));
                case EasingType.Quartic:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.165f, 0.84f), new Vector2(0.44f, 1f));
                case EasingType.Quintic:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.23f, 1f), new Vector2(0.32f, 1f));
                case EasingType.Sine:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.39f, 0.575f), new Vector2(0.565f, 1f));
                default:
                    throw new ArgumentOutOfRangeException(nameof(easingType), easingType, null);
            }
        }

        private static string GetAnimationPath(CompositeTransform transform, UIElement element, string property)
        {
            if (element.RenderTransform == transform)
            {
                return "(UIElement.RenderTransform).(CompositeTransform.Rotation)";
            }

            var group = element.RenderTransform as TransformGroup;

            if (group == null)
            {
                return string.Empty;
            }

            for (var index = 0; index < group.Children.Count; index++)
            {
                if (group.Children[index] == transform)
                {
                    return $"(UIElement.RenderTransform).(TransformGroup.Children)[{index}].(CompositeTransform.{property})";
                }
            }

            return string.Empty;
        }

        private static CompositeTransform GetAttachedCompositeTransform(UIElement element)
        {
            // We need to use an index to keep track of our CompositeTransform as animation engine
            // recreates new transform objects when animating properties

            // Already attached?
            var compositeTransformIndex = AnimationTools.GetAnimationCompositeTransformIndex(element);

            if (compositeTransformIndex > -2)
            {
                if (compositeTransformIndex == -1 && element.RenderTransform is CompositeTransform)
                {
                    return (CompositeTransform)element.RenderTransform;
                }

                var group = element.RenderTransform as TransformGroup;

                if (group?.Children.Count > compositeTransformIndex && group.Children[compositeTransformIndex] is CompositeTransform)
                {
                    return (CompositeTransform)group.Children[compositeTransformIndex];
                }
            }

            // Let's create a new CompositeTransform
            var result = new CompositeTransform();

            var currentTransform = element.RenderTransform;

            if (currentTransform != null)
            {
                // We found a RenderTransform

                // Is it a TransformGroup?
                var currentTransformGroup = currentTransform as TransformGroup;

                if (currentTransformGroup != null)
                {
                    currentTransformGroup.Children.Add(result);

                    AnimationTools.SetAnimationCompositeTransformIndex(element, currentTransformGroup.Children.Count - 1);
                }
                else
                {
                    // Let's create our own TransformGroup
                    var group = new TransformGroup();
                    group.Children.Add(currentTransform);
                    group.Children.Add(result);
                    element.RenderTransform = group;

                    AnimationTools.SetAnimationCompositeTransformIndex(element, 1);
                }
            }
            else
            {
                element.RenderTransform = result;

                AnimationTools.SetAnimationCompositeTransformIndex(element, -1);
            }

            return result;
        }
    }
}
