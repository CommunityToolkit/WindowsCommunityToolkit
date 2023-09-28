// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// XAML equivalent of Win2d's CanvasSolidColorBrush class which paints in solid color.
    /// </summary>
    public class SolidColorCanvasBrush : RenderCanvasBrushBase
    {
        /// <summary>
        /// Color Dependency Property
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color",
            typeof(Color),
            typeof(SolidColorCanvasBrush),
            new PropertyMetadata(Colors.Transparent, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the color of the brush.
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (SolidColorCanvasBrush)d;

            // Recreate the canvas brush on any property change.
            brush.OnUpdated();
        }

        /// <inheritdoc/>
        protected override void OnUpdated()
        {
            CanvasBrush = new CanvasSolidColorBrush(CompositionGenerator.Instance.Device, Color);
            base.OnUpdated();
        }
    }
}
