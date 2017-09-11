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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.DeveloperTools
{
    /// <summary>
    /// AlignmentGrid is used to display a grid to help aligning controls
    /// </summary>
    public class AlignmentGrid : ContentControl
    {
        /// <summary>
        /// Identifies the <see cref="LineBrush"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register(nameof(LineBrush), typeof(Brush), typeof(AlignmentGrid), new PropertyMetadata(null, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="HorizontalStep"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalStepProperty = DependencyProperty.Register(nameof(HorizontalStep), typeof(double), typeof(AlignmentGrid), new PropertyMetadata(20.0, OnPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="VerticalStep"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalStepProperty = DependencyProperty.Register(nameof(VerticalStep), typeof(double), typeof(AlignmentGrid), new PropertyMetadata(20.0, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var alignmentGrid = dependencyObject as AlignmentGrid;

            alignmentGrid?.Rebuild();
        }

        private readonly Canvas containerCanvas = new Canvas();

        /// <summary>
        /// Gets or sets the step to use horizontally.
        /// </summary>
        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the step to use horizontally.
        /// </summary>
        public double HorizontalStep
        {
            get { return (double)GetValue(HorizontalStepProperty); }
            set { SetValue(HorizontalStepProperty, value); }
        }

        /// <summary>
        /// Gets or sets the step to use horizontally.
        /// </summary>
        public double VerticalStep
        {
            get { return (double)GetValue(VerticalStepProperty); }
            set { SetValue(VerticalStepProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignmentGrid"/> class.
        /// </summary>
        public AlignmentGrid()
        {
            SizeChanged += AlignmentGrid_SizeChanged;

            IsHitTestVisible = false;

            Opacity = 0.5;

            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
            Content = containerCanvas;
        }

        private void Rebuild()
        {
            containerCanvas.Children.Clear();
            var horizontalStep = HorizontalStep;
            var verticalStep = VerticalStep;
            var brush = LineBrush ?? (Brush)Application.Current.Resources["ApplicationForegroundThemeBrush"];

            for (double x = 0; x < ActualWidth; x += horizontalStep)
            {
                var line = new Rectangle
                {
                    Width = 1,
                    Height = ActualHeight,
                    Fill = brush
                };
                Canvas.SetLeft(line, x);

                containerCanvas.Children.Add(line);
            }

            for (double y = 0; y < ActualHeight; y += verticalStep)
            {
                var line = new Rectangle
                {
                    Width = ActualWidth,
                    Height = 1,
                    Fill = brush
                };
                Canvas.SetTop(line, y);

                containerCanvas.Children.Add(line);
            }
        }

        private void AlignmentGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rebuild();
        }
    }
}
