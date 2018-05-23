// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
        /// A cached dictionary mapping easings to bezier control points
        /// </summary>
        private static readonly Dictionary<(string, EasingMode), (Vector2, Vector2)> _compositionEasingFunctions = new Dictionary<(string, EasingMode), (Vector2, Vector2)>();

        /// <summary>
        /// Gets or sets the default EasingType used for storyboard animations
        /// </summary>
        public static EasingType DefaultEasingType { get; set; } = EasingType.Cubic;

        /// <summary>
        /// Begins a Storyboard animation and returns a task that completes when the
        /// animation is complete
        /// </summary>
        /// <param name="storyboard">The storyboard to be started</param>
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
        /// Gets the EasingFunction from an EasingType and optional EasingMode to be used with Storyboard animations
        /// </summary>
        /// <param name="easingType">The EasingType used to determine the EasingFunction</param>
        /// <param name="easingMode">The EasingMode used to determine the EasingFunction. Defaults to <see cref="EasingMode.EaseOut"/></param>
        /// <returns>Return the appropriate EasingFuntion or null if the EasingType is Linear</returns>
        public static EasingFunctionBase GetEasingFunction(EasingType easingType, EasingMode easingMode = EasingMode.EaseOut)
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
                    return new CubicEase { EasingMode = easingMode };
                case EasingType.Back:
                    return new BackEase { EasingMode = easingMode };
                case EasingType.Bounce:
                    return new BounceEase { EasingMode = easingMode };
                case EasingType.Elastic:
                    return new ElasticEase { EasingMode = easingMode };
                case EasingType.Circle:
                    return new CircleEase { EasingMode = easingMode };
                case EasingType.Quadratic:
                    return new QuadraticEase { EasingMode = easingMode };
                case EasingType.Quartic:
                    return new QuarticEase { EasingMode = easingMode };
                case EasingType.Quintic:
                    return new QuinticEase { EasingMode = easingMode };
                case EasingType.Sine:
                    return new SineEase { EasingMode = easingMode };
                default:
                    throw new NotSupportedException($"{easingType.ToString()} EasingType is not currently supported");
            }
        }

        /// <summary>
        /// Generates an <see cref="CompositionEasingFunction"/> to be used with Composition animations
        /// </summary>
        /// <param name="easingType">The <see cref="EasingType"/> used to generate the CompositionEasingFunction</param>
        /// <param name="compositor">The <see cref="Compositor"/></param>
        /// <param name="easingMode">The <see cref="EasingMode"/> used to generate the CompositionEasingFunction. Defaults to <see cref="EasingMode.EaseOut"/></param>
        /// <returns><see cref="CompositionEasingFunction"/></returns>
        public static CompositionEasingFunction GetCompositionEasingFunction(EasingType easingType, Compositor compositor, EasingMode easingMode = EasingMode.EaseOut)
        {
            if (DesignTimeHelpers.IsRunningInLegacyDesignerMode)
            {
                return compositor.CreateLinearEasingFunction();
            }

            return GenerateCompositionEasingFunctionFromEasingType(easingType, compositor, easingMode);
        }

        /// <summary>
        /// Creates the cache of easing functions if the cache does not already exist.
        /// </summary>
        private static void EnsureEasingsCached()
        {
            if (_compositionEasingFunctions.Count > 0)
            {
                return;
            }

            // We don't cache actual composition easing functions here as they can be disposed
            // and we don't want to deal with caching a disposed easing function
            void Add(EasingType type, EasingMode mode, Vector2 p1, Vector2 p2)
            {
                // In order to generate a usable hashcode for our ValueTuple without collisions
                // we can't use enum values for both type & mode, so we have to string one of them.
                _compositionEasingFunctions[(type.ToString(), mode)] = (p1, p2);
            }

            Add(EasingType.Cubic, EasingMode.EaseOut, new Vector2(0.215f, 0.61f), new Vector2(0.355f, 1f));
            Add(EasingType.Cubic, EasingMode.EaseIn, new Vector2(0.55f, 0.055f), new Vector2(0.675f, 0.19f));
            Add(EasingType.Cubic, EasingMode.EaseInOut, new Vector2(0.645f, 0.045f), new Vector2(0.355f, 1f));

            Add(EasingType.Back, EasingMode.EaseOut, new Vector2(0.175f, 0.885f), new Vector2(0.32f, 1.275f));
            Add(EasingType.Back, EasingMode.EaseIn, new Vector2(0.6f, -0.28f), new Vector2(0.735f, 0.045f));
            Add(EasingType.Back, EasingMode.EaseInOut, new Vector2(0.68f, -0.55f), new Vector2(0.265f, 1.55f));

            Add(EasingType.Bounce, EasingMode.EaseOut, new Vector2(0.58f, 1.93f), new Vector2(.08f, .36f));
            Add(EasingType.Bounce, EasingMode.EaseIn, new Vector2(0.93f, 0.7f), new Vector2(0.4f, -0.93f));
            Add(EasingType.Bounce, EasingMode.EaseInOut, new Vector2(0.65f, -0.85f), new Vector2(0.35f, 1.85f));

            Add(EasingType.Elastic, EasingMode.EaseOut, new Vector2(0.37f, 2.68f), new Vector2(0f, 0.22f));
            Add(EasingType.Elastic, EasingMode.EaseIn, new Vector2(1, .78f), new Vector2(.63f, -1.68f));
            Add(EasingType.Elastic, EasingMode.EaseInOut, new Vector2(0.9f, -1.2f), new Vector2(0.1f, 2.2f));

            Add(EasingType.Circle, EasingMode.EaseOut, new Vector2(0.075f, 0.82f), new Vector2(0.165f, 1f));
            Add(EasingType.Circle, EasingMode.EaseIn, new Vector2(0.6f, 0.04f), new Vector2(0.98f, 0.335f));
            Add(EasingType.Circle, EasingMode.EaseInOut, new Vector2(0.785f, 0.135f), new Vector2(0.15f, 0.86f));

            Add(EasingType.Quadratic, EasingMode.EaseOut, new Vector2(0.25f, 0.46f), new Vector2(0.45f, 0.94f));
            Add(EasingType.Quadratic, EasingMode.EaseIn, new Vector2(0.55f, 0.085f), new Vector2(0.68f, 0.53f));
            Add(EasingType.Quadratic, EasingMode.EaseInOut, new Vector2(0.445f, 0.03f), new Vector2(0.515f, 0.955f));

            Add(EasingType.Quartic, EasingMode.EaseOut, new Vector2(0.165f, 0.84f), new Vector2(0.44f, 1f));
            Add(EasingType.Quartic, EasingMode.EaseIn, new Vector2(0.895f, 0.03f), new Vector2(0.685f, 0.22f));
            Add(EasingType.Quartic, EasingMode.EaseInOut, new Vector2(0.77f, 0.0f), new Vector2(0.175f, 1.0f));

            Add(EasingType.Quintic, EasingMode.EaseOut, new Vector2(0.23f, 1f), new Vector2(0.32f, 1f));
            Add(EasingType.Quintic, EasingMode.EaseIn, new Vector2(0.755f, 0.05f), new Vector2(0.855f, 0.06f));
            Add(EasingType.Quintic, EasingMode.EaseInOut, new Vector2(0.86f, 0.0f), new Vector2(0.07f, 1.0f));

            Add(EasingType.Sine, EasingMode.EaseOut, new Vector2(0.39f, 0.575f), new Vector2(0.565f, 1f));
            Add(EasingType.Sine, EasingMode.EaseIn, new Vector2(0.47f, 0.0f), new Vector2(0.745f, 0.715f));
            Add(EasingType.Sine, EasingMode.EaseInOut, new Vector2(0.445f, 0.05f), new Vector2(0.55f, 0.95f));
        }

        private static CompositionEasingFunction GenerateCompositionEasingFunctionFromEasingType(EasingType easingType, Compositor compositor, EasingMode easingMode = EasingMode.EaseOut)
        {
            if (easingType == EasingType.Default)
            {
                easingType = DefaultEasingType;
            }

            if (easingType == EasingType.Linear)
            {
                return compositor.CreateLinearEasingFunction();
            }

            // Pay-per-play caching of easing functions
            EnsureEasingsCached();

            if (_compositionEasingFunctions.TryGetValue((easingType.ToString(), easingMode), out (Vector2, Vector2) points))
            {
                return compositor.CreateCubicBezierEasingFunction(points.Item1, points.Item2);
            }

            throw new NotSupportedException($"{easingType.ToString()} EasingType and {easingMode.ToString()} EasingMode combination is not currently supported");
        }

        private static string GetAnimationPath(CompositeTransform transform, UIElement element, string property)
        {
            if (element.RenderTransform == transform)
            {
                return $"(UIElement.RenderTransform).(CompositeTransform.{property})";
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
