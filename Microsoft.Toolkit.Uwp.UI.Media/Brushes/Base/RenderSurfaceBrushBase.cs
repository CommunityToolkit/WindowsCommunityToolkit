// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Base class for RenderSurface brushes.
    /// </summary>
    public abstract class RenderSurfaceBrushBase : XamlCompositionBrushBase
    {
        /// <summary>
        /// Event which indicates that the components of the brush have changed.
        /// </summary>
        public event EventHandler<EventArgs> Updated;

        /// <summary>
        /// Gets or sets the CompositionSurface associated with the brush.
        /// </summary>
        protected IRenderSurface RenderSurface { get; set; }

        /// <summary>
        /// The initialization <see cref="AsyncMutex"/> instance.
        /// </summary>
        private readonly AsyncMutex connectedMutex = new();

        /// <summary>
        /// Gets the associated CompositionBrush
        /// </summary>
        internal CompositionBrush Brush
        {
            get
            {
                return CompositionBrush;
            }
        }

        /// <summary>
        /// SurfaceWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty SurfaceWidthProperty = DependencyProperty.Register(
            "SurfaceWidth",
            typeof(double),
            typeof(RenderSurfaceBrushBase),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the width of the Brush Surface.
        /// </summary>
        public double SurfaceWidth
        {
            get => (double)GetValue(SurfaceWidthProperty);
            set => SetValue(SurfaceWidthProperty, value);
        }

        /// <summary>
        /// SurfaceHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty SurfaceHeightProperty = DependencyProperty.Register(
            "SurfaceHeight",
            typeof(double),
            typeof(RenderSurfaceBrushBase),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the height of the Brush Surface.
        /// </summary>
        public double SurfaceHeight
        {
            get => (double)GetValue(SurfaceHeightProperty);
            set => SetValue(SurfaceHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="ICompositionGenerator"/> instance.
        /// </summary>
        protected ICompositionGenerator Generator { get; set; }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes.
        /// </summary>
        /// <param name="d">The object whose property has changed.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (RenderSurfaceBrushBase)d;

            if (brush.SurfaceWidth < 0d)
            {
                throw new ArgumentException("SurfaceWidth must be a positive number!");
            }

            if (brush.SurfaceHeight < 0d)
            {
                throw new ArgumentException("SurfaceHeight must be a positive number!");
            }

            // Recreate the Render Surface Brush on any property change.
            brush.Refresh();
        }

        /// <summary>
        /// Gets the CompositionGenerator Instance.
        /// </summary>
        protected void GetGeneratorInstance()
        {
            if (Window.Current != null)
            {
                Generator = CompositionGenerator.Instance;

                Generator.DeviceReplaced += OnDeviceReplaced;
            }
        }

        /// <inheritdoc/>
        protected override async void OnConnected()
        {
            using (await connectedMutex.LockAsync())
            {
                if (CompositionBrush == null)
                {
                    GetGeneratorInstance();

                    OnSurfaceBrushUpdated(true);
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
                    Generator = null;
                }

                // Dispose Brush resources
                OnSurfaceBrushDisposed();

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
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void OnDeviceReplaced(object sender, object e)
        {
            OnDisconnected();
            OnConnected();
        }

        /// <summary>
        /// Invoked whenever any brush property is updated.
        /// </summary>
        /// <param name="createSurface">Indicates whether the surface needs to be created.</param>
        protected virtual void OnSurfaceBrushUpdated(bool createSurface = false)
        {
            Updated?.Invoke(this, null);
        }

        /// <summary>
        /// Invoked whenever any brush property is updated.
        /// </summary>
        protected virtual void OnSurfaceBrushDisposed()
        {
        }

        /// <summary>
        /// Redraws the brush content
        /// </summary>
        public void Refresh()
        {
            OnSurfaceBrushUpdated();
        }

        /// <summary>
        /// Checks if the URI starts with http: or https:.
        /// </summary>
        /// <param name="uri">URI.</param>
        /// <returns>True if it does, otherwise false.</returns>
        protected static bool IsHttpUri(Uri uri)
        {
            return uri != null && uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }
    }
}
