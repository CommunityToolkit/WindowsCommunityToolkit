// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

//// Example brush from https://blogs.windows.com/buildingapps/2017/07/18/working-brushes-content-xaml-visual-layer-interop-part-one/#z70vPv1QMAvZsceo.97

using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Brushes
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
