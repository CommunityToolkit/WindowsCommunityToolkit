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

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// The VariableSizedGrid control allows to display items from a list using different values
    /// for Width and Height item properties. You can control the number of rows and columns to be
    /// displayed as well as the items orientation in the panel. Finally, the AspectRatio property
    /// allow us to control the relation between Width and Height.
    /// </summary>
    public partial class VariableSizedGridView
    {
        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(VariableSizedGridView), new PropertyMetadata(new Thickness(2)));

        /// <summary>
        /// Identifies the <see cref="ItemPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(nameof(ItemPadding), typeof(Thickness), typeof(VariableSizedGridView), new PropertyMetadata(new Thickness(2)));

        /// <summary>
        /// Identifies the <see cref="MaximumRowsOrColumns"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumRowsOrColumnsProperty = DependencyProperty.Register(nameof(MaximumRowsOrColumns), typeof(int), typeof(VariableSizedGridView), new PropertyMetadata(4, MaximumRowsOrColumnsChanged));

        /// <summary>
        /// Identifies the <see cref="AspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register(nameof(AspectRatio), typeof(double), typeof(VariableSizedGridView), new PropertyMetadata(1.0, AspectRatioChanged));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(VariableSizedGridView), new PropertyMetadata(Orientation.Horizontal, OrientationChanged));

        /// <summary>
        /// Gets or sets the margin for each item.
        /// </summary>
        /// <value>The item margin.</value>
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets the padding applied to each item.
        /// </summary>
        /// <value>The item padding.</value>
        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of rows or columns.
        /// </summary>
        /// <value>The maximum rows or columns.</value>
        public int MaximumRowsOrColumns
        {
            get { return (int)GetValue(MaximumRowsOrColumnsProperty); }
            set { SetValue(MaximumRowsOrColumnsProperty, value); }
        }

        private static void MaximumRowsOrColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridView;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Gets or sets the height-to-width aspect ratio for each tile.
        /// </summary>
        /// <value>The aspect ratio.</value>
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        private static void AspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridView;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Gets or sets the dimension by which child elements are stacked.
        /// </summary>
        /// <value>One of the enumeration values that specifies the orientation of child elements.</value>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGridView;
            control.SetOrientation((Orientation)e.NewValue);
        }
    }
}
