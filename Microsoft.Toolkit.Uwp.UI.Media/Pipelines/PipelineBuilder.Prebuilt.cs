// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using Microsoft.Graphics.Canvas.Effects;
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
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromHostBackdropAcrylic(
            Color tint,
            float mix,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromHostBackdrop()
                .LuminanceToAlpha()
                .Opacity(0.4f)
                .Blend(FromHostBackdrop(), BlendEffectMode.Multiply)
                .Tint(tint, mix)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the host backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="tintSetter">The optional tint color setter for the effect</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="mixSetter">The optional tint mix setter for the effect</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromHostBackdropAcrylic(
            Color tint,
            out EffectSetter<Color> tintSetter,
            float mix,
            out EffectSetter<float> mixSetter,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromHostBackdrop()
                .LuminanceToAlpha()
                .Opacity(0.4f)
                .Blend(FromHostBackdrop(), BlendEffectMode.Multiply)
                .Tint(tint, out tintSetter, mix, out mixSetter)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the host backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="tintAnimation">The optional tint color animation for the effect</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="mixAnimation">The optional tint mix animation for the effect</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromHostBackdropAcrylic(
            Color tint,
            out EffectAnimation<Color> tintAnimation,
            float mix,
            out EffectAnimation<float> mixAnimation,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromHostBackdrop()
                .LuminanceToAlpha()
                .Opacity(0.4f)
                .Blend(FromHostBackdrop(), BlendEffectMode.Multiply)
                .Tint(tint, out tintAnimation, mix, out mixAnimation)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
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
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            float mix,
            float blur,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
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
        /// <param name="tintSetter">The optional tint color setter for the effect</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="mixSetter">The optional tint mix setter for the effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="blurSetter">The optional blur setter for the effect</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            out EffectSetter<Color> tintSetter,
            float mix,
            out EffectSetter<float> mixSetter,
            float blur,
            out EffectSetter<float> blurSetter,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromBackdrop()
                .Tint(tint, out tintSetter, mix, out mixSetter)
                .Blur(blur, out blurSetter)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }

        /// <summary>
        /// Returns a new <see cref="PipelineBuilder"/> instance that implements the in-app backdrop acrylic effect
        /// </summary>
        /// <param name="tint">The tint color to use</param>
        /// <param name="tintAnimation">The optional tint color animation for the effect</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <param name="mixAnimation">The optional tint mix animation for the effect</param>
        /// <param name="blur">The amount of blur to apply to the acrylic brush</param>
        /// <param name="blurAnimation">The optional blur animation for the effect</param>
        /// <param name="noiseUri">The <see cref="Uri"/> for the noise texture to load for the acrylic effect</param>
        /// <param name="cacheMode">The cache mode to use to load the image</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public static PipelineBuilder FromBackdropAcrylic(
            Color tint,
            out EffectAnimation<Color> tintAnimation,
            float mix,
            out EffectAnimation<float> mixAnimation,
            float blur,
            out EffectAnimation<float> blurAnimation,
            Uri noiseUri,
            CacheMode cacheMode = CacheMode.Default)
        {
            return
                FromBackdrop()
                .Tint(tint, out tintAnimation, mix, out mixAnimation)
                .Blur(blur, out blurAnimation)
                .Blend(FromTiles(noiseUri, cacheMode: cacheMode), BlendEffectMode.Overlay, Placement.Background);
        }
    }
}
