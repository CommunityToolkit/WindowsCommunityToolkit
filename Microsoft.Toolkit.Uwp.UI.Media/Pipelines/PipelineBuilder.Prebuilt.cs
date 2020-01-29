// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Media.Pipelines
{
    /// <summary>
    /// A <see langword="class"/> that allows to build custom effects pipelines and create <see cref="Windows.UI.Composition.CompositionBrush"/> instances from them
    /// </summary>
    public sealed partial class PipelineBuilder
    {
        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the host backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="noiseRelativePath">The relative path for the noise texture to load (eg. "/Assets/noise.png")</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromHostBackdropAcrylic(Color tint, float mix, string noiseRelativePath, CacheMode cacheMode = CacheMode.Default)
        {
            return FromHostBackdropAcrylic(tint, mix, noiseRelativePath.ToAppxUri(), cacheMode);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the host backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromHostBackdropAcrylic(Color tint, float mix, Uri noiseUri, CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromHostBackdrop()
                .Effect(source => new LuminanceToAlphaEffect { Source = source })
                .Opacity(0.4f)
                .Blend(FromHostBackdrop(), BlendEffectMode.Multiply)
                .Tint(tint, mix)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the host backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="tintAnimation">The animation to apply on the tint color of the effect</param>
        /// <param name="noiseRelativePath">The relative path for the noise texture to load (eg. "/Assets/noise.png")</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromHostBackdropAcrylic(Color tint, float mix, out EffectAnimation tintAnimation, string noiseRelativePath, CacheMode cacheMode = CacheMode.Default)
        {
            return FromHostBackdropAcrylic(tint, mix, out tintAnimation, noiseRelativePath.ToAppxUri(), cacheMode);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the host backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="tintAnimation">The animation to apply on the tint color of the effect</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromHostBackdropAcrylic(Color tint, float mix, out EffectAnimation tintAnimation, Uri noiseUri, CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromHostBackdrop()
                .Effect(source => new LuminanceToAlphaEffect { Source = source })
                .Opacity(0.4f)
                .Blend(FromHostBackdrop(), BlendEffectMode.Multiply)
                .Tint(tint, mix, out tintAnimation)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="noiseRelativePath">The relative path for the noise texture to load (eg. "/Assets/noise.png")</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(Color tint, float mix, float blur, string noiseRelativePath, CacheMode cacheMode = CacheMode.Default)
        {
            return FromBackdropAcrylic(tint, mix, blur, noiseRelativePath.ToAppxUri(), cacheMode);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(Color tint, float mix, float blur, Uri noiseUri, CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromBackdrop()
                .Tint(tint, mix)
                .Blur(blur)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="tintAnimation">The animation to apply on the tint color of the effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="noiseRelativePath">The relative path for the noise texture to load (eg. "/Assets/noise.png")</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            float mix,
            out EffectAnimation tintAnimation,
            float blur,
            string noiseRelativePath,
            CacheMode cacheMode = CacheMode.Default)
        {
            return FromBackdropAcrylic(tint, mix, out tintAnimation, blur, noiseRelativePath.ToAppxUri(), cacheMode);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="tintAnimation">The animation to apply on the tint color of the effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            float mix,
            out EffectAnimation tintAnimation,
            float blur,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromBackdrop()
                .Tint(tint, mix, out tintAnimation)
                .Blur(blur)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="blurAnimation">The animation to apply on the blur effect in the pipeline</param>
        /// <param name="noiseRelativePath">The relative path for the noise texture to load (eg. "/Assets/noise.png")</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            float mix,
            float blur,
            out EffectAnimation blurAnimation,
            string noiseRelativePath,
            CacheMode cacheMode = CacheMode.Default)
        {
            return FromBackdropAcrylic(tint, mix, blur, out blurAnimation, noiseRelativePath.ToAppxUri(), cacheMode);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="blurAnimation">The animation to apply on the blur effect in the pipeline</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            float mix,
            float blur,
            out EffectAnimation blurAnimation,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromBackdrop()
                .Tint(tint, mix)
                .Blur(blur, out blurAnimation)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="tintAnimation">The animation to apply on the tint color of the effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="blurAnimation">The animation to apply on the blur effect in the pipeline</param>
        /// <param name="noiseRelativePath">The relative path for the noise texture to load (eg. "/Assets/noise.png")</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            float mix,
            out EffectAnimation tintAnimation,
            float blur,
            out EffectAnimation blurAnimation,
            string noiseRelativePath,
            CacheMode cacheMode = CacheMode.Default)
        {
            return FromBackdropAcrylic(tint, mix, out tintAnimation, blur, out blurAnimation, noiseRelativePath.ToAppxUri(), cacheMode);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="tintAnimation">The animation to apply on the tint color of the effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="blurAnimation">The animation to apply on the blur effect in the pipeline</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            float mix,
            out EffectAnimation tintAnimation,
            float blur,
            out EffectAnimation blurAnimation,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromBackdrop()
                .Tint(tint, mix, out tintAnimation)
                .Blur(blur, out blurAnimation)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }
    }
}
