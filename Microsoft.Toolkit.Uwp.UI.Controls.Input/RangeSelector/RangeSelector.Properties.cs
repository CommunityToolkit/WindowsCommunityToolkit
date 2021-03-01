// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// RangeSelector is a "double slider" control for range values.
    /// </summary>
    public partial class RangeSelector : Control
    {
        /// <summary>
        /// Identifies the Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                nameof(Minimum),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultMinimum, MinimumChangedCallback));

        /// <summary>
        /// Identifies the Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultMaximum, MaximumChangedCallback));

        /// <summary>
        /// Identifies the RangeMin dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeMinProperty =
            DependencyProperty.Register(
                nameof(RangeMin),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultMinimum, RangeMinChangedCallback));

        /// <summary>
        /// Identifies the RangeMax dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeMaxProperty =
            DependencyProperty.Register(
                nameof(RangeMax),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultMaximum, RangeMaxChangedCallback));

        /// <summary>
        /// Identifies the StepFrequency dependency property.
        /// </summary>
        public static readonly DependencyProperty StepFrequencyProperty =
            DependencyProperty.Register(
                nameof(StepFrequency),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultStepFrequency));

        /// <summary>
        /// Gets or sets the minimum value of the range.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum value of the range.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Gets or sets the current lower limit value of the range.
        /// </summary>
        /// <value>
        /// The current lower limit.
        /// </value>
        public double RangeMin
        {
            get => (double)GetValue(RangeMinProperty);
            set => SetValue(RangeMinProperty, value);
        }

        /// <summary>
        /// Gets or sets the current upper limit value of the range.
        /// </summary>
        /// <value>
        /// The current upper limit.
        /// </value>
        public double RangeMax
        {
            get => (double)GetValue(RangeMaxProperty);
            set => SetValue(RangeMaxProperty, value);
        }

        /// <summary>
        /// Gets or sets the value part of a value range that steps should be created for.
        /// </summary>
        /// <value>
        /// The value part of a value range that steps should be created for.
        /// </value>
        public double StepFrequency
        {
            get => (double)GetValue(StepFrequencyProperty);
            set => SetValue(StepFrequencyProperty, value);
        }

        private static void MinimumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => AdjustForBoundsChange(Bound.Minimum, d, e);

        private static void MaximumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => AdjustForBoundsChange(Bound.Maximum, d, e);

        private static void AdjustForBoundsChange(Bound changedBound, DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;

            if (rangeSelector == null)
            {
                return;
            }

            var newValue = (double)e.NewValue;
            var oldValue = (double)e.OldValue;

            switch (changedBound)
            {
                case Bound.Minimum:
                    if (rangeSelector.Maximum < newValue)
                    {
                        rangeSelector.Maximum = newValue + Epsilon;
                    }

                    if (rangeSelector.RangeMin < newValue)
                    {
                        rangeSelector.RangeMin = newValue;
                    }

                    if (rangeSelector.RangeMax < newValue)
                    {
                        rangeSelector.RangeMax = newValue;
                    }

                    break;
                case Bound.Maximum:
                    if (rangeSelector.Minimum > newValue)
                    {
                        rangeSelector.Minimum = newValue - Epsilon;
                    }

                    if (rangeSelector.RangeMax > newValue)
                    {
                        rangeSelector.RangeMax = newValue;
                    }

                    if (rangeSelector.RangeMin > newValue)
                    {
                        rangeSelector.RangeMin = newValue;
                    }

                    break;
            }

            if (newValue != oldValue)
            {
                rangeSelector.SyncThumbs();
            }
        }

        private static void RangeMinChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => AdjustForRangeBoundsChange(Bound.Minimum, d, e);

        private static void RangeMaxChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => AdjustForRangeBoundsChange(Bound.Maximum, d, e);

        private static void AdjustForRangeBoundsChange(Bound changedBound, DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;
            if (rangeSelector == null)
            {
                return;
            }

            var newValue = (double)e.NewValue;

            switch (changedBound)
            {
                case Bound.Minimum:
                    rangeSelector._minSet = true;

                    rangeSelector.RangeMinToStepFrequency();

                    if (newValue < rangeSelector.Minimum)
                    {
                        rangeSelector.RangeMin = rangeSelector.Minimum;
                    }
                    else if (newValue > rangeSelector.Maximum)
                    {
                        rangeSelector.RangeMin = rangeSelector.Maximum;
                    }

                    rangeSelector.SyncActiveRectangle();

                    // If the new value is greater than the old max, move the max also
                    if (newValue > rangeSelector.RangeMax)
                    {
                        rangeSelector.RangeMax = newValue;
                    }

                    break;
                case Bound.Maximum:
                    rangeSelector._maxSet = true;

                    rangeSelector.RangeMaxToStepFrequency();

                    if (newValue < rangeSelector.Minimum)
                    {
                        rangeSelector.RangeMax = rangeSelector.Minimum;
                    }
                    else if (newValue > rangeSelector.Maximum)
                    {
                        rangeSelector.RangeMax = rangeSelector.Maximum;
                    }

                    rangeSelector.SyncActiveRectangle();

                    // If the new max is less than the old minimum then move the minimum
                    if (newValue < rangeSelector.RangeMin)
                    {
                        rangeSelector.RangeMin = newValue;
                    }

                    break;
            }

            rangeSelector.SyncThumbs();
        }

        private enum Bound
        {
            Minimum,
            Maximum
        }
    }
}
