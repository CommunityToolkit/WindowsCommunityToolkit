// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//// Example brush from https://blogs.windows.com/buildingapps/2017/07/18/working-brushes-content-xaml-visual-layer-interop-part-one/#z70vPv1QMAvZsceo.97

using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// The <see cref="BackdropInvertBrush"/> is a <see cref="Brush"/> which inverts whatever is behind it in the application.
    /// </summary>
    public class BackdropInvertBrush : XamlCompositionBrushBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackdropInvertBrush"/> class.
        /// </summary>
        public BackdropInvertBrush()
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

                // Use a Win2D invert affect applied to a CompositionBackdropBrush.
                var graphicsEffect = new InvertEffect
                {
                    Name = "Invert",
                    Source = new CompositionEffectSourceParameter("backdrop")
                };

                var effectFactory = Window.Current.Compositor.CreateEffectFactory(graphicsEffect);
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
