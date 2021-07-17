// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Class which defines various properties which govern how the stroke is rendered.
    /// </summary>
    public class StrokeStyle : DependencyObject, IDisposable
    {
        private bool _disposedValue;
        private CanvasStrokeStyle _canvasStrokeStyle;

        /// <summary>
        /// Event to notify that the properties of this class have been updated.
        /// </summary>
        public event EventHandler<EventArgs> Updated;

        /// <summary>
        /// CustomDashStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty CustomDashStyleProperty = DependencyProperty.Register(
            "CustomDashStyle",
            typeof(string),
            typeof(StrokeStyle),
            new PropertyMetadata(string.Empty, OnCustomDashStyleChanged));

        /// <summary>
        /// Gets or sets the an array describing a custom dash pattern. This overrides the DashStyle property, which is only used when CustomDashStyle is set to null.
        /// A custom dash style is an array whose elements specify the length of each dash and space in the pattern.
        /// The first element sets the length of a dash, the second element sets the length of a space, the third element sets the length of a dash, and so on.
        /// The length of each dash and space in the dash pattern is the product of the element value in the array and the stroke width.
        /// This array must contain an even number of elements.
        /// If the dash style is configured to contain a dash which is zero-length, that dash will only be visible with a cap style other than Flat.
        /// </summary>
        public string CustomDashStyle
        {
            get => (string)GetValue(CustomDashStyleProperty);
            set => SetValue(CustomDashStyleProperty, value);
        }

        /// <summary>
        /// Handles changes to the CustomDashStyle property.
        /// </summary>
        /// <param name="d"><see cref="StrokeStyle" /></param>
        /// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnCustomDashStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var strokeStyle = (StrokeStyle)d;
            strokeStyle.OnCustomDashStyleChanged();
        }

        /// <summary>
        /// Instance handler for the changes to the CustomDashStyle dependency property.
        /// </summary>
        private void OnCustomDashStyleChanged()
        {
            var result = new List<float>();

            if (!string.IsNullOrWhiteSpace(CustomDashStyle))
            {
                var arr = CustomDashStyle.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Any())
                {
                    if (arr.Length % 2 != 0)
                    {
                        throw new ArgumentException("CustomDashStyle must contain an even number of elements!");
                    }

                    foreach (var token in arr)
                    {
                        if (float.TryParse(token, out float num))
                        {
                            result.Add(num);
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid value! Cannot convert '{token}' to 'System.Single'!");
                        }
                    }
                }
            }

            ParsedCustomDashStyle = result.Any() ? result.ToArray() : null;

            OnUpdated();
        }

        /// <summary>
        /// Gets the custom dash style parsed from the input string.
        /// </summary>
        public float[] ParsedCustomDashStyle { get; private set; } = null;

        /// <summary>
        /// DashCap Dependency Property
        /// </summary>
        public static readonly DependencyProperty DashCapProperty = DependencyProperty.Register(
            "DashCap",
            typeof(CanvasCapStyle),
            typeof(StrokeStyle),
            new PropertyMetadata(CanvasCapStyle.Square, OnPropertyChanged));

        /// <summary>
        /// Gets or sets how the ends of each dash are drawn. Defaults to Square.
        /// If this is set to Flat, dots will have zero size so only dashes are visible.
        /// </summary>
        public CanvasCapStyle DashCap
        {
            get => (CanvasCapStyle)GetValue(DashCapProperty);
            set => SetValue(DashCapProperty, value);
        }

        /// <summary>
        /// DashOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty DashOffsetProperty = DependencyProperty.Register(
            "DashOffset",
            typeof(double),
            typeof(StrokeStyle),
            new PropertyMetadata(0d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets how far into the dash sequence the stroke will start.
        /// </summary>
        public double DashOffset
        {
            get => (double)GetValue(DashOffsetProperty);
            set => SetValue(DashOffsetProperty, value);
        }

        /// <summary>
        /// DashStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty DashStyleProperty = DependencyProperty.Register(
            "DashStyle",
            typeof(CanvasDashStyle),
            typeof(StrokeStyle),
            new PropertyMetadata(CanvasDashStyle.Solid, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the stroke's dash pattern. This is ignored if CustomDashStyle has been set.
        /// </summary>
        public CanvasDashStyle DashStyle
        {
            get => (CanvasDashStyle)GetValue(DashStyleProperty);
            set => SetValue(DashStyleProperty, value);
        }

        /// <summary>
        /// EndCap Dependency Property
        /// </summary>
        public static readonly DependencyProperty EndCapProperty = DependencyProperty.Register(
            "EndCap",
            typeof(CanvasCapStyle),
            typeof(StrokeStyle),
            new PropertyMetadata(CanvasCapStyle.Flat, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the type of shape used at the end of a stroke. Defaults to Flat.
        /// </summary>
        public CanvasCapStyle EndCap
        {
            get => (CanvasCapStyle)GetValue(EndCapProperty);
            set => SetValue(EndCapProperty, value);
        }

        /// <summary>
        /// LineJoin Dependency Property
        /// </summary>
        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin",
            typeof(CanvasLineJoin),
            typeof(StrokeStyle),
            new PropertyMetadata(CanvasLineJoin.Miter, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the type of joint used at the vertices of a shape's outline.
        /// </summary>
        public CanvasLineJoin LineJoin
        {
            get => (CanvasLineJoin)GetValue(LineJoinProperty);
            set => SetValue(LineJoinProperty, value);
        }

        /// <summary>
        /// MiterLimit Dependency Property
        /// </summary>
        public static readonly DependencyProperty MiterLimitProperty = DependencyProperty.Register(
            "MiterLimit",
            typeof(double),
            typeof(StrokeStyle),
            new PropertyMetadata(10d, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the limit on the ratio of the miter length to half the stroke's thickness.
        /// </summary>
        public double MiterLimit
        {
            get => (double)GetValue(MiterLimitProperty);
            set => SetValue(MiterLimitProperty, value);
        }

        /// <summary>
        /// StartCap Dependency Property
        /// </summary>
        public static readonly DependencyProperty StartCapProperty = DependencyProperty.Register(
            "StartCap",
            typeof(CanvasCapStyle),
            typeof(StrokeStyle),
            new PropertyMetadata(CanvasCapStyle.Flat, OnPropertyChanged));

        /// <summary>
        /// Gets or sets the type of shape used at the beginning of a stroke. Defaults to Flat.
        /// </summary>
        public CanvasCapStyle StartCap
        {
            get => (CanvasCapStyle)GetValue(StartCapProperty);
            set => SetValue(StartCapProperty, value);
        }

        /// <summary>
        /// TransformBehavior Dependency Property
        /// </summary>
        public static readonly DependencyProperty TransformBehaviorProperty = DependencyProperty.Register(
            "TransformBehavior",
            typeof(CanvasStrokeTransformBehavior),
            typeof(StrokeStyle),
            new PropertyMetadata(CanvasStrokeTransformBehavior.Normal, OnPropertyChanged));

        /// <summary>
        /// Gets or sets how the world transform, dots per inch (DPI), and stroke width affect the shape of the pen.
        /// </summary>
        public CanvasStrokeTransformBehavior TransformBehavior
        {
            get => (CanvasStrokeTransformBehavior)GetValue(TransformBehaviorProperty);
            set => SetValue(TransformBehaviorProperty, value);
        }

        /// <summary>
        /// Method that is called whenever the dependency properties of the StrokeStyle changes
        /// </summary>
        /// <param name="d">The object whose property has changed</param>
        /// <param name="e">Event arguments</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var strokeStyle = (StrokeStyle)d;

            // Recreate the canvas brush on any property change.
            strokeStyle.OnUpdated();
        }

        private void OnUpdated()
        {
            _canvasStrokeStyle = new CanvasStrokeStyle()
            {
                CustomDashStyle = ParsedCustomDashStyle,
                DashCap = DashCap,
                DashOffset = (float)DashOffset,
                DashStyle = DashStyle,
                EndCap = EndCap,
                LineJoin = LineJoin,
                MiterLimit = (float)MiterLimit,
                StartCap = StartCap,
                TransformBehavior = TransformBehavior
            };

            Updated?.Invoke(this, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrokeStyle"/> class.
        /// </summary>
        public StrokeStyle()
        {
            ParsedCustomDashStyle = new float[0];
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    ParsedCustomDashStyle = null;
                }

                // Free unmanaged resources (unmanaged objects), if any, and override finalizer
                _disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the CanvasStrokeStyle.
        /// </summary>
        /// <returns><see cref="CanvasStrokeStyle"/></returns>
        public CanvasStrokeStyle GetCanvasStrokeStyle()
        {
            return _canvasStrokeStyle;
        }
    }
}
