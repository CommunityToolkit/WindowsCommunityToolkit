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
        private bool _updatingBounds;
        private bool _updatingRangeValues;

        /// <summary>
        /// Identifies the <see cref="Minimum"/> property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                nameof(Minimum),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultMinimum, MinimumChangedCallback));

        /// <summary>
        /// Identifies the <see cref="Maximum"/> property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultMaximum, MaximumChangedCallback));

        /// <summary>
        /// Identifies the <see cref="RangeStart"/> property.
        /// </summary>
        public static readonly DependencyProperty RangeStartProperty =
            DependencyProperty.Register(
                nameof(RangeStart),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultMinimum, RangeStartChangedCallback));

        /// <summary>
        /// Identifies the <see cref="RangeEnd"/> property.
        /// </summary>
        public static readonly DependencyProperty RangeEndProperty =
            DependencyProperty.Register(
                nameof(RangeEnd),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultMaximum, RangeEndChangedCallback));

        /// <summary>
        /// Identifies the <see cref="StepFrequency"/> property.
        /// </summary>
        public static readonly DependencyProperty StepFrequencyProperty =
            DependencyProperty.Register(
                nameof(StepFrequency),
                typeof(double),
                typeof(RangeSelector),
                new PropertyMetadata(DefaultStepFrequency));

        /// <summary>
        /// Gets or sets the absolute minimum value of the range.
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
        /// Gets or sets the absolute maximum value of the range.
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
        /// Gets or sets the current selected lower limit value of the range, modifiable by the user.
        /// </summary>
        /// <value>
        /// The current lower limit.
        /// </value>
        public double RangeStart
        {
            get => (double)GetValue(RangeStartProperty);
            set => SetValue(RangeStartProperty, value);
        }

        /// <summary>
        /// Gets or sets the current selected upper limit value of the range, modifiable by the user.
        /// </summary>
        /// <value>
        /// The current upper limit.
        /// </value>
        public double RangeEnd
        {
            get => (double)GetValue(RangeEndProperty);
            set => SetValue(RangeEndProperty, value);
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

        private static void MinimumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => AdjustForBoundsChanged(Bound.Minimum, d, e);

        private static void MaximumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => AdjustForBoundsChanged(Bound.Maximum, d, e);

        private static void RangeStartChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => AdjustForRangeValuesChanged(Bound.Minimum, d, e);

        private static void RangeEndChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => AdjustForRangeValuesChanged(Bound.Maximum, d, e);

        private static void AdjustForBoundsChanged(Bound changedBound, DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;

            if (rangeSelector == null)
            {
                return;
            }

            if (rangeSelector._updatingBounds)
            {
                return;
            }

            rangeSelector._updatingBounds = true;

            var newValue = (double)e.NewValue;
            var oldValue = (double)e.OldValue;

            switch (changedBound)
            {
                case Bound.Minimum:
                    if (rangeSelector.Maximum < newValue)
                    {
                        rangeSelector.Maximum = newValue + Epsilon;
                    }

                    if (rangeSelector.RangeStart < newValue)
                    {
                        rangeSelector.RangeStart = newValue;
                    }

                    if (rangeSelector.RangeEnd < newValue)
                    {
                        rangeSelector.RangeEnd = newValue;
                    }

                    break;
                case Bound.Maximum:
                    if (rangeSelector.Minimum > newValue)
                    {
                        rangeSelector.Minimum = newValue - Epsilon;
                    }

                    if (rangeSelector.RangeEnd > newValue)
                    {
                        rangeSelector.RangeEnd = newValue;
                    }

                    if (rangeSelector.RangeStart > newValue)
                    {
                        rangeSelector.RangeStart = newValue;
                    }

                    break;
            }

            if (newValue != oldValue)
            {
                rangeSelector.SyncThumbs();
            }

            rangeSelector._updatingBounds = false;
        }

        private static void AdjustForRangeValuesChanged(Bound changedBound, DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;
            if (rangeSelector == null)
            {
                return;
            }

            if (rangeSelector._updatingRangeValues)
            {
                return;
            }

            rangeSelector._updatingRangeValues = true;

            var newValue = (double)e.NewValue;
            var minimum = rangeSelector.Minimum;
            var maximum = rangeSelector.Maximum;
            var rangeMin = rangeSelector.RangeStart;
            var rangeMax = rangeSelector.RangeEnd;
            var stepFrequency = rangeSelector.StepFrequency;

            switch (changedBound)
            {
                case Bound.Minimum:
                    rangeSelector._minSet = true;

                    rangeSelector.RangeStart = minimum + DistanceFromBound(minimum, stepFrequency, rangeMin);

                    if (newValue < rangeSelector.Minimum)
                    {
                        rangeSelector.RangeStart = rangeSelector.Minimum;
                    }
                    else if (newValue > rangeSelector.Maximum)
                    {
                        rangeSelector.RangeStart = rangeSelector.Maximum;
                    }

                    rangeSelector.SyncActiveRectangle();

                    // If the new value is greater than the old max, move the max also
                    if (newValue > rangeSelector.RangeEnd)
                    {
                        rangeSelector.RangeEnd = newValue;
                    }

                    break;
                case Bound.Maximum:
                    rangeSelector._maxSet = true;

                    rangeSelector.RangeEnd = maximum - DistanceFromBound(maximum, stepFrequency, rangeMax);

                    if (newValue < rangeSelector.Minimum)
                    {
                        rangeSelector.RangeEnd = rangeSelector.Minimum;
                    }
                    else if (newValue > rangeSelector.Maximum)
                    {
                        rangeSelector.RangeEnd = rangeSelector.Maximum;
                    }

                    rangeSelector.SyncActiveRectangle();

                    // If the new max is less than the old minimum then move the minimum
                    if (newValue < rangeSelector.RangeStart)
                    {
                        rangeSelector.RangeStart = newValue;
                    }

                    break;
            }

            rangeSelector.SyncThumbs();

            rangeSelector._updatingRangeValues = false;
        }

        private enum Bound
        {
            Minimum,
            Maximum
        }

        private static double DistanceFromBound(double bound, double stepLength, double value)
        {
            var difference = Math.Abs(bound - value);
            var steps = (int)Math.Round(difference / stepLength);
            return steps * stepLength;
        }
    }
}
