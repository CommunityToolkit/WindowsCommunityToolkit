using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions;
using Microsoft.Toolkit.Uwp.UI.Media.Surface;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Base class for RenderSurface brushes
    /// </summary>
    public abstract class RenderSurfaceBrushBase : XamlCompositionBrushBase
    {
        /// <summary>
        /// The initialization <see cref="AsyncMutex"/> instance.
        /// </summary>
        private readonly AsyncMutex connectedMutex = new AsyncMutex();

        /// <summary>
        /// SurfaceWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty SurfaceWidthProperty = DependencyProperty.Register(
            "SurfaceWidth",
            typeof(float),
            typeof(RenderSurfaceBrushBase),
            new PropertyMetadata(0f, OnSurfaceWidthChanged));

        /// <summary>
        /// SurfaceHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty SurfaceHeightProperty = DependencyProperty.Register(
            "SurfaceHeight",
            typeof(float),
            typeof(RenderSurfaceBrushBase),
            new PropertyMetadata(0f, OnSurfaceHeightChanged));

        /// <summary>
        /// Gets or sets the width of the Brush Surface.
        /// </summary>
        public float SurfaceWidth
        {
            get => (float)GetValue(SurfaceWidthProperty);
            set => SetValue(SurfaceWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the height of the Brush Surface.
        /// </summary>
        public float SurfaceHeight
        {
            get => (float)GetValue(SurfaceHeightProperty);
            set => SetValue(SurfaceHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="ICompositionGenerator"/> instance.
        /// </summary>
        protected ICompositionGenerator Generator { get; set; }

        /// <summary>
        /// Handles changes to the SurfaceWidth property.
        /// </summary>
        /// <param name="d">RenderSurfaceBrushBase</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnSurfaceWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (RenderSurfaceBrushBase)d;
            target.OnSurfaceWidthChanged();
        }

        /// <summary>
        /// Handles changes to the SurfaceHeight property.
        /// </summary>
        /// <param name="d">RenderSurfaceBrushBase</param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnSurfaceHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (RenderSurfaceBrushBase)d;
            brush.OnSurfaceHeightChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the SurfaceWidth property.
        /// </summary>
        private void OnSurfaceWidthChanged()
        {
            OnSurfaceBrushUpdated();
        }

        /// <summary>
        /// Instance handler for the changes to the SurfaceHeight property.
        /// </summary>
        private void OnSurfaceHeightChanged()
        {
            OnSurfaceBrushUpdated();
        }

        /// <inheritdoc/>
        protected override async void OnConnected()
        {
            using (await connectedMutex.LockAsync())
            {
                if (CompositionBrush == null)
                {
                    Generator = CompositionGenerator.Instance;

                    Generator.DeviceReplaced += OnDeviceReplaced;

                    OnSurfaceBrushUpdated();
                }
            }

            base.OnConnected();
        }

        /// <inheritdoc/>
        protected override async void OnDisconnected()
        {
            using (await connectedMutex.LockAsync())
            {
                if (Generator != null)
                {
                    Generator.DeviceReplaced -= OnDeviceReplaced;
                    Generator.Dispose();
                }

                // Dispose of composition resources when no longer in use.
                if (CompositionBrush != null)
                {
                    CompositionBrush.Dispose();
                    CompositionBrush = null;
                }
            }

            base.OnDisconnected();
        }

        /// <summary>
        /// Handler for the DeviceReplaced event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void OnDeviceReplaced(object sender, object e)
        {
            OnDisconnected();
            OnConnected();
        }

        /// <summary>
        /// Invoked whenever any brush property is updated.
        /// </summary>
        protected virtual void OnSurfaceBrushUpdated()
        {
        }
    }
}
