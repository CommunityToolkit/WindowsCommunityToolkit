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
    /// A brush which alters the colors of whatever is behind it in the application by applying a per-channel gamma transfer function.  See https://microsoft.github.io/Win2D/html/T_Microsoft_Graphics_Canvas_Effects_GammaTransferEffect.htm.
    /// </summary>
    public class BackdropGammaTransferBrush : XamlCompositionBrushBase
    {
        /// <summary>
        /// Identifies the <see cref="AlphaAmplitude"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlphaAmplitudeProperty =
            DependencyProperty.Register(nameof(AlphaAmplitude), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(1.0, OnScalarPropertyChangedHelper(nameof(AlphaAmplitude))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the alpha chennel.
        /// </summary>
        public double AlphaAmplitude
        {
            get { return (double)GetValue(AlphaAmplitudeProperty); }
            set { SetValue(AlphaAmplitudeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AlphaDisable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlphaDisableProperty =
            DependencyProperty.Register(nameof(AlphaDisable), typeof(bool), typeof(BackdropGammaTransferBrush), new PropertyMetadata(false, OnBooleanPropertyChangedHelper(nameof(AlphaDisable))));

        /// <summary>
        /// Gets or sets a value indicating whether to disable alpha transfer.
        /// </summary>
        public bool AlphaDisable
        {
            get { return (bool)GetValue(AlphaDisableProperty); }
            set { SetValue(AlphaDisableProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AlphaExponent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlphaExponentProperty =
            DependencyProperty.Register(nameof(AlphaExponent), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(1.0, OnScalarPropertyChangedHelper(nameof(AlphaExponent))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the alpha chennel.
        /// </summary>
        public double AlphaExponent
        {
            get { return (double)GetValue(AlphaExponentProperty); }
            set { SetValue(AlphaExponentProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AlphaOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlphaOffsetProperty =
            DependencyProperty.Register(nameof(AlphaOffset), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(0.0, OnScalarPropertyChangedHelper(nameof(AlphaOffset))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the alpha chennel.
        /// </summary>
        public double AlphaOffset
        {
            get { return (double)GetValue(AlphaOffsetProperty); }
            set { SetValue(AlphaOffsetProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BlueAmplitude"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlueAmplitudeProperty =
            DependencyProperty.Register(nameof(BlueAmplitude), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(1.0, OnScalarPropertyChangedHelper(nameof(BlueAmplitude))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Blue chennel.
        /// </summary>
        public double BlueAmplitude
        {
            get { return (double)GetValue(BlueAmplitudeProperty); }
            set { SetValue(BlueAmplitudeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BlueDisable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlueDisableProperty =
            DependencyProperty.Register(nameof(BlueDisable), typeof(bool), typeof(BackdropGammaTransferBrush), new PropertyMetadata(false, OnBooleanPropertyChangedHelper(nameof(BlueDisable))));

        /// <summary>
        /// Gets or sets a value indicating whether to disable Blue transfer.
        /// </summary>
        public bool BlueDisable
        {
            get { return (bool)GetValue(BlueDisableProperty); }
            set { SetValue(BlueDisableProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BlueExponent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlueExponentProperty =
            DependencyProperty.Register(nameof(BlueExponent), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(1.0, OnScalarPropertyChangedHelper(nameof(BlueExponent))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Blue chennel.
        /// </summary>
        public double BlueExponent
        {
            get { return (double)GetValue(BlueExponentProperty); }
            set { SetValue(BlueExponentProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="BlueOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BlueOffsetProperty =
            DependencyProperty.Register(nameof(BlueOffset), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(0.0, OnScalarPropertyChangedHelper(nameof(BlueOffset))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Blue chennel.
        /// </summary>
        public double BlueOffset
        {
            get { return (double)GetValue(BlueOffsetProperty); }
            set { SetValue(BlueOffsetProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GreenAmplitude"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GreenAmplitudeProperty =
            DependencyProperty.Register(nameof(GreenAmplitude), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(1.0, OnScalarPropertyChangedHelper(nameof(GreenAmplitude))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Green chennel.
        /// </summary>
        public double GreenAmplitude
        {
            get { return (double)GetValue(GreenAmplitudeProperty); }
            set { SetValue(GreenAmplitudeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GreenDisable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GreenDisableProperty =
            DependencyProperty.Register(nameof(GreenDisable), typeof(bool), typeof(BackdropGammaTransferBrush), new PropertyMetadata(false, OnBooleanPropertyChangedHelper(nameof(GreenDisable))));

        /// <summary>
        /// Gets or sets a value indicating whether to disable Green transfer.
        /// </summary>
        public bool GreenDisable
        {
            get { return (bool)GetValue(GreenDisableProperty); }
            set { SetValue(GreenDisableProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GreenExponent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GreenExponentProperty =
            DependencyProperty.Register(nameof(GreenExponent), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(1.0, OnScalarPropertyChangedHelper(nameof(GreenExponent))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Green chennel.
        /// </summary>
        public double GreenExponent
        {
            get { return (double)GetValue(GreenExponentProperty); }
            set { SetValue(GreenExponentProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GreenOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GreenOffsetProperty =
            DependencyProperty.Register(nameof(GreenOffset), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(0.0, OnScalarPropertyChangedHelper(nameof(GreenOffset))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Green chennel.
        /// </summary>
        public double GreenOffset
        {
            get { return (double)GetValue(GreenOffsetProperty); }
            set { SetValue(GreenOffsetProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RedAmplitude"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedAmplitudeProperty =
            DependencyProperty.Register(nameof(RedAmplitude), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(1.0, OnScalarPropertyChangedHelper(nameof(RedAmplitude))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Red chennel.
        /// </summary>
        public double RedAmplitude
        {
            get { return (double)GetValue(RedAmplitudeProperty); }
            set { SetValue(RedAmplitudeProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RedDisable"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedDisableProperty =
            DependencyProperty.Register(nameof(RedDisable), typeof(bool), typeof(BackdropGammaTransferBrush), new PropertyMetadata(false, OnBooleanPropertyChangedHelper(nameof(RedDisable))));

        /// <summary>
        /// Gets or sets a value indicating whether to disable Red transfer.
        /// </summary>
        public bool RedDisable
        {
            get { return (bool)GetValue(RedDisableProperty); }
            set { SetValue(RedDisableProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RedExponent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedExponentProperty =
            DependencyProperty.Register(nameof(RedExponent), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(1.0, OnScalarPropertyChangedHelper(nameof(RedExponent))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Red chennel.
        /// </summary>
        public double RedExponent
        {
            get { return (double)GetValue(RedExponentProperty); }
            set { SetValue(RedExponentProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RedOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedOffsetProperty =
            DependencyProperty.Register(nameof(RedOffset), typeof(double), typeof(BackdropGammaTransferBrush), new PropertyMetadata(0.0, OnScalarPropertyChangedHelper(nameof(RedOffset))));

        /// <summary>
        /// Gets or sets the amount of scale to apply to the Red chennel.
        /// </summary>
        public double RedOffset
        {
            get { return (double)GetValue(RedOffsetProperty); }
            set { SetValue(RedOffsetProperty, value); }
        }

        private static PropertyChangedCallback OnScalarPropertyChangedHelper(string propertyname)
        {
            return new PropertyChangedCallback((d, e) =>
            {
                var brush = (BackdropGammaTransferBrush)d;

                // Unbox and set a new blur amount if the CompositionBrush exists.
                brush.CompositionBrush?.Properties.InsertScalar("GammaTransfer." + propertyname, (float)(double)e.NewValue);
            });
        }

        private static PropertyChangedCallback OnBooleanPropertyChangedHelper(string propertyname)
        {
            return new PropertyChangedCallback((d, e) =>
            {
                var brush = (BackdropGammaTransferBrush)d;

                // We can't animate our boolean properties so recreate our internal brush.
                brush.OnDisconnected();
                brush.OnConnected();
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackdropGammaTransferBrush"/> class.
        /// </summary>
        public BackdropGammaTransferBrush()
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
                var graphicsEffect = new GammaTransferEffect
                {
                    Name = "GammaTransfer",
                    AlphaAmplitude = (float)AlphaAmplitude,
                    AlphaDisable = AlphaDisable,
                    AlphaExponent = (float)AlphaExponent,
                    AlphaOffset = (float)AlphaOffset,
                    RedAmplitude = (float)RedAmplitude,
                    RedDisable = RedDisable,
                    RedExponent = (float)RedExponent,
                    RedOffset = (float)RedOffset,
                    GreenAmplitude = (float)GreenAmplitude,
                    GreenDisable = GreenDisable,
                    GreenExponent = (float)GreenExponent,
                    GreenOffset = (float)GreenOffset,
                    BlueAmplitude = (float)BlueAmplitude,
                    BlueDisable = BlueDisable,
                    BlueExponent = (float)BlueExponent,
                    BlueOffset = (float)BlueOffset,
                    Source = new CompositionEffectSourceParameter("backdrop")
                };

                var effectFactory = Window.Current.Compositor.CreateEffectFactory(graphicsEffect, new[]
                {
                    "GammaTransfer.AlphaAmplitude",
                    "GammaTransfer.AlphaExponent",
                    "GammaTransfer.AlphaOffset",
                    "GammaTransfer.RedAmplitude",
                    "GammaTransfer.RedExponent",
                    "GammaTransfer.RedOffset",
                    "GammaTransfer.GreenAmplitude",
                    "GammaTransfer.GreenExponent",
                    "GammaTransfer.GreenOffset",
                    "GammaTransfer.BlueAmplitude",
                    "GammaTransfer.BlueExponent",
                    "GammaTransfer.BlueOffset",
                });
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
