// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Provides a base class for objects that define geometric shapes using CanvasGeometry.
    /// </summary>
    public abstract class CanvasCoreGeometry : DependencyObject, ICanvasPathGeometry, IDisposable
    {
        /// <summary>
        /// Event to notify that the properties of this class have been updated.
        /// </summary>
        public event EventHandler<EventArgs> Updated;

        private bool _disposedValue;

        /// <summary>
        /// Geometry Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeometryProperty = DependencyProperty.Register(
            "Geometry",
            typeof(CanvasGeometry),
            typeof(CanvasCoreGeometry),
            new PropertyMetadata(null, OnGeometryChanged));

        /// <summary>
        /// Gets or sets the associated Win2d CanvasGeometry.
        /// </summary>
        public CanvasGeometry Geometry
        {
            get => (CanvasGeometry)GetValue(GeometryProperty);
            protected set => SetValue(GeometryProperty, value);
        }

        /// <summary>
        /// Handles changes to the Geometry property.
        /// </summary>
        /// <param name="d">CanvasCoreGeometry.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var coreGeometry = (CanvasCoreGeometry)d;
            coreGeometry.OnGeometryChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the Geometry dependency property.
        /// </summary>
        protected virtual void OnGeometryChanged()
        {
        }

        /// <summary>
        /// Method to be called when any of the parameters affecting the Geometry is updated.
        /// </summary>
        protected virtual void OnUpdateGeometry()
        {
            Updated?.Invoke(this, null);
        }

        /// <summary>
        /// Raises the Updated event.
        /// </summary>
        protected void RaiseUpdatedEvent()
        {
            Updated?.Invoke(this, null);
        }

        /// <summary>
        /// Call this method to redraw its Geometry (usually when <see cref="CompositionGenerator.DeviceReplaced"/> event is raised).
        /// </summary>
        public void Refresh()
        {
            OnUpdateGeometry();
        }

        /// <summary>
        /// Disposes the resources used by the CanvasCoreGeometry and its derivatives
        /// </summary>
        /// <param name="disposing">Flag to indicate if we are disposing the managed objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Geometry.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
