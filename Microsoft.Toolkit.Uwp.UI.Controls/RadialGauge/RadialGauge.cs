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

using System;
using System.Numerics;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A Modern UI Radial Gauge using XAML and Composition API.
    /// The scale of the gauge is a clockwise arc that sweeps from MinAngle (default lower left, at -150°) to MaxAngle (default lower right, at +150°).
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
        /// Identifies the optional StepSize property.
        /// </summary>
        public static readonly DependencyProperty StepSizeProperty =
            DependencyProperty.Register(nameof(StepSize), typeof(double), typeof(RadialGauge), new PropertyMetadata(0.0));

        // Identifies the IsInteractive dependency property.
        public static readonly DependencyProperty IsInteractiveProperty =
            DependencyProperty.Register(nameof(IsInteractive), typeof(bool), typeof(RadialGauge), new PropertyMetadata(false, OnInteractivityChanged));

        /// <summary>
        /// Identifies the ScaleWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ScaleWidthProperty =
            DependencyProperty.Register(nameof(ScaleWidth), typeof(double), typeof(RadialGauge), new PropertyMetadata(26.0, OnScaleChanged));

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
        /// Identifies the MinAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty MinAngleProperty =
            DependencyProperty.Register(nameof(MinAngle), typeof(int), typeof(RadialGauge), new PropertyMetadata(-150, OnScaleChanged));

        /// <summary>
        /// Identifies the MaxAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxAngleProperty =
            DependencyProperty.Register(nameof(MaxAngle), typeof(int), typeof(RadialGauge), new PropertyMetadata(150, OnScaleChanged));

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

        private Compositor _compositor;
        private ContainerVisual _root;
        private SpriteVisual _needle;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGauge"/> class.
        /// Create a default radial gauge control.
        /// </summary>
        public RadialGauge()
        {
            DefaultStyleKey = typeof(RadialGauge);
        }

        /// <summary>
        /// Gets or sets the minimum value of the scale.
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value of the scale.
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the rounding interval for the Value.
        /// </summary>
        public double StepSize
        {
            get { return (double)GetValue(StepSizeProperty); }
            set { SetValue(StepSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control accepts setting its value through interaction.
        /// </summary>
        public bool IsInteractive
        {
            get { return (bool)GetValue(IsInteractiveProperty); }
            set { SetValue(IsInteractiveProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the scale, in percentage of the gauge radius.
        /// </summary>
        public double ScaleWidth
        {
            get { return (double)GetValue(ScaleWidthProperty); }
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
        /// Gets or sets the displayed unit measure.
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
        /// Gets or sets the brush for the displayed value.
        /// </summary>
        public Brush ValueBrush
        {
            get { return (Brush)GetValue(ValueBrushProperty); }
            set { SetValue(ValueBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush for the displayed unit measure.
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
        public double NeedleWidth
        {
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
        /// Gets or sets the start angle of the scale, which corresponds with the Minimum value, in degrees. It's typically on the left hand side of the control. The proposed value range is from -180 (bottom) to 0° (top).
        /// </summary>
        /// <remarks>Changing MinAngle may require retemplating the control.</remarks>
        public int MinAngle
        {
            get { return (int)GetValue(MinAngleProperty); }
            set { SetValue(MinAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the end angle of the scale, which corresponds with the Maximum value, in degrees. It 's typically on the right hand side of the control. The proposed value range is from 0° (top) to 180° (bottom).
        /// </summary>
        /// <remarks>Changing MaxAngle may require retemplating the control.</remarks>
        public int MaxAngle
        {
            get { return (int)GetValue(MaxAngleProperty); }
            set { SetValue(MaxAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the current angle of the needle (between MinAngle and MaxAngle). Setting the angle will update the Value.
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

        private static void OnValueChanged(DependencyObject d)
        {
            RadialGauge radialGauge = (RadialGauge)d;
            if (!double.IsNaN(radialGauge.Value))
            {
                if (radialGauge.StepSize != 0)
                {
                    radialGauge.Value = radialGauge.RoundToMultiple(radialGauge.Value, radialGauge.StepSize);
                }

                var middleOfScale = 100 - radialGauge.ScalePadding - (radialGauge.ScaleWidth / 2);
                var valueText = radialGauge.GetTemplateChild(ValueTextPartName) as TextBlock;
                radialGauge.ValueAngle = radialGauge.ValueToAngle(radialGauge.Value);

                // Needle
                if (radialGauge._needle != null)
                {
                    radialGauge._needle.RotationAngleInDegrees = (float)radialGauge.ValueAngle;
                }

                // Trail
                var trail = radialGauge.GetTemplateChild(TrailPartName) as Path;
                if (trail != null)
                {
                    if (radialGauge.ValueAngle > radialGauge.MinAngle)
                    {
                        trail.Visibility = Visibility.Visible;

                        if (radialGauge.ValueAngle - radialGauge.MinAngle == 360)
                        {
                            // Draw full circle.
                            var eg = new EllipseGeometry();
                            eg.Center = new Point(100, 100);
                            eg.RadiusX = 100 - radialGauge.ScalePadding - (radialGauge.ScaleWidth / 2);
                            eg.RadiusY = eg.RadiusX;
                            trail.Data = eg;
                        }
                        else
                        {
                            // Draw arc.
                            var pg = new PathGeometry();
                            var pf = new PathFigure();
                            pf.IsClosed = false;
                            pf.StartPoint = radialGauge.ScalePoint(radialGauge.MinAngle, middleOfScale);
                            var seg = new ArcSegment();
                            seg.SweepDirection = SweepDirection.Clockwise;
                            seg.IsLargeArc = radialGauge.ValueAngle > (180 + radialGauge.MinAngle);
                            seg.Size = new Size(middleOfScale, middleOfScale);
                            seg.Point = radialGauge.ScalePoint(Math.Min(radialGauge.ValueAngle, radialGauge.MaxAngle), middleOfScale);  // On overflow, stop trail at MaxAngle.
                            pf.Segments.Add(seg);
                            pg.Figures.Add(pf);
                            trail.Data = pg;
                        }
                    }
                    else
                    {
                        trail.Visibility = Visibility.Collapsed;
                    }
                }

                // Value Text
                if (valueText != null)
                {
                    valueText.Text = radialGauge.Value.ToString(radialGauge.ValueStringFormat);
                }
            }
        }

        private static void OnInteractivityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RadialGauge radialGauge = (RadialGauge)d;

            if (radialGauge.IsInteractive)
            {
                radialGauge.Tapped += radialGauge.RadialGauge_Tapped;
                radialGauge.ManipulationDelta += radialGauge.RadialGauge_ManipulationDelta;
                radialGauge.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            }
            else
            {
                radialGauge.Tapped -= radialGauge.RadialGauge_Tapped;
                radialGauge.ManipulationDelta -= radialGauge.RadialGauge_ManipulationDelta;
                radialGauge.ManipulationMode = ManipulationModes.None;
            }
        }

        private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnScaleChanged(d);
        }

        private static void OnScaleChanged(DependencyObject d)
        {
            RadialGauge radialGauge = (RadialGauge)d;

            var scale = radialGauge.GetTemplateChild(ScalePartName) as Path;
            if (scale != null)
            {
                if (radialGauge.MaxAngle - radialGauge.MinAngle == 360)
                {
                    // Draw full circle.
                    var eg = new EllipseGeometry();
                    eg.Center = new Point(100, 100);
                    eg.RadiusX = 100 - radialGauge.ScalePadding - (radialGauge.ScaleWidth / 2);
                    eg.RadiusY = eg.RadiusX;
                    scale.Data = eg;
                }
                else
                {
                    // Draw arc.
                    var pg = new PathGeometry();
                    var pf = new PathFigure();
                    pf.IsClosed = false;
                    var middleOfScale = 100 - radialGauge.ScalePadding - (radialGauge.ScaleWidth / 2);
                    pf.StartPoint = radialGauge.ScalePoint(radialGauge.MinAngle, middleOfScale);
                    var seg = new ArcSegment();
                    seg.SweepDirection = SweepDirection.Clockwise;
                    seg.IsLargeArc = radialGauge.MaxAngle > (radialGauge.MinAngle + 180);
                    seg.Size = new Size(middleOfScale, middleOfScale);
                    seg.Point = radialGauge.ScalePoint(radialGauge.MaxAngle, middleOfScale);
                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);
                    scale.Data = pg;
                }

                OnFaceChanged(radialGauge);
            }
        }

        private static void OnFaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnFaceChanged(d);
        }

        private static void OnFaceChanged(DependencyObject d)
        {
            RadialGauge radialGauge = (RadialGauge)d;

            var container = radialGauge.GetTemplateChild(ContainerPartName) as Grid;
            if (container == null || DesignMode.DesignModeEnabled)
            {
                // Bad template.
                return;
            }

            radialGauge._root = container.GetVisual();
            radialGauge._root.Children.RemoveAll();
            radialGauge._compositor = radialGauge._root.Compositor;

            // Ticks.
            SpriteVisual tick;
            for (double i = radialGauge.Minimum; i <= radialGauge.Maximum; i += radialGauge.TickSpacing)
            {
                tick = radialGauge._compositor.CreateSpriteVisual();
                tick.Size = new Vector2((float)radialGauge.TickWidth, (float)radialGauge.TickLength);
                tick.Brush = radialGauge._compositor.CreateColorBrush(radialGauge.TickBrush.Color);
                tick.Offset = new Vector3(100 - ((float)radialGauge.TickWidth / 2), 0.0f, 0);
                tick.CenterPoint = new Vector3((float)radialGauge.TickWidth / 2, 100.0f, 0);
                tick.RotationAngleInDegrees = (float)radialGauge.ValueToAngle(i);
                radialGauge._root.Children.InsertAtTop(tick);
            }

            // Scale Ticks.
            for (double i = radialGauge.Minimum; i <= radialGauge.Maximum; i += radialGauge.TickSpacing)
            {
                tick = radialGauge._compositor.CreateSpriteVisual();
                tick.Size = new Vector2((float)radialGauge.ScaleTickWidth, (float)radialGauge.ScaleWidth);
                tick.Brush = radialGauge._compositor.CreateColorBrush(radialGauge.ScaleTickBrush.Color);
                tick.Offset = new Vector3(100 - ((float)radialGauge.ScaleTickWidth / 2), (float)radialGauge.ScalePadding, 0);
                tick.CenterPoint = new Vector3((float)radialGauge.ScaleTickWidth / 2, 100 - (float)radialGauge.ScalePadding, 0);
                tick.RotationAngleInDegrees = (float)radialGauge.ValueToAngle(i);
                radialGauge._root.Children.InsertAtTop(tick);
            }

            // Needle.
            radialGauge._needle = radialGauge._compositor.CreateSpriteVisual();
            radialGauge._needle.Size = new Vector2((float)radialGauge.NeedleWidth, (float)radialGauge.NeedleLength);
            radialGauge._needle.Brush = radialGauge._compositor.CreateColorBrush(radialGauge.NeedleBrush.Color);
            radialGauge._needle.CenterPoint = new Vector3((float)radialGauge.NeedleWidth / 2, (float)radialGauge.NeedleLength, 0);
            radialGauge._needle.Offset = new Vector3(100 - ((float)radialGauge.NeedleWidth / 2), 100 - (float)radialGauge.NeedleLength, 0);
            radialGauge._root.Children.InsertAtTop(radialGauge._needle);

            OnValueChanged(radialGauge);
        }

        private void RadialGauge_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            SetGaugeValueFromPoint(e.Position);
        }

        private void RadialGauge_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SetGaugeValueFromPoint(e.GetPosition(this));
        }

        private void SetGaugeValueFromPoint(Point p)
        {
            var pt = new Point(p.X - (ActualWidth / 2), -p.Y + (ActualHeight / 2));

            var angle = Math.Atan2(pt.X, pt.Y) * 180 / Math.PI;
            var value = Minimum + ((Maximum - Minimum) * (angle - MinAngle) / (MaxAngle - MinAngle));
            if (value < Minimum || value > Maximum)
            {
                // Ignore positions outside the scale angle.
                return;
            }

            Value = value;
        }

        private Point ScalePoint(double angle, double middleOfScale)
        {
            return new Point(100 + (Math.Sin(Degrees2Radians * angle) * middleOfScale), 100 - (Math.Cos(Degrees2Radians * angle) * middleOfScale));
        }

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

            return ((value - Minimum) / (Maximum - Minimum) * (MaxAngle - MinAngle)) + MinAngle;
        }

        private double RoundToMultiple(double number, double multiple)
        {
            double modulo = number % multiple;
            if ((multiple - modulo) <= modulo)
            {
                modulo = multiple - modulo;
            }
            else
            {
                modulo *= -1;
            }

            return number + modulo;
        }
    }
}
