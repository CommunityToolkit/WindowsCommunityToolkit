// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Arranges child elements into a staggered grid pattern where items are added to the column that has used least amount of space.
    /// </summary>
    public class StaggeredLayout : VirtualizingLayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaggeredLayout"/> class.
        /// </summary>
        public StaggeredLayout()
        {
        }

        /// <summary>
        /// Gets or sets the desired width for each column.
        /// </summary>
        /// <remarks>
        /// The width of columns can exceed the DesiredColumnWidth if the HorizontalAlignment is set to Stretch.
        /// </remarks>
        public double DesiredColumnWidth
        {
            get { return (double)GetValue(DesiredColumnWidthProperty); }
            set { SetValue(DesiredColumnWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DesiredColumnWidth"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="DesiredColumnWidth"/> dependency property.</returns>
        public static readonly DependencyProperty DesiredColumnWidthProperty = DependencyProperty.Register(
            nameof(DesiredColumnWidth),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(250d, OnDesiredColumnWidthChanged));

        /// <summary>
        /// Gets or sets the spacing between columns of items.
        /// </summary>
        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
            nameof(ColumnSpacing),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(0d, OnSpacingChanged));

        /// <summary>
        /// Gets or sets the spacing between rows of items.
        /// </summary>
        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RowSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
            nameof(RowSpacing),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(0d, OnSpacingChanged));

        /// <inheritdoc/>
        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            context.LayoutState = new StaggeredLayoutState(context);
            base.InitializeForContextCore(context);
        }

        /// <inheritdoc/>
        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            context.LayoutState = null;
            base.UninitializeForContextCore(context);
        }

        /// <inheritdoc/>
        protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
        {
            var state = (StaggeredLayoutState)context.LayoutState;

            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                state.RemoveFromIndex(args.OldStartingIndex);
            }
            else if (args.Action == NotifyCollectionChangedAction.Add)
            {
                state.RemoveFromIndex(args.NewStartingIndex);
            }
            else if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                state.Clear();
            }

            base.OnItemsChangedCore(context, source, args);
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            if (context.ItemCount == 0)
            {
                return new Size(availableSize.Width, 0);
            }

            if ((context.RealizationRect.Width == 0) && (context.RealizationRect.Height == 0))
            {
                return new Size(availableSize.Width, 0.0);
            }

            var state = (StaggeredLayoutState)context.LayoutState;

            double availableWidth = availableSize.Width;
            double availableHeight = availableSize.Height;

            double columnWidth = Math.Min(DesiredColumnWidth, availableWidth);
            if (columnWidth != state.ColumnWidth)
            {
                // The items will need to be remeasured
                state.Clear();
            }

            state.ColumnWidth = Math.Min(DesiredColumnWidth, availableWidth);
            int numColumns = Math.Max(1, (int)Math.Floor(availableWidth / state.ColumnWidth));

            // adjust for column spacing on all columns expect the first
            double totalWidth = state.ColumnWidth + ((numColumns - 1) * (state.ColumnWidth + ColumnSpacing));
            if (totalWidth > availableWidth)
            {
                numColumns--;
            }
            else if (double.IsInfinity(availableWidth))
            {
                availableWidth = totalWidth;
            }

            if (numColumns != state.NumberOfColumns)
            {
                // The items will not need to be remeasured, but they will need to go into new columns
                state.ClearColumns();
            }

            var columnHeights = new double[numColumns];
            var itemsPerColumn = new int[numColumns];
            var deadColumns = new HashSet<int>();

            for (int i = 0; i < context.ItemCount; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                UIElement element = null;
                StaggeredItem item = state.GetItemAt(i);
                if (item.Height == 0)
                {
                    element = context.GetOrCreateElementAt(i);
                    element.Measure(new Size(state.ColumnWidth, availableHeight));
                    item.Height = element.DesiredSize.Height;
                }

                int numberOfItems = itemsPerColumn[columnIndex];
                double spacing = numberOfItems > 0 ? (RowSpacing * numberOfItems) : 0;
                item.Top = columnHeights[columnIndex] + spacing;
                double bottom = item.Top + item.Height;

                // ignore row spacing. columnHeights is just used to determine which column to place the item.
                // row spacing shouldn't change the column the item is within.
                columnHeights[columnIndex] += item.Height;
                itemsPerColumn[columnIndex]++;
                state.AddItemToColumn(item, columnIndex);

                if (bottom < context.RealizationRect.Top)
                {
                    // The bottom of the element is above the realization area
                    if (element != null)
                    {
                        context.RecycleElement(element);
                    }
                }
                else if (item.Top > context.RealizationRect.Bottom)
                {
                    // The top of the element is below the realization area
                    // item.RecycleElement();
                    deadColumns.Add(columnIndex);
                }
                else
                {
                    // We ALWAYS want to measure an item that will be in the bounds
                    context.GetOrCreateElementAt(i).Measure(new Size(state.ColumnWidth, availableHeight));
                }

                if (deadColumns.Count == numColumns)
                {
                    break;
                }
            }

            double desiredHeight = state.GetHeight();

            return new Size(availableWidth, desiredHeight);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            if ((context.RealizationRect.Width == 0) && (context.RealizationRect.Height == 0))
            {
                return finalSize;
            }

            var state = (StaggeredLayoutState)context.LayoutState;

            // Cycle through each column and arrange the items that are within the realization bounds
            for (int columnIndex = 0; columnIndex < state.NumberOfColumns; columnIndex++)
            {
                StaggeredColumnLayout layout = state.GetColumnLayout(columnIndex);
                for (int i = 0; i < layout.Count; i++)
                {
                    StaggeredItem item = layout[i];

                    double bottom = item.Top + item.Height;
                    if (bottom < context.RealizationRect.Top)
                    {
                        // element is above the realization bounds
                        continue;
                    }

                    if (item.Top <= context.RealizationRect.Bottom)
                    {
                        double itemHorizontalOffset = (state.ColumnWidth * columnIndex) + (ColumnSpacing * columnIndex);

                        Rect bounds = new Rect(itemHorizontalOffset, item.Top, state.ColumnWidth, item.Height);
                        UIElement element = context.GetOrCreateElementAt(item.Index);
                        element.Arrange(bounds);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return finalSize;
        }

        private static void OnDesiredColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (StaggeredLayout)d;
            panel.InvalidateMeasure();
        }

        private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (StaggeredLayout)d;
            panel.InvalidateMeasure();
        }

        private int GetColumnIndex(double[] columnHeights)
        {
            int columnIndex = 0;
            double height = columnHeights[0];
            for (int j = 1; j < columnHeights.Length; j++)
            {
                if (columnHeights[j] < height)
                {
                    columnIndex = j;
                    height = columnHeights[j];
                }
            }

            return columnIndex;
        }
    }
}
