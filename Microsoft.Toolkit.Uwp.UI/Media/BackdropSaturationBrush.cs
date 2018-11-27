// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Brush which applies a SaturationEffect to the Backdrop. http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_SaturationEffect.htm
    /// </summary>
    public class BackdropSaturationBrush : XamlCompositionBrushBase
    {
        /// <summary>
        /// Identifies the <see cref="Saturation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            nameof(Saturation),
            typeof(double),
            typeof(BackdropSaturationBrush),
            new PropertyMetadata(0.5, new PropertyChangedCallback(OnSaturationChanged)));

        /// <summary>
        /// Gets or sets the amount of gaussian blur to apply to the background.
        /// </summary>
        public double Saturation
        {
            get { return (double)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }

        private static void OnSaturationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (BackdropSaturationBrush)d;

            // Clamp Value as per docs http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_SaturationEffect.htm
            var value = (float)(double)e.NewValue;
            if (value > 1.0)
            {
                brush.Saturation = 1.0;
            }
            else if (value < 0.0)
            {
                brush.Saturation = 0.0;
            }

            // Unbox and set a new blur amount if the CompositionBrush exists.
            brush.CompositionBrush?.Properties.InsertScalar("Saturation.Saturation", (float)brush.Saturation);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackdropSaturationBrush"/> class.
        /// </summary>
        public BackdropSaturationBrush()
        {
        }

        /// <summary>
        /// Initializes the Composition Brush.
        /// </summary>
        protected override void OnConnected()
        {
            // Delay creating composition resources until they're required.
            if (CompositionBrush == null)
            {
                // Abort if effects aren't supported.
                if (!CompositionCapabilities.GetForCurrentView().AreEffectsSupported())
                {
                    return;
                }

                var backdrop = Window.Current.Compositor.CreateBackdropBrush();

                // Use a Win2D blur affect applied to a CompositionBackdropBrush.
                var graphicsEffect = new SaturationEffect
                {
                    Name = "Saturation",
                    Saturation = (float)Saturation,
                    Source = new CompositionEffectSourceParameter("backdrop")
                };

                var effectFactory = Window.Current.Compositor.CreateEffectFactory(graphicsEffect, new[] { "Saturation.Saturation" });
                var effectBrush = effectFactory.CreateBrush();

                effectBrush.SetSourceParameter("backdrop", backdrop);

                CompositionBrush = effectBrush;
            }
        }

        /// <summary>
        /// Deconstructs the Composition Brush.
        /// </summary>
        protected override void OnDisconnected()
        {
            // Dispose of composition resources when no longer in use.
            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }
    }
}
