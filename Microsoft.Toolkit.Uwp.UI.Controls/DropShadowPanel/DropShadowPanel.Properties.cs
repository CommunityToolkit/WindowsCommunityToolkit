// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="DropShadowPanel"/> control allows the creation of a DropShadow for any Xaml FrameworkElement in markup
    /// making it easier to add shadows to Xaml without having to directly drop down to Windows.UI.Composition APIs.
    /// </summary>
    public partial class DropShadowPanel
    {
        /// <summary>
        /// Identifies the <see cref="BlurRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlurRadiusProperty =
             DependencyProperty.Register(nameof(BlurRadius), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(9.0, OnBlurRadiusChanged));

        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(DropShadowPanel), new PropertyMetadata(Colors.Black, OnColorChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetXChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetY"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty =
            DependencyProperty.Register(nameof(OffsetY), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetYChanged));

        /// <summary>
        /// Identifies the <see cref="OffsetZ"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetZProperty =
            DependencyProperty.Register(nameof(OffsetZ), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(0.0, OnOffsetZChanged));

        /// <summary>
        /// Identifies the <see cref="ShadowOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShadowOpacityProperty =
            DependencyProperty.Register(nameof(ShadowOpacity), typeof(double), typeof(DropShadowPanel), new PropertyMetadata(1.0, OnShadowOpacityChanged));

        /// <summary>
        /// Gets a value indicating whether the platform supports drop shadows.
        /// </summary>
        /// <remarks>
        /// On platforms not supporting drop shadows, this control has no effect.
        /// </remarks>
        public static bool IsSupported =>
            (!DesignTimeHelpers.IsRunningInLegacyDesignerMode) && ApiInformation.IsTypePresent("Windows.UI.Composition.DropShadow"); // SDK >= 14393

        /// <summary>
         /// Gets DropShadow. Exposes the underlying composition object to allow custom Windows.UI.Composition animations.
         /// </summary>
        public DropShadow DropShadow => _dropShadow;

        /// <summary>
        /// Gets or sets the mask of the underlying <see cref="Windows.UI.Composition.DropShadow"/>.
        /// Allows for a custom <see cref="Windows.UI.Composition.CompositionBrush"/> to be set.
        /// </summary>
        public CompositionBrush Mask
        {
            get
            {
                return _dropShadow?.Mask;
            }

            set
            {
                if (_dropShadow != null)
                {
                    _dropShadow.Mask = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the blur radius of the drop shadow.
        /// </summary>
        public double BlurRadius
        {
            get
            {
                return (double)GetValue(BlurRadiusProperty);
            }

            set
            {
                SetValue(BlurRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the drop shadow.
        /// </summary>
        public Color Color
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }

            set
            {
                SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the x offset of the drop shadow.
        /// </summary>
        public double OffsetX
        {
            get
            {
                return (double)GetValue(OffsetXProperty);
            }

            set
            {
                SetValue(OffsetXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the y offset of the drop shadow.
        /// </summary>
        public double OffsetY
        {
            get
            {
                return (double)GetValue(OffsetYProperty);
            }

            set
            {
                SetValue(OffsetYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the z offset of the drop shadow.
        /// </summary>
        public double OffsetZ
        {
            get
            {
                return (double)GetValue(OffsetZProperty);
            }

            set
            {
                SetValue(OffsetZProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the drop shadow.
        /// </summary>
        public double ShadowOpacity
        {
            get
            {
                return (double)GetValue(ShadowOpacityProperty);
            }

            set
            {
                SetValue(ShadowOpacityProperty, value);
            }
        }

        private static void OnBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                var panel = d as DropShadowPanel;
                panel?.OnBlurRadiusChanged((double)e.NewValue);
            }
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                var panel = d as DropShadowPanel;
                panel?.OnColorChanged((Color)e.NewValue);
            }
        }

        private static void OnOffsetXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                var panel = d as DropShadowPanel;
                panel?.OnOffsetXChanged((double)e.NewValue);
            }
        }

        private static void OnOffsetYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                var panel = d as DropShadowPanel;
                panel?.OnOffsetYChanged((double)e.NewValue);
            }
        }

        private static void OnOffsetZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                var panel = d as DropShadowPanel;
                panel?.OnOffsetZChanged((double)e.NewValue);
            }
        }

        private static void OnShadowOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported)
            {
                var panel = d as DropShadowPanel;
                panel?.OnShadowOpacityChanged((double)e.NewValue);
            }
        }
    }
}
