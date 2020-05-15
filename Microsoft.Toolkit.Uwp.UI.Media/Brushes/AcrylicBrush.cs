// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Brushes.Base;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// A <see cref="XamlCompositionBrush"/> that implements an acrylic effect with customizable parameters
    /// </summary>
    public sealed class AcrylicBrush : XamlCompositionEffectBrushBase
    {
        /// <summary>
        /// The <see cref="EffectSetter{T}"/> instance in use to set the blur amount
        /// </summary>
        /// <remarks>This is only set when <see cref="Source"/> is <see cref="AcrylicBackgroundSource.Backdrop"/></remarks>
        private EffectSetter<float> blurAmountSetter;

        /// <summary>
        /// The <see cref="EffectSetter{T}"/> instance in use to set the tint color
        /// </summary>
        private EffectSetter<Color> tintSetter;

        /// <summary>
        /// The <see cref="EffectSetter{T}"/> instance in use to set the tint mix amount
        /// </summary>
        private EffectSetter<float> tintMixSetter;

        /// <summary>
        /// Gets or sets the source mode for the effect
        /// </summary>
        public AcrylicBackgroundSource Source
        {
            get => (AcrylicBackgroundSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(AcrylicBackgroundSource),
            typeof(AcrylicBrush),
            new PropertyMetadata(AcrylicBackgroundSource.Backdrop, OnSourcePropertyChanged));

        /// <summary>
        /// Updates the UI when <see cref="Source"/> changes
        /// </summary>
        /// <param name="d">The current <see cref="AcrylicBrush"/> instance</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for <see cref="SourceProperty"/></param>
        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AcrylicBrush brush &&
                brush.CompositionBrush != null)
            {
                brush.OnDisconnected();
                brush.OnConnected();
            }
        }

        /// <summary>
        /// Gets or sets the blur amount for the effect
        /// </summary>
        /// <remarks>This property is ignored when the active mode is <see cref="AcrylicBackgroundSource.HostBackdrop"/></remarks>
        public double BlurAmount
        {
            get => (double)GetValue(BlurAmountProperty);
            set => SetValue(BlurAmountProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="BlurAmount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register(
            nameof(BlurAmount),
            typeof(double),
            typeof(AcrylicBrush),
            new PropertyMetadata(0.0, OnBlurAmountPropertyChanged));

        /// <summary>
        /// Updates the UI when <see cref="Source"/> changes
        /// </summary>
        /// <param name="d">The current <see cref="AcrylicBrush"/> instance</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for <see cref="SourceProperty"/></param>
        private static void OnBlurAmountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AcrylicBrush brush &&
                brush.Source != AcrylicBackgroundSource.HostBackdrop && // Blur is fixed by OS when using HostBackdrop source.
                brush.CompositionBrush is CompositionBrush target)
            {
                brush.blurAmountSetter?.Invoke(target, (float)(double)e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the tint for the effect
        /// </summary>
        public Color Tint
        {
            get => (Color)GetValue(TintProperty);
            set => SetValue(TintProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Tint"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TintProperty = DependencyProperty.Register(
            nameof(Tint),
            typeof(Color),
            typeof(AcrylicBrush),
            new PropertyMetadata(default(Color), OnTintPropertyChanged));

        /// <summary>
        /// Updates the UI when <see cref="Tint"/> changes
        /// </summary>
        /// <param name="d">The current <see cref="AcrylicBrush"/> instance</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for <see cref="TintProperty"/></param>
        private static void OnTintPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AcrylicBrush brush &&
                brush.CompositionBrush is CompositionBrush target)
            {
                brush.tintSetter?.Invoke(target, (Color)e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the tint mix factor for the effect
        /// </summary>
        public double TintMix { get; set; }

        /// <summary>
        /// Identifies the <see cref="TintMix"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TintMixProperty = DependencyProperty.Register(
            nameof(TintMix),
            typeof(double),
            typeof(AcrylicBrush),
            new PropertyMetadata(0.0, OnTintMixPropertyChanged));

        /// <summary>
        /// Updates the UI when <see cref="TintMix"/> changes
        /// </summary>
        /// <param name="d">The current <see cref="AcrylicBrush"/> instance</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for <see cref="TintMixProperty"/></param>
        private static void OnTintMixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AcrylicBrush brush &&
                brush.CompositionBrush is CompositionBrush target)
            {
                brush.tintMixSetter?.Invoke(target, (float)(double)e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> for the texture to use
        /// </summary>
        public Uri TextureUri
        {
            get => (Uri)GetValue(TextureUriProperty);
            set => SetValue(TextureUriProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="TextureUri"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextureUriProperty = DependencyProperty.Register(
            nameof(TextureUri),
            typeof(Uri),
            typeof(AcrylicBrush),
            new PropertyMetadata(default, OnTextureUriPropertyChanged));

        /// <summary>
        /// Updates the UI when <see cref="TextureUri"/> changes
        /// </summary>
        /// <param name="d">The current <see cref="AcrylicBrush"/> instance</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for <see cref="TextureUriProperty"/></param>
        private static void OnTextureUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AcrylicBrush brush &&
                brush.CompositionBrush != null)
            {
                brush.OnDisconnected();
                brush.OnConnected();
            }
        }

        /// <inheritdoc/>
        protected override PipelineBuilder OnBrushRequested()
        {
            switch (this.Source)
            {
                case AcrylicBackgroundSource.Backdrop:
                    return PipelineBuilder.FromBackdropAcrylic(
                        Tint,
                        out tintSetter,
                        (float)TintMix,
                        out tintMixSetter,
                        (float)BlurAmount,
                        out this.blurAmountSetter,
                        TextureUri);
                case AcrylicBackgroundSource.HostBackdrop:
                    return PipelineBuilder.FromHostBackdropAcrylic(
                        Tint,
                        out tintSetter,
                        (float)TintMix,
                        out tintMixSetter,
                        TextureUri);
                default: throw new ArgumentOutOfRangeException(nameof(this.Source), $"Invalid acrylic source: {this.Source}");
            }
        }
    }
}
