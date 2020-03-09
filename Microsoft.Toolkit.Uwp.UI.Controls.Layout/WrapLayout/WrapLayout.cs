// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class WrapLayout : VirtualizingLayout
    {
        /// <summary>
        /// Gets or sets a uniform Horizontal distance (in pixels) between items when <see cref="Orientation"/> is set to Horizontal,
        /// or between columns of items when <see cref="Orientation"/> is set to Vertical.
        /// </summary>
        public double HorizontalSpacing
        {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set { SetValue(HorizontalSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="HorizontalSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalSpacingProperty =
            DependencyProperty.Register(
                nameof(HorizontalSpacing),
                typeof(double),
                typeof(WrapLayout),
                new PropertyMetadata(0d, LayoutPropertyChanged));

        /// <summary>
        /// Gets or sets a uniform Vertical distance (in pixels) between items when <see cref="Orientation"/> is set to Vertical,
        /// or between rows of items when <see cref="Orientation"/> is set to Horizontal.
        /// </summary>
        public double VerticalSpacing
        {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set { SetValue(VerticalSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="VerticalSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalSpacingProperty =
            DependencyProperty.Register(
                nameof(VerticalSpacing),
                typeof(double),
                typeof(WrapLayout),
                new PropertyMetadata(0d, LayoutPropertyChanged));

        /// <summary>
        /// Gets or sets the orientation of the WrapLayout.
        /// Horizontal means that child controls will be added horizontally until the width of the panel is reached, then a new row is added to add new child controls.
        /// Vertical means that children will be added vertically until the height of the panel is reached, then a new column is added.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(WrapLayout),
                new PropertyMetadata(Orientation.Horizontal, LayoutPropertyChanged));

        private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WrapLayout wp)
            {
                wp.InvalidateMeasure();
                wp.InvalidateArrange();
            }
        }

        /// <inheritdoc />
        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            var state = new WrapLayoutState(context);
            context.LayoutState = state;
            base.InitializeForContextCore(context);
        }

        /// <inheritdoc />
        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            context.LayoutState = null;
            base.UninitializeForContextCore(context);
        }

        /// <inheritdoc />
        protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
        {
            var state = (WrapLayoutState)context.LayoutState;

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    state.RemoveFromIndex(args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    int minIndex = Math.Min(args.NewStartingIndex, args.OldStartingIndex);
                    state.RemoveFromIndex(minIndex);

                    state.RecycleElementAt(args.OldStartingIndex);
                    state.RecycleElementAt(args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    state.RemoveFromIndex(args.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    state.RemoveFromIndex(args.NewStartingIndex);
                    state.RecycleElementAt(args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    state.Clear();
                    break;
            }

            base.OnItemsChangedCore(context, source, args);
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            var totalMeasure = UvMeasure.Zero;
            var parentMeasure = new UvMeasure(Orientation, availableSize.Width, availableSize.Height);
            var spacingMeasure = new UvMeasure(Orientation, HorizontalSpacing, VerticalSpacing);
            var realizationBounds = new UvBounds(Orientation, context.RealizationRect);
            var position = UvMeasure.Zero;

            var state = (WrapLayoutState)context.LayoutState;
            if (state.Orientation != Orientation)
            {
                state.SetOrientation(Orientation);
            }

            if (spacingMeasure.Equals(state.Spacing) == false)
            {
                state.ClearPositions();
                state.Spacing = spacingMeasure;
            }

            if (state.AvailableU != parentMeasure.U)
            {
                state.ClearPositions();
                state.AvailableU = parentMeasure.U;
            }

            double currentV = 0;
            for (int i = 0; i < context.ItemCount; i++)
            {
                UIElement child = null;
                bool measured = false;
                WrapItem item = state.GetItemAt(i);
                if (item.Measure == null)
                {
                    child = context.GetOrCreateElementAt(i);
                    child.Measure(availableSize);
                    item.Measure = new UvMeasure(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);
                    measured = true;
                }

                UvMeasure currentMeasure = item.Measure.Value;
                if (currentMeasure.U == 0)
                {
                    continue; // ignore collapsed items
                }

                if (item.Position == null)
                {
                    if (parentMeasure.U < position.U + currentMeasure.U)
                    {
                        // New Row
                        position.U = 0;
                        position.V += currentV + spacingMeasure.V;
                        currentV = 0;
                    }

                    item.Position = position;
                }

                position = item.Position.Value;

                double vEnd = position.V + currentMeasure.V;
                if (vEnd < realizationBounds.VMin)
                {
                    // Item is "above" the bounds
                    if (child != null)
                    {
                        context.RecycleElement(child);
                    }
                }
                else if (position.V > realizationBounds.VMax)
                {
                    // Item is "below" the bounds.
                    break;
                }
                else if (measured == false)
                {
                    // Always measure elements that are within the bounds
                    child = context.GetOrCreateElementAt(i);
                    child.Measure(availableSize);
                }

                position.U += currentMeasure.U + spacingMeasure.U;
                currentV = Math.Max(currentMeasure.V, currentV);
            }

            // update value with the last line
            // if the the last loop is(parentMeasure.U > currentMeasure.U + lineMeasure.U) the total isn't calculated then calculate it
            // if the last loop is (parentMeasure.U > currentMeasure.U) the currentMeasure isn't added to the total so add it here
            // for the last condition it is zeros so adding it will make no difference
            // this way is faster than an if condition in every loop for checking the last item
            totalMeasure.U = parentMeasure.U;
            totalMeasure.V = state.GetHeight();

            totalMeasure.U = Math.Ceiling(totalMeasure.U);

            return Orientation == Orientation.Horizontal ? new Size(totalMeasure.U, totalMeasure.V) : new Size(totalMeasure.V, totalMeasure.U);
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            if (context.ItemCount > 0)
            {
                var parentMeasure = new UvMeasure(Orientation, finalSize.Width, finalSize.Height);
                var spacingMeasure = new UvMeasure(Orientation, HorizontalSpacing, VerticalSpacing);
                var realizationBounds = new UvBounds(Orientation, context.RealizationRect);

                var state = (WrapLayoutState)context.LayoutState;
                bool arrange(WrapItem item, bool isLast = false)
                {
                    if (item.Measure.HasValue == false)
                    {
                        return false;
                    }

                    if (item.Position == null)
                    {
                        return false;
                    }

                    var desiredMeasure = item.Measure.Value;
                    if (desiredMeasure.U == 0)
                    {
                        return true; // if an item is collapsed, avoid adding the spacing
                    }

                    UvMeasure position = item.Position.Value;

                    // Stretch the last item to fill the available space
                    if (isLast)
                    {
                        desiredMeasure.U = parentMeasure.U - position.U;
                    }

                    if (((position.V + desiredMeasure.V) >= realizationBounds.VMin) && (position.V <= realizationBounds.VMax))
                    {
                        // place the item
                        UIElement child = context.GetOrCreateElementAt(item.Index);
                        if (Orientation == Orientation.Horizontal)
                        {
                            child.Arrange(new Rect(position.U, position.V, desiredMeasure.U, desiredMeasure.V));
                        }
                        else
                        {
                            child.Arrange(new Rect(position.V, position.U, desiredMeasure.V, desiredMeasure.U));
                        }
                    }
                    else if (position.V > realizationBounds.VMax)
                    {
                        return false;
                    }

                    return true;
                }

                for (var i = 0; i < context.ItemCount; i++)
                {
                    bool continueArranging = arrange(state.GetItemAt(i));
                    if (continueArranging == false)
                    {
                        break;
                    }
                }
            }

            return finalSize;
        }
    }
}