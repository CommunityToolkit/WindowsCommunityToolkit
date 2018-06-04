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
    /// Brush which applies a SepiaEffect to the Backdrop. http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_SepiaEffect.htm
    /// </summary>
    public class BackdropSepiaBrush : XamlCompositionBrushBase
    {
        /// <summary>
        /// Identifies the <see cref="Intensity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IntensityProperty = DependencyProperty.Register(
            nameof(Intensity),
            typeof(double),
            typeof(BackdropSepiaBrush),
            new PropertyMetadata(0.5, new PropertyChangedCallback(OnIntensityChanged)));

        /// <summary>
        /// Gets or sets the amount of gaussian blur to apply to the background.
        /// </summary>
        public double Intensity
        {
            get { return (double)GetValue(IntensityProperty); }
            set { SetValue(IntensityProperty, value); }
        }

        private static void OnIntensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (BackdropSepiaBrush)d;

            // Clamp Value as per docs http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_SepiaEffect.htm
            var value = (float)(double)e.NewValue;
            if (value > 1.0)
            {
                brush.Intensity = 1.0;
            }
            else if (value < 0.0)
            {
                brush.Intensity = 0.0;
            }

            // Unbox and set a new blur amount if the CompositionBrush exists.
            brush.CompositionBrush?.Properties.InsertScalar("Sepia.Intensity", (float)brush.Intensity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackdropSepiaBrush"/> class.
        /// </summary>
        public BackdropSepiaBrush()
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
                var graphicsEffect = new SepiaEffect
                {
                    Name = "Sepia",
                    Intensity = (float)Intensity,
                    Source = new CompositionEffectSourceParameter("backdrop")
                };

                var effectFactory = Window.Current.Compositor.CreateEffectFactory(graphicsEffect, new[] { "Sepia.Intensity" });
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
