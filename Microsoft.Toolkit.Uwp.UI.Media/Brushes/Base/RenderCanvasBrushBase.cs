// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Abstract base class for SolidColorCanvasBrush, LinearGradientCanvasBrush and RadialGradientCanvasBrush which are XAML equivalents of Win2d brushes.
    /// </summary>
    public abstract class RenderCanvasBrushBase : DependencyObject, IDisposable
    {
        /// <summary>
        /// Event which indicates that the components of the brush have changed.
        /// </summary>
        public event EventHandler<EventArgs> Updated;

        private bool _disposedValue;

        /// <summary>
        /// Gets or sets the associated CanvasBrush.
        /// </summary>
        public ICanvasBrush CanvasBrush { get; protected set; }

        /// <summary>
        /// Opacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register(
            "Opacity",
            typeof(double),
            typeof(RenderCanvasBrushBase),
            new PropertyMetadata(1d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the opacity of the brush.
        /// </summary>
        public double Opacity
        {
            get => (double)GetValue(OpacityProperty);
            set => SetValue(OpacityProperty, value);
        }

        /// <summary>
        /// Transform Dependency Property
        /// </summary>
        public static readonly DependencyProperty TransformProperty = DependencyProperty.Register(
            "Transform",
            typeof(MatrixTransform),
            typeof(RenderCanvasBrushBase),
            new PropertyMetadata(new MatrixTransform { Matrix = Matrix.Identity }, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the transform matrix applied to the brush.
        /// </summary>
        public MatrixTransform Transform
        {
            get => (MatrixTransform)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }

        /// <summary>
        /// Refreshes the CanvasBrush when any of the properties are updated
        /// </summary>
        protected virtual void OnUpdated()
        {
            RaiseUpdatedEvent();
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
            OnUpdated();
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

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = (RenderCanvasBrushBase)d;

            // Recreate the canvas brush on any property change.
            brush.Refresh();
        }
    }
}
