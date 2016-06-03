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
    /// <remarks>All calculations are for a 200x200 square. The viewbox will do the rest.</remarks>
    [TemplatePart(Name = ContainerPartName, Type = typeof(Grid))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = TrailPartName, Type = typeof(Path))]
    [TemplatePart(Name = ValueTextPartName, Type = typeof(TextBlock))]
    public class RadialGauge : Control
    {
        // Template Parts.
        private const string ContainerPartName = "PART_Container";
        private const string ScalePartName = "PART_Scale";
        private const string TrailPartName = "PART_Trail";
        private const string ValueTextPartName = "PART_ValueText";

        // For convenience.
        private const double Degrees2Radians = Math.PI / 180;

        // Candidate dependency properties.
        // Feel free to modify...
        private const double MinAngle = -150.0;
        private const double MaxAngle = 150.0;
        private const float TickHeight = 18.0f;
        private const float TickWidth = 5.0f;
        private const float ScalePadding = 23.0f;
        private const float ScaleTickWidth = 2.5f;
        private const float NeedleWidth = 5.0f;
        private const float NeedleHeight = 100.0f;

        private Compositor _compositor;
        private ContainerVisual _root;
        private SpriteVisual _needle;

        public RadialGauge()
        {
            this.DefaultStyleKey = typeof(RadialGauge);
        }

        /// <summary>
        /// Identifies the Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(RadialGauge), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the minimum on the scale.
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Identifies the Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(RadialGauge), new PropertyMetadata(100.0));

        /// <summary>
        /// Gets or sets the maximum on the scale.
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Identifies the ScaleWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleWidthProperty =
            DependencyProperty.Register("ScaleWidth", typeof(Double), typeof(RadialGauge), new PropertyMetadata(26.0));

        /// <summary>
        /// Gets or sets the width of the scale.
        /// </summary>
        public Double ScaleWidth
        {
            get { return (Double)GetValue(ScaleWidthProperty); }
            set { SetValue(ScaleWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(RadialGauge), new PropertyMetadata(0.0, OnValueChanged));

        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the Unit dependency property.
        /// </summary>
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(string), typeof(RadialGauge), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the unit measure.
        /// </summary>
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        /// <summary>
        /// Identifies the NeedleBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty NeedleBrushProperty =
            DependencyProperty.Register("NeedleBrush", typeof(SolidColorBrush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        /// <summary>
        /// Gets or sets the needle brush.
        /// </summary>
        public SolidColorBrush NeedleBrush
        {
            get { return (SolidColorBrush)GetValue(NeedleBrushProperty); }
            set { SetValue(NeedleBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the TrailBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TrailBrushProperty =
            DependencyProperty.Register("TrailBrush", typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        /// <summary>
        /// Gets or sets the trail brush.
        /// </summary>
        public Brush TrailBrush
        {
            get { return (Brush)GetValue(TrailBrushProperty); }
            set { SetValue(TrailBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the ScaleBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register("ScaleBrush", typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        /// <summary>
        /// Gets or sets the scale brush.
        /// </summary>
        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the ScaleTickBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleTickBrushProperty =
            DependencyProperty.Register("ScaleTickBrush", typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        /// <summary>
        /// Gets or sets the scale tick brush.
        /// </summary>
        public SolidColorBrush ScaleTickBrush
        {
            get { return (SolidColorBrush)GetValue(ScaleTickBrushProperty); }
            set { SetValue(ScaleTickBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the TickBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty TickBrushProperty =
            DependencyProperty.Register("TickBrush", typeof(SolidColorBrush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Gets or sets the outer tick brush.
        /// </summary>
        public SolidColorBrush TickBrush
        {
            get { return (SolidColorBrush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the ValuaBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueBrushProperty =
            DependencyProperty.Register("ValueBrush", typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Gets or sets the value brush.
        /// </summary>
        public Brush ValueBrush
        {
            get { return (Brush)GetValue(ValueBrushProperty); }
            set { SetValue(ValueBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the UnitBrush dependency property.
        /// </summary>
        public static readonly DependencyProperty UnitBrushProperty =
            DependencyProperty.Register("UnitBrush", typeof(Brush), typeof(RadialGauge), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// Gets or sets the unit brush.
        /// </summary>
        public Brush UnitBrush
        {
            get { return (Brush)GetValue(UnitBrushProperty); }
            set { SetValue(UnitBrushProperty, value); }
        }

        /// <summary>
        /// Identifies the ValueStringFormat dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueStringFormatProperty =
            DependencyProperty.Register("ValueStringFormat", typeof(string), typeof(RadialGauge), new PropertyMetadata("N0"));

        /// <summary>
        /// Gets or sets the value string format.
        /// </summary>
        public string ValueStringFormat
        {
            get { return (string)GetValue(ValueStringFormatProperty); }
            set { SetValue(ValueStringFormatProperty, value); }
        }

        /// <summary>
        /// Identifies the TickSpacing dependency property.
        /// </summary>
        public static readonly DependencyProperty TickSpacingProperty =
        DependencyProperty.Register("TickSpacing", typeof(int), typeof(RadialGauge), new PropertyMetadata(10));

        /// <summary>
        /// Gets or sets the tick spacing, in units.
        /// </summary>
        public int TickSpacing
        {
            get { return (int)GetValue(TickSpacingProperty); }
            set { SetValue(TickSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the ValuaAngle dependency property.
        /// </summary>
        protected static readonly DependencyProperty ValueAngleProperty =
            DependencyProperty.Register("ValueAngle", typeof(double), typeof(RadialGauge), new PropertyMetadata(null));

        protected double ValueAngle
        {
            get { return (double)GetValue(ValueAngleProperty); }
            set { SetValue(ValueAngleProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            // Scale.
            var scale = this.GetTemplateChild(ScalePartName) as Path;
            if (scale != null)
            {
                var pg = new PathGeometry();
                var pf = new PathFigure();
                pf.IsClosed = false;
                var middleOfScale = 100 - ScalePadding - this.ScaleWidth / 2;
                pf.StartPoint = this.ScalePoint(MinAngle, middleOfScale);
                var seg = new ArcSegment();
                seg.SweepDirection = SweepDirection.Clockwise;
                seg.IsLargeArc = true;
                seg.Size = new Size(middleOfScale, middleOfScale);
                seg.Point = this.ScalePoint(MaxAngle, middleOfScale);
                pf.Segments.Add(seg);
                pg.Figures.Add(pf);
                scale.Data = pg;
            }

            var container = this.GetTemplateChild(ContainerPartName) as Grid;
            _root = container.GetVisual();
            _compositor = _root.Compositor;

            // Ticks.
            SpriteVisual tick;
            for (double i = Minimum; i <= Maximum; i += TickSpacing)
            {
                tick = _compositor.CreateSpriteVisual();
                tick.Size = new Vector2(TickWidth, TickHeight);
                tick.Brush = _compositor.CreateColorBrush(TickBrush.Color);
                tick.Offset = new Vector3(100 - TickWidth / 2, 0.0f, 0);
                tick.CenterPoint = new Vector3(TickWidth / 2, 100.0f, 0);
                tick.RotationAngleInDegrees = (float)ValueToAngle(i);
                _root.Children.InsertAtTop(tick);
            }

            // Scale Ticks.
            for (double i = Minimum; i <= Maximum; i += TickSpacing)
            {
                tick = _compositor.CreateSpriteVisual();
                tick.Size = new Vector2(ScaleTickWidth, (float)ScaleWidth);
                tick.Brush = _compositor.CreateColorBrush(ScaleTickBrush.Color);
                tick.Offset = new Vector3(100 - ScaleTickWidth / 2, ScalePadding, 0);
                tick.CenterPoint = new Vector3(ScaleTickWidth / 2, 100 - ScalePadding, 0);
                tick.RotationAngleInDegrees = (float)ValueToAngle(i);
                _root.Children.InsertAtTop(tick);
            }

            // Needle.
            _needle = _compositor.CreateSpriteVisual();
            _needle.Size = new Vector2(NeedleWidth, NeedleHeight);
            _needle.Brush = _compositor.CreateColorBrush(NeedleBrush.Color);
            _needle.CenterPoint = new Vector3(NeedleWidth / 2, NeedleHeight, 0);
            _needle.Offset = new Vector3(100 - NeedleWidth / 2, 100 - NeedleHeight, 0);
            _root.Children.InsertAtTop(_needle);

            OnValueChanged(this);
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
            if (!Double.IsNaN(c.Value))
            {
                var middleOfScale = 100 - ScalePadding - c.ScaleWidth / 2;
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
                        // We start from -150, so +30 becomes a large arc.
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
            if (value < this.Minimum)
            {
                return MinAngle - 7.5;
            }

            // Off-scale on the right.
            if (value > this.Maximum)
            {
                return MaxAngle + 7.5;
            }

            return (value - this.Minimum) / (this.Maximum - this.Minimum) * (MaxAngle - MinAngle) + MinAngle;
        }
    }
}
