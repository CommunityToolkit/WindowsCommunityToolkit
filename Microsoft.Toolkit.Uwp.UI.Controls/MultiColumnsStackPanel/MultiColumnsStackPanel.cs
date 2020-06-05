// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// <see cref="MultiColumnsStackPanel"/> positions its child elements vertically in one or several columns based on the <see cref="MultiColumnsStackPanel.MaxColumnWidth"/> property.
    /// </summary>
    public class MultiColumnsStackPanel : Panel
    {
        /// <summary>
        /// The DP to store the MaxColumnWidth value.
        /// </summary>
        public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.Register(
            nameof(MaxColumnWidth),
            typeof(double),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        /// <summary>
        /// The DP to store the ColumnsSpacing value.
        /// </summary>
        public static readonly DependencyProperty ColumnsSpacingProperty = DependencyProperty.Register(
            nameof(ColumnsSpacing),
            typeof(double),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        /// <summary>
        /// The DP to store the ItemsSpacing value.
        /// </summary>
        public static readonly DependencyProperty ItemsSpacingProperty = DependencyProperty.Register(
            nameof(ItemsSpacing),
            typeof(double),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        /// <summary>
        /// The DP to store the HorizontalContentAlignment value.
        /// </summary>
        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register(
            nameof(HorizontalContentAlignment),
            typeof(HorizontalAlignment),
            typeof(MultiColumnsStackPanel),
            new PropertyMetadata(HorizontalAlignment.Stretch, OnLayoutPropertyChanged));

        /// <summary>
        /// Gets or sets the maximum width for the columns.
        /// If the value is 0, it will display a single column (like a vertical <see cref="StackPanel"/>).
        /// </summary>
        public double MaxColumnWidth
        {
            get => (double)GetValue(MaxColumnWidthProperty);
            set => SetValue(MaxColumnWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the spacing between two columns.
        /// </summary>
        public double ColumnsSpacing
        {
            get => (double)GetValue(ColumnsSpacingProperty);
            set => SetValue(ColumnsSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the spacing between two items.
        /// </summary>
        public double ItemsSpacing
        {
            get => (double)GetValue(ItemsSpacingProperty);
            set => SetValue(ItemsSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the control's content.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiColumnsStackPanel)d;

            control.InvalidateMeasure();
            control.InvalidateArrange();
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var child in Children)
            {
                child.Measure(availableSize);
            }

            return availableSize;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var child in Children)
            {
                child.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }

            return finalSize;
        }
    }
}
