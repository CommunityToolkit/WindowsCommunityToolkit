// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Represents a Geometry defined by the combination of two <see cref="CanvasCoreGeometry"/> objects.
    /// </summary>
    public class CanvasCombinedGeometry : CanvasCoreGeometry
    {
        private WeakEventListener<CanvasCoreGeometry, object, EventArgs> _geometry1UpdateListener;
        private WeakEventListener<CanvasCoreGeometry, object, EventArgs> _geometry2UpdateListener;

        /// <summary>
        /// Geometry1 Dependency Property
        /// </summary>
        public static readonly DependencyProperty Geometry1Property = DependencyProperty.Register(
            "Geometry1",
            typeof(CanvasCoreGeometry),
            typeof(CanvasCombinedGeometry),
            new PropertyMetadata(null, OnGeometry1Changed));

        /// <summary>
        /// Gets or sets the first <see cref="CanvasCoreGeometry"/> object to combine.
        /// </summary>
        public CanvasCoreGeometry Geometry1
        {
            get => (CanvasCoreGeometry)GetValue(Geometry1Property);
            set => SetValue(Geometry1Property, value);
        }

        /// <summary>
        /// Handles changes to the Geometry1 property.
        /// </summary>
        /// <param name="d"><see cref="CanvasCombinedGeometry"/>.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnGeometry1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combinedGeometry = (CanvasCombinedGeometry)d;
            combinedGeometry.OnGeometry1Changed();
        }

        /// <summary>
        /// Instance handler for the changes to the Geometry1 dependency property.
        /// </summary>
        private void OnGeometry1Changed()
        {
            _geometry1UpdateListener?.Detach();
            _geometry1UpdateListener = null;

            if (Geometry1 != null)
            {
                _geometry1UpdateListener = new WeakEventListener<CanvasCoreGeometry, object, EventArgs>(Geometry1)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            OnUpdateGeometry();
                        });
                    }
                };

                Geometry1.Updated += _geometry1UpdateListener.OnEvent;

                OnUpdateGeometry();
            }
        }

        /// <summary>
        /// Geometry2 Dependency Property
        /// </summary>
        public static readonly DependencyProperty Geometry2Property = DependencyProperty.Register(
            "Geometry2",
            typeof(CanvasCoreGeometry),
            typeof(CanvasCombinedGeometry),
            new PropertyMetadata(null, OnGeometry2Changed));

        /// <summary>
        /// Gets or sets the second <see cref="CanvasCoreGeometry"/> to combine.
        /// </summary>
        public CanvasCoreGeometry Geometry2
        {
            get => (CanvasCoreGeometry)GetValue(Geometry2Property);
            set => SetValue(Geometry2Property, value);
        }

        /// <summary>
        /// Handles changes to the Geometry2 property.
        /// </summary>
        /// <param name="d"><see cref="CanvasCombinedGeometry" />.</param>
        /// <param name="e">DependencyProperty changed event arguments.</param>
        private static void OnGeometry2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combinedGeometry = (CanvasCombinedGeometry)d;
            combinedGeometry.OnGeometry2Changed();
        }

        /// <summary>
        /// Instance handler for the changes to the Geometry2 dependency property.
        /// </summary>
        private void OnGeometry2Changed()
        {
            _geometry2UpdateListener?.Detach();
            _geometry2UpdateListener = null;

            if (Geometry2 != null)
            {
                _geometry2UpdateListener = new WeakEventListener<CanvasCoreGeometry, object, EventArgs>(Geometry2)
                {
                    OnEventAction = async (instance, source, args) =>
                    {
                        await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            OnUpdateGeometry();
                        });
                    }
                };

                Geometry2.Updated += _geometry2UpdateListener.OnEvent;

                OnUpdateGeometry();
            }
        }

        /// <summary>
        /// Transform Dependency Property
        /// </summary>
        public static readonly DependencyProperty TransformProperty = DependencyProperty.Register(
            "Transform",
            typeof(MatrixTransform),
            typeof(CanvasCombinedGeometry),
            new PropertyMetadata(Matrix3x2.Identity.ToMatrixTransform(), OnPropertyChanged));

        /// <summary>
        /// Gets or sets the MatrixTransform to be applied to Geometry2 before combining with Geometry1.
        /// </summary>
        public MatrixTransform Transform
        {
            get => (MatrixTransform)GetValue(TransformProperty);
            set => SetValue(TransformProperty, value);
        }

        /// <summary>
        /// GeometryCombineMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty GeometryCombineModeProperty = DependencyProperty.Register(
            "GeometryCombineMode",
            typeof(CanvasGeometryCombine),
            typeof(CanvasCombinedGeometry),
            new PropertyMetadata(CanvasGeometryCombine.Union, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the method by which the geometries specified by <see cref="Geometry1"/> and <see cref="Geometry2"/> are meant to be combined.
        /// </summary>
        public CanvasGeometryCombine GeometryCombineMode
        {
            get => (CanvasGeometryCombine)GetValue(GeometryCombineModeProperty);
            set => SetValue(GeometryCombineModeProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the Brush changes
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geometry = (CanvasCombinedGeometry)d;

            // Recreate the geometry on any property change.
            geometry.OnUpdateGeometry();
        }

        /// <inheritdoc/>
        protected override void OnUpdateGeometry()
        {
            if (Geometry1?.Geometry == null || Geometry2?.Geometry == null)
            {
                Geometry = null;
                return;
            }

            Geometry = Geometry1.Geometry.CombineWith(Geometry2.Geometry, Transform.ToMatrix3x2(), GeometryCombineMode);

            RaiseUpdatedEvent();
        }
    }
}
