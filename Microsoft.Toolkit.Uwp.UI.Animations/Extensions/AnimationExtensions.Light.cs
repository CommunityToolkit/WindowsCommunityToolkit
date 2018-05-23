// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Effects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides an extension which allows lighting.
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Stores all the point lights along with the visuals that they are applied to.
        /// This is to stop multiplication of point lights on a single visual.
        /// </summary>
        private static Dictionary<Visual, PointLight> pointLights = new Dictionary<Visual, PointLight>();

        /// <summary>
        /// Gets a value indicating whether this instance is lighting supported.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is lighting supported; otherwise, <c>false</c>.
        /// </value>
        public static bool IsLightingSupported
        {
            get
            {
                bool lightingSupported = true;

                if (!Windows.Foundation.Metadata.ApiInformation.IsMethodPresent("Windows.UI.Xaml.Hosting.ElementCompositionPreview", "SetElementChildVisual"))
                {
                    lightingSupported = false;
                }

                if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Composition.CompositionSurfaceBrush"))
                {
                    lightingSupported = false;
                }

                return lightingSupported;
            }
        }

        /// <summary>
        /// Animates a point light and it's distance.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        /// <param name="distance">The value.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="color">The color of the spotlight.</param>
        /// <param name="easingType">The easing function</param>
        /// <param name="easingMode">The easing mode</param>
        /// <returns>An animation set.</returns>
        public static AnimationSet Light(
            this FrameworkElement associatedObject,
            double distance = 0d,
            double duration = 500d,
            double delay = 0d,
            Color? color = null,
            EasingType easingType = EasingType.Default,
            EasingMode easingMode = EasingMode.EaseOut)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Light(distance, duration, delay, color, easingType, easingMode);
        }

        /// <summary>
        /// Animates a point light and it's distance.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="distance">The distance of the light.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay. (ignored if duration == 0)</param>
        /// <param name="color">The color of the spotlight.</param>
        /// <param name="easingType">The easing function</param>
        /// <param name="easingMode">The easing mode</param>
        /// <seealso cref="IsLightingSupported" />
        /// <returns>
        /// An Animation Set.
        /// </returns>
        public static AnimationSet Light(
            this AnimationSet animationSet,
            double distance = 0d,
            double duration = 500d,
            double delay = 0d,
            Color? color = null,
            EasingType easingType = EasingType.Default,
            EasingMode easingMode = EasingMode.EaseOut)
        {
            if (!IsLightingSupported)
            {
                return null;
            }

            if (animationSet == null)
            {
                return null;
            }

            var visual = animationSet.Visual;
            var associatedObject = animationSet.Element as FrameworkElement;

            if (associatedObject == null)
            {
                return animationSet;
            }

            var compositor = visual?.Compositor;
            if (compositor == null)
            {
                return null;
            }

            var task = new AnimationTask();
            task.AnimationSet = animationSet;

            task.Task = DispatcherHelper.ExecuteOnUIThreadAsync(
                () =>
            {
                const string sceneName = "PointLightScene";
                PointLight pointLight;
                CompositionDrawingSurface normalMap = null;

                if (!pointLights.ContainsKey(visual))
                {
                    SurfaceLoader.Initialize(compositor);
                    normalMap = SurfaceLoader.LoadText(string.Empty, new Windows.Foundation.Size(512, 512), new Graphics.Canvas.Text.CanvasTextFormat(), Colors.Transparent, Colors.Transparent);
                }

                if (pointLights.ContainsKey(visual))
                {
                    pointLight = pointLights[visual];
                }
                else
                {
                    pointLight = compositor.CreatePointLight();

                    var normalBrush = compositor.CreateSurfaceBrush(normalMap);
                    normalBrush.Stretch = CompositionStretch.Fill;

                    // check to see if the visual already has a point light applied.
                    var spriteVisual = ElementCompositionPreview.GetElementChildVisual(associatedObject) as SpriteVisual;
                    var normalsBrush = spriteVisual?.Brush as CompositionEffectBrush;

                    if (normalsBrush == null || normalsBrush.Comment != sceneName)
                    {
                        var lightEffect = new CompositeEffect()
                        {
                            Mode = CanvasComposite.Add,
                            Sources =
                            {
                                new CompositionEffectSourceParameter("ImageSource"),
                                new SceneLightingEffect()
                                {
                                    Name = sceneName,
                                    AmbientAmount = 0,
                                    DiffuseAmount = 0.5f,
                                    SpecularAmount = 0,
                                    NormalMapSource = new CompositionEffectSourceParameter("NormalMap"),
                                }
                            }
                        };

                        var effectFactory = compositor.CreateEffectFactory(lightEffect);
                        var brush = effectFactory.CreateBrush();
                        brush.SetSourceParameter("NormalMap", normalBrush);

                        var sprite = compositor.CreateSpriteVisual();
                        sprite.Size = visual.Size;
                        sprite.Brush = brush;
                        sprite.Comment = sceneName;

                        ElementCompositionPreview.SetElementChildVisual(task.AnimationSet.Element, sprite);

                        pointLight.CoordinateSpace = visual;
                        pointLight.Targets.Add(visual);
                    }
                }

                pointLight.Color = color ?? Colors.White;
                var delayTime = task.Delay != null ? task.Delay.Value : TimeSpan.FromMilliseconds(delay);
                var durationTime = task.Duration != null ? task.Duration.Value : TimeSpan.FromMilliseconds(duration);

                if (durationTime.TotalMilliseconds <= 0)
                {
                    task.AnimationSet.AddEffectDirectPropertyChange(pointLight, (float)distance, nameof(pointLight.Offset));
                }
                else
                {
                    var diffuseAnimation = compositor.CreateVector3KeyFrameAnimation();
                    diffuseAnimation.InsertKeyFrame(1f, new System.Numerics.Vector3(visual.Size.X / 2, visual.Size.Y / 2, (float)distance), GetCompositionEasingFunction(easingType, compositor, easingMode));
                    diffuseAnimation.Duration = durationTime;
                    diffuseAnimation.DelayTime = delayTime;

                    task.AnimationSet.AddCompositionEffectAnimation(pointLight, diffuseAnimation, nameof(pointLight.Offset));
                }

                pointLights[visual] = pointLight;
            }, Windows.UI.Core.CoreDispatcherPriority.Normal);

            animationSet.AddAnimationThroughTask(task);
            return animationSet;
        }
    }
}
