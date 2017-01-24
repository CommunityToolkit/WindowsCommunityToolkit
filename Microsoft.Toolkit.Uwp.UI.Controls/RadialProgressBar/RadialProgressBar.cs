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
// *****************************************************************

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// An alternative impementation of a progress bar.
    /// Progression is represented by a loop filling up in a clockwise fashion.
    /// Like the traditional progress bar, it inherits from RangeBase, so Minimum, Maximum and Value properties work the same way.
    /// </summary>
    [TemplatePart(Name = OutlineFigurePartName, Type = typeof(PathFigure))]
    [TemplatePart(Name = OutlineArcPartName, Type = typeof(ArcSegment))]
    [TemplatePart(Name = BarFigurePartName, Type = typeof(PathFigure))]
    [TemplatePart(Name = BarArcPartName, Type = typeof(ArcSegment))]
    public sealed class RadialProgressBar : RangeBase
    {
        private const string OutlineFigurePartName = "OutlineFigurePart";
        private const string OutlineArcPartName = "OutlineArcPart";
        private const string BarFigurePartName = "BarFigurePart";
        private const string BarArcPartName = "BarArcPart";

        private const string DefaultForegroundColorBrushName = "SystemControlHighlightAccentBrush";
        private const string DefaultBackgroundColorBrushName = "SystemControlBackgroundBaseLowBrush";

        private PathFigure OutlineFigure { get; set; }

        private PathFigure BarFigure { get; set; }

        private ArcSegment OutlineArc { get; set; }

        private ArcSegment BarArc { get; set; }

        private bool AllTemplatePartsDefined
        {
            get { return OutlineFigure != null && OutlineArc != null && BarFigure != null && BarArc != null; }
        }

        /// <summary>
        /// Called when the Minimum property changes.
        /// </summary>
        /// <param name="oldMinimum">Old value of the Minimum property.</param>
        /// <param name="newMinimum">New value of the Minimum property.</param>
        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            RenderSegment();
        }

        /// <summary>
        /// Called when the Maximum property changes.
        /// </summary>
        /// <param name="oldMaximum">Old value of the Maximum property.</param>
        /// <param name="newMaximum">New value of the Maximum property.</param>
        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            RenderSegment();
        }

        /// <summary>
        /// Called when the Value property changes.
        /// </summary>
        /// <param name="oldValue">Old value of the Value property.</param>
        /// <param name="newValue">New value of the Value property.</param>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            RenderSegment();
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            OutlineFigure = GetTemplateChild(OutlineFigurePartName) as PathFigure;
            OutlineArc = GetTemplateChild(OutlineArcPartName) as ArcSegment;
            BarFigure = GetTemplateChild(BarFigurePartName) as PathFigure;
            BarArc = GetTemplateChild(BarArcPartName) as ArcSegment;

            RenderAll();
        }

        /// <summary>
        /// Gets or sets the thickness of the circular ouline and segment
        /// </summary>
        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        /// <summary>
        /// Identifies the Thickness dependency property
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(double), typeof(RadialProgressBar), new PropertyMetadata(4.0, ThicknessChangedHandler));

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialProgressBar"/> class.
        /// Create a default circular progress bar
        /// </summary>
        public RadialProgressBar()
        {
            this.DefaultStyleKey = typeof(RadialProgressBar);

            Foreground = Application.Current.Resources[DefaultForegroundColorBrushName] as SolidColorBrush;
            Background = Application.Current.Resources[DefaultBackgroundColorBrushName] as SolidColorBrush;

            SizeChanged += SizeChangedHandler;
        }

        // Render outline and progress segment when thickness is changed
        private static void ThicknessChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as RadialProgressBar;
            sender.RenderAll();
        }

        // Render outline and progress segment when control is resized.
        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            var self = sender as RadialProgressBar;
            self.RenderAll();
        }

        private double ComputeNormalizedRange()
        {
            var range = Maximum - Minimum;
            var delta = Value - Minimum;
            var output = range == 0.0 ? 0.0 : delta / range;
            output = Math.Min(Math.Max(0.0, output), 0.9999);
            return output;
        }

        // Compute size of ellipse so that the outer edge touches the bounding rectangle
        private Size ComputeEllipseSize()
        {
            var safeThickness = Math.Max(Thickness, 0.0);
            var width = Math.Max((ActualWidth - safeThickness) / 2.0, 0.0);
            var height = Math.Max((ActualHeight - safeThickness) / 2.0, 0.0);
            return new Size(width, height);
        }

        // Render the segment representing progress ratio.
        private void RenderSegment()
        {
            if (!AllTemplatePartsDefined)
            {
                return;
            }

            var normalizedRange = ComputeNormalizedRange();

            var angle = 2 * Math.PI * normalizedRange;
            var size = ComputeEllipseSize();
            var translationFactor = Math.Max(Thickness / 2.0, 0.0);

            double x = (Math.Sin(angle) * size.Width) + size.Width + translationFactor;
            double y = (((Math.Cos(angle) * size.Height) - size.Height) * -1) + translationFactor;

            BarArc.IsLargeArc = angle >= Math.PI;
            BarArc.Point = new Point(x, y);
        }

        // Render the progress segment and the loop outline. Needs to run when control is resized or retemplated
        private void RenderAll()
        {
            if (!AllTemplatePartsDefined)
            {
                return;
            }

            var size = ComputeEllipseSize();
            var segmentWidth = size.Width;
            var translationFactor = Math.Max(Thickness / 2.0, 0.0);

            OutlineFigure.StartPoint = BarFigure.StartPoint = new Point(segmentWidth + translationFactor, translationFactor);
            OutlineArc.Size = BarArc.Size = new Size(segmentWidth, size.Height);
            OutlineArc.Point = new Point(segmentWidth + translationFactor - 0.05, translationFactor);

            RenderSegment();
        }
    }
}
