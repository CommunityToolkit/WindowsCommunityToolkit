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

        private static CompositionEasingFunction GenerateCompositionEasingFunctionFromEasingType(EasingType easingType, Compositor compositor, EasingMode easingMode = EasingMode.EaseOut)
        {
            if (easingType == EasingType.Default)
            {
                easingType = DefaultEasingType;
            }

#pragma warning disable SA1515 // Single-line comment must be preceded by blank line - suppressed for brevity.

            int key = ((int)easingType * 10) + (int)easingMode;
            switch (key)
            {
                // Linear, EaseOut, EaseIn, EaseInOut
                case 10:
                case 11:
                case 12:
                    return compositor.CreateLinearEasingFunction();
                // Cubic, EaseOut
                case 20:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.215f, 0.61f), new Vector2(0.355f, 1f));
                // Cubic, EaseIn
                case 21:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.55f, 0.055f), new Vector2(0.675f, 0.19f));
                // Cubic, EaseInOut
                case 22:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.645f, 0.045f), new Vector2(0.355f, 1f));
                // Back, EaseOut
                case 30:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.175f, 0.885f), new Vector2(0.32f, 1.275f));
                // Back, EaseIn
                case 31:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.6f, -0.28f), new Vector2(0.735f, 0.045f));
                // Back, EaseInOut
                case 32:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.68f, -0.55f), new Vector2(0.265f, 1.55f));
                // Bounce, EaseOut
                case 40:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.58f, 1.93f), new Vector2(.08f, .36f));
                // Bounce, EaseIn
                case 41:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.93f, 0.7f), new Vector2(0.4f, -0.93f));
                // Bounce, EaseInOut
                case 42:
                    throw new NotImplementedException();
                // Elastic, EaseOut
                case 50:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.37f, 2.68f), new Vector2(0f, 0.22f));
                // Elastic, EaseIn
                case 51:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(1, .78f), new Vector2(.63f, -1.68f));
                // Elastic, EaseInOut
                case 52:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.18f, -3f), new Vector2(0.82f, 3));
                // Circle, EaseOut
                case 60:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.075f, 0.82f), new Vector2(0.165f, 1f));
                // Circle, EaseIn
                case 61:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.6f, 0.04f), new Vector2(0.98f, 0.335f));
                // Circle, EaseInOut
                case 62:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.785f, 0.135f), new Vector2(0.15f, 0.86f));
                // Quadratic, EaseOut
                case 70:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.25f, 0.46f), new Vector2(0.45f, 0.94f));
                // Quadratic, EaseIn
                case 71:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.55f, 0.085f), new Vector2(0.68f, 0.53f));
                // Quadratic, EaseInOut
                case 72:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.445f, 0.03f), new Vector2(0.515f, 0.955f));
                // Quartic, EaseOut
                case 80:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.165f, 0.84f), new Vector2(0.44f, 1f));
                // Quartic, EaseIn
                case 81:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.895f, 0.03f), new Vector2(0.685f, 0.22f));
                // Quartic, EaseInOut
                case 82:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.77f, 0.0f), new Vector2(0.175f, 1.0f));
                // Quintic, EaseOut
                case 90:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.23f, 1f), new Vector2(0.32f, 1f));
                // Quintic, EaseIn
                case 91:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.755f, 0.05f), new Vector2(0.855f, 0.06f));
                // Quintic, EaseInOut
                case 92:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.86f, 0.0f), new Vector2(0.07f, 1.0f));
                // Sine, EaseOut
                case 100:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.39f, 0.575f), new Vector2(0.565f, 1f));
                // Sine, EaseIn
                case 101:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.47f, 0.0f), new Vector2(0.745f, 0.715f));
                // Sine, EaseInOut
                case 102:
                    return compositor.CreateCubicBezierEasingFunction(new Vector2(0.445f, 0.05f), new Vector2(0.55f, 0.95f));
                default:
                    throw new NotSupportedException($"{easingType.ToString()} EasingType and {easingMode.ToString()} EasingMode combination is not currently supported");
            }

#pragma warning restore SA1515 // Single-line comment must be preceded by blank line

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
