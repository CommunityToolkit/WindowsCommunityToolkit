// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// A Modern UI Radial Gauge using XAML and Composition API.
    /// </summary>
    //// All calculations are for a 200x200 square. The viewbox will do the rest.
    [TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = TrailPartName, Type = typeof(Path))]
    [TemplatePart(Name = ValueTextPartName, Type = typeof(TextBlock))]
    public class RadialGauge : Control
    {
        /// <summary>
        /// Identifies the Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(RadialGauge), new PropertyMetadata(0.0, OnScaleChanged));

        /// <summary>
        /// Identifies the Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(RadialGauge), new PropertyMetadata(100.0, OnScaleChanged));

        /// <summary>
        /// Identifies the ScaleWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleWidthProperty =
            DependencyProperty.Register(nameof(ScaleWidth), typeof(Double), typeof(RadialGauge), new PropertyMetadata(26.0, OnScaleChanged));

        /// <summary>
        /// Identifies the NeedleBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty NeedleBrushProperty =
            DependencyProperty.Register(nameof(NeedleBrush), typeof(SolidColorBrush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.Red), OnFaceChanged));

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(RadialGauge), new PropertyMetadata(0.0, OnValueChanged));

        /// <summary>
        /// Identifies the Unit dependency property.
        /// </summary>
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(nameof(Unit), typeof(string), typeof(RadialGauge), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the TrailBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TrailBrushProperty =
            DependencyProperty.Register(nameof(TrailBrush), typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        /// <summary>
        /// Identifies the ScaleBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register(nameof(ScaleBrush), typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        /// <summary>
        /// Identifies the ScaleTickBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleTickBrushProperty =
            DependencyProperty.Register(nameof(ScaleTickBrush), typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnFaceChanged));

        /// <summary>
        /// Identifies the TickBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TickBrushProperty =
            DependencyProperty.Register(nameof(TickBrush), typeof(SolidColorBrush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.White), OnFaceChanged));

        /// <summary>
        /// Identifies the ValueBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueBrushProperty =
            DependencyProperty.Register(nameof(ValueBrush), typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Identifies the UnitBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty UnitBrushProperty =
            DependencyProperty.Register(nameof(UnitBrush), typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Identifies the ValueStringFormat dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueStringFormatProperty =
            DependencyProperty.Register(nameof(ValueStringFormat), typeof(string), typeof(RadialGauge), new PropertyMetadata("N0"));

        /// <summary>
        /// Identifies the TickSpacing dependency property.
        /// </summary>
        public static readonly DependencyProperty TickSpacingProperty =
        DependencyProperty.Register(nameof(TickSpacing), typeof(int), typeof(RadialGauge), new PropertyMetadata(10, OnFaceChanged));

        /// <summary>
        /// Identifies the NeedleLength dependency property.
        /// </summary>
        public static readonly DependencyProperty NeedleLengthProperty =
            DependencyProperty.Register(nameof(NeedleLength), typeof(double), typeof(RadialGauge), new PropertyMetadata(100d, OnFaceChanged));

        /// <summary>
        /// Identifies the NeedleWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty NeedleWidthProperty =
            DependencyProperty.Register(nameof(NeedleWidth), typeof(double), typeof(RadialGauge), new PropertyMetadata(5d, OnFaceChanged));

        /// <summary>
        /// Identifies the ScalePadding dependency property.
        /// </summary>
        public static readonly DependencyProperty ScalePaddingProperty =
            DependencyProperty.Register(nameof(ScalePadding), typeof(double), typeof(RadialGauge), new PropertyMetadata(23d, OnFaceChanged));

        /// <summary>
        /// Identifies the ScaleTickWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleTickWidthProperty =
            DependencyProperty.Register(nameof(ScaleTickWidth), typeof(double), typeof(RadialGauge), new PropertyMetadata(2.5, OnFaceChanged));

        /// <summary>
        /// Identifies the TickWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty TickWidthProperty =
            DependencyProperty.Register(nameof(TickWidth), typeof(double), typeof(RadialGauge), new PropertyMetadata(5d, OnFaceChanged));

        /// <summary>
        /// Identifies the TickLength dependency property.
        /// </summary>
        public static readonly DependencyProperty TickLengthProperty =
            DependencyProperty.Register(nameof(TickLength), typeof(double), typeof(RadialGauge), new PropertyMetadata(18d, OnFaceChanged));

        /// <summary>
        /// Identifies the ValueAngle dependency property.
        /// </summary>
        protected static readonly DependencyProperty ValueAngleProperty =
            DependencyProperty.Register(nameof(ValueAngle), typeof(double), typeof(RadialGauge), new PropertyMetadata(null));

        // Template Parts.
        private const string ContainerPartName = "PART_Container";
        private const string ScalePartName = "PART_Scale";
        private const string TrailPartName = "PART_Trail";
        private const string ValueTextPartName = "PART_ValueText";

        // For convenience.
        private const double Degrees2Radians = Math.PI / 180;

        private const double MinAngle = -150.0;
        private const double MaxAngle = 150.0;

        private Compositor _compositor;
        private ContainerVisual _root;
        private SpriteVisual _needle;

        /// <summary>
        /// Create a default radial gauge control.
        /// </summary>
        public RadialGauge()
        {
            this.DefaultStyleKey = typeof(RadialGauge);
        }

        /// <summary>
        /// Gets or sets the minimum on the scale.
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum on the scale.
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the scale.
        /// </summary>
        public double ScaleWidth
        {
            get { return (Double)GetValue(ScaleWidthProperty); }
            set { SetValue(ScaleWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the unit measure.
        /// </summary>
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the needle brush.
        /// </summary>
        public SolidColorBrush NeedleBrush
        {
            get { return (SolidColorBrush)GetValue(NeedleBrushProperty); }
            set { SetValue(NeedleBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the trail brush.
        /// </summary>
        public Brush TrailBrush
        {
            get { return (Brush)GetValue(TrailBrushProperty); }
            set { SetValue(TrailBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale brush.
        /// </summary>
        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the scale tick brush.
        /// </summary>
        public SolidColorBrush ScaleTickBrush
        {
            get { return (SolidColorBrush)GetValue(ScaleTickBrushProperty); }
            set { SetValue(ScaleTickBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the outer tick brush.
        /// </summary>
        public SolidColorBrush TickBrush
        {
            get { return (SolidColorBrush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value brush.
        /// </summary>
        public Brush ValueBrush
        {
            get { return (Brush)GetValue(ValueBrushProperty); }
            set { SetValue(ValueBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the unit brush.
        /// </summary>
        public Brush UnitBrush
        {
            get { return (Brush)GetValue(UnitBrushProperty); }
            set { SetValue(UnitBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value string format.
        /// </summary>
        public string ValueStringFormat
        {
            get { return (string)GetValue(ValueStringFormatProperty); }
            set { SetValue(ValueStringFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the tick spacing, in units.
        /// </summary>
        public int TickSpacing
        {
            get { return (int)GetValue(TickSpacingProperty); }
            set { SetValue(TickSpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the needle length, in percentage of the gauge radius.
        /// </summary>
        public double NeedleLength
        {
            get { return (double)GetValue(NeedleLengthProperty); }
            set { SetValue(NeedleLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the needle width, in percentage of the gauge radius.
        /// </summary>
        public double NeedleWidth        {
            get { return (double)GetValue(NeedleWidthProperty); }
            set { SetValue(NeedleWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance of the scale from the outside of the control, in percentage of the gauge radius.
        /// </summary>
        public double ScalePadding
        {
            get { return (double)GetValue(ScalePaddingProperty); }
            set { SetValue(ScalePaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the scale ticks, in percentage of the gauge radius.
        /// </summary>
        public double ScaleTickWidth
        {
            get { return (double)GetValue(ScaleTickWidthProperty); }
            set { SetValue(ScaleTickWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the length of the ticks, in percentage of the gauge radius.
        /// </summary>
        public double TickLength
        {
            get { return (double)GetValue(TickLengthProperty); }
            set { SetValue(TickLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the ticks, in percentage of the gauge radius.
        /// </summary>
        public double TickWidth
        {
            get { return (double)GetValue(TickWidthProperty); }
            set { SetValue(TickWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle of the needle.
        /// </summary>
        protected double ValueAngle
        {
            get { return (double)GetValue(ValueAngleProperty); }
            set { SetValue(ValueAngleProperty, value); }
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            OnScaleChanged(this);

            base.OnApplyTemplate();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnValueChanged(d);
        }

        /// <summary>
        /// Updates the needle rotation, the trail, and the value text according to the new value.
        /// </summary>
        private static void OnValueChanged(DependencyObject d)
        {
            RadialGauge c = (RadialGauge)d;
            if (!double.IsNaN(c.Value))
            {
                var middleOfScale = 100 - c.ScalePadding - c.ScaleWidth / 2;
                var valueText = c.GetTemplateChild(ValueTextPartName) as TextBlock;
                c.ValueAngle = c.ValueToAngle(c.Value);

                // Needle
                if (c._needle != null)
                {
                    c._needle.RotationAngleInDegrees = (float)c.ValueAngle;
                }

                // Trail
                var trail = c.GetTemplateChild(TrailPartName) as Path;
                if (trail != null)
                {
                    if (c.ValueAngle > MinAngle)
                    {
                        trail.Visibility = Visibility.Visible;
                        var pg = new PathGeometry();
                        var pf = new PathFigure();
                        pf.IsClosed = false;
                        pf.StartPoint = c.ScalePoint(MinAngle, middleOfScale);
                        var seg = new ArcSegment();
                        seg.SweepDirection = SweepDirection.Clockwise;
                        seg.IsLargeArc = c.ValueAngle > (180 + MinAngle);
                        seg.Size = new Size(middleOfScale, middleOfScale);
                        seg.Point = c.ScalePoint(Math.Min(c.ValueAngle, MaxAngle), middleOfScale);  // On overflow, stop trail at MaxAngle.
                        pf.Segments.Add(seg);
                        pg.Figures.Add(pf);
                        trail.Data = pg;
                    }
                    else
                    {
                        trail.Visibility = Visibility.Collapsed;
                    }
                }

                // Value Text
                if (valueText != null)
                {
                    valueText.Text = c.Value.ToString(c.ValueStringFormat);
                }
            }
        }

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnScaleChanged(d);
        }

        /// <summary>
        /// Updates the background scale.
        /// </summary>
        private static void OnScaleChanged(DependencyObject d)
        {
            RadialGauge c = (RadialGauge)d;

            var scale = c.GetTemplateChild(ScalePartName) as Path;
            if (scale != null)
            {
                var pg = new PathGeometry();
                var pf = new PathFigure();
                pf.IsClosed = false;
                var middleOfScale = 100 - c.ScalePadding - c.ScaleWidth / 2;
                pf.StartPoint = c.ScalePoint(MinAngle, middleOfScale);
                var seg = new ArcSegment();
                seg.SweepDirection = SweepDirection.Clockwise;
                seg.IsLargeArc = true;
                seg.Size = new Size(middleOfScale, middleOfScale);
                seg.Point = c.ScalePoint(MaxAngle, middleOfScale);
                pf.Segments.Add(seg);
                pg.Figures.Add(pf);
                scale.Data = pg;

                OnFaceChanged(c);
            }
        }

        private static void OnFaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnFaceChanged(d);
        }

        /// <summary>
        /// Updates the face: ticks, scale ticks, and needle.
        /// </summary>
        private static void OnFaceChanged(DependencyObject d)
        {
            RadialGauge c = (RadialGauge)d;

            var container = c.GetTemplateChild(ContainerPartName) as Grid;
            if (container == null)
            {
                // Bad template.
                return;
            }

            c._root = container.GetVisual();
            c._root.Children.RemoveAll();
            c._compositor = c._root.Compositor;

            // Ticks.
            SpriteVisual tick;
            for (double i = c.Minimum; i <= c.Maximum; i += c.TickSpacing)
            {
                tick = c._compositor.CreateSpriteVisual();
                tick.Size = new Vector2((float)c.TickWidth, (float)c.TickLength);
                tick.Brush = c._compositor.CreateColorBrush(c.TickBrush.Color);
                tick.Offset = new Vector3(100 - (float)c.TickWidth / 2, 0.0f, 0);
                tick.CenterPoint = new Vector3((float)c.TickWidth / 2, 100.0f, 0);
                tick.RotationAngleInDegrees = (float)c.ValueToAngle(i);
                c._root.Children.InsertAtTop(tick);
            }

            // Scale Ticks.
            for (double i = c.Minimum; i <= c.Maximum; i += c.TickSpacing)
            {
                tick = c._compositor.CreateSpriteVisual();
                tick.Size = new Vector2((float)c.ScaleTickWidth, (float)c.ScaleWidth);
                tick.Brush = c._compositor.CreateColorBrush(c.ScaleTickBrush.Color);
                tick.Offset = new Vector3(100 - (float)c.ScaleTickWidth / 2, (float)c.ScalePadding, 0);
                tick.CenterPoint = new Vector3((float)c.ScaleTickWidth / 2, 100 - (float)c.ScalePadding, 0);
                tick.RotationAngleInDegrees = (float)c.ValueToAngle(i);
                c._root.Children.InsertAtTop(tick);
            }

            // Needle.
            c._needle = c._compositor.CreateSpriteVisual();
            c._needle.Size = new Vector2((float)c.NeedleWidth, (float)c.NeedleLength );
            c._needle.Brush = c._compositor.CreateColorBrush(c.NeedleBrush.Color);
            c._needle.CenterPoint = new Vector3((float)c.NeedleWidth / 2, (float)c.NeedleLength, 0);
            c._needle.Offset = new Vector3(100 - (float)c.NeedleWidth / 2, 100 - (float)c.NeedleLength, 0);
            c._root.Children.InsertAtTop(c._needle);

            OnValueChanged(c);
        }

        /// <summary>
        /// Transforms a set of polar coordinates into a Windows Point.
        /// </summary>
        private Point ScalePoint(double angle, double middleOfScale)
        {
            return new Point(100 + Math.Sin(Degrees2Radians * angle) * middleOfScale, 100 - Math.Cos(Degrees2Radians * angle) * middleOfScale);
        }

        /// <summary>
        /// Returns the angle for a specific value.
        /// </summary>
        /// <returns>In degrees.</returns>
        private double ValueToAngle(double value)
        {
            // Off-scale on the left.
            if (value < Minimum)
            {
                return MinAngle - 7.5;
            }

            // Off-scale on the right.
            if (value > Maximum)
            {
                return MaxAngle + 7.5;
            }

            return (value - Minimum) / (Maximum - Minimum) * (MaxAngle - MinAngle) + MinAngle;
        }
    }
}
