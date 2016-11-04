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
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Composition.Effects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides an extension which allows lighting.
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Stores all the point lights along with the visuals that they are applied to.
        /// This is to stop mulitplication of point lights on a single visual.
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
        /// <returns>An animation set.</returns>
        public static async Task<AnimationSet> Light(
            this FrameworkElement associatedObject,
            double distance = 0d,
            double duration = 500d,
            double delay = 0d)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return await animationSet.Light(distance, duration, delay);
        }

        /// <summary>
        /// Animates a point light and it's distance.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="distance">The distance of the light.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay. (ignored if duration == 0)</param>
        /// <seealso cref="IsLightingSupported" />
        /// <returns>
        /// An Animation Set.
        /// </returns>
        public static async Task<AnimationSet> Light(
            this AnimationSet animationSet,
            double distance = 0d,
            double duration = 500d,
            double delay = 0d)
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

            const string sceneName = "PointLightScene";
            PointLight pointLight;

            if (pointLights.ContainsKey(visual))
            {
                pointLight = pointLights[visual];
            }
            else
            {
                pointLight = compositor.CreatePointLight();

                SurfaceLoader.Initialize(compositor);
                CompositionDrawingSurface normalMap = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Microsoft.Toolkit.Uwp.UI.Animations/Assets/SphericalWithMask.png"));

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

                    ElementCompositionPreview.SetElementChildVisual(animationSet.Element, sprite);

                    pointLight.CoordinateSpace = visual;
                    pointLight.Targets.Add(visual);
                }
            }

            if (duration <= 0)
            {
                animationSet.AddEffectDirectPropertyChange(pointLight, (float)distance, nameof(pointLight.Offset));
            }
            else
            {
                var diffuseAnimation = compositor.CreateVector3KeyFrameAnimation();
                diffuseAnimation.InsertKeyFrame(1f, new System.Numerics.Vector3(visual.Size.X / 2, visual.Size.Y / 2, (float)distance));
                diffuseAnimation.Duration = TimeSpan.FromMilliseconds(duration);
                diffuseAnimation.DelayTime = TimeSpan.FromMilliseconds(delay);

                animationSet.AddCompositionEffectAnimation(pointLight, diffuseAnimation, nameof(pointLight.Offset));
            }

            pointLights[visual] = pointLight;

            return animationSet;
        }
    }
}
