// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Brushes.Base;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Brush which applies a SaturationEffect to the Backdrop. http://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_SaturationEffect.htm
    /// </summary>
    public class BackdropSaturationBrush : XamlCompositionEffectBrushBase
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

            // Unbox and set a new blur amount if the CompositionBrush exists
            if (brush.CompositionBrush is CompositionBrush target)
            {
                brush.setter?.Invoke(target, (float)brush.Saturation);
            }
        }

        /// <summary>
        /// The <see cref="EffectSetter{T}"/> instance currently in use
        /// </summary>
        private EffectSetter<float> setter;

        /// <inheritdoc/>
        protected override PipelineBuilder OnBrushRequested()
        {
            return PipelineBuilder.FromBackdrop().Saturation((float)Saturation, out setter);
        }
    }
}
