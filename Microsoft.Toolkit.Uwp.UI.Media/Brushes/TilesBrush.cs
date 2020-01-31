// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Brushes.Base;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// A <see cref="XamlCompositionBrush"/> that displays a tiled image
    /// </summary>
    public sealed class TilesBrush : XamlCompositionEffectBrushBase
    {
        /// <summary>
        /// Gets or sets the <see cref="Uri"/> to the texture to use
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
            typeof(TilesBrush),
            new PropertyMetadata(default, OnTextureUriPropertyChanged));

        /// <summary>
        /// Updates the UI when <see cref="TextureUri"/> changes
        /// </summary>
        /// <param name="d">The current <see cref="AcrylicBrush"/> instance</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for <see cref="TextureUriProperty"/></param>
        private static void OnTextureUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TilesBrush brush &&
                brush.CompositionBrush != null)
            {
                brush.OnDisconnected();
                brush.OnConnected();
            }
        }

        /// <inheritdoc/>
        protected override PipelineBuilder OnBrushRequested() => PipelineBuilder.FromTiles(this.TextureUri);
    }
}
