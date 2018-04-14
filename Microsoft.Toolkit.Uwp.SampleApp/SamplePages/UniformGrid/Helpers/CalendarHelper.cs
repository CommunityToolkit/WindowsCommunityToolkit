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
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.UniformGrid.Helpers
{
    [Bindable]
    public class CalendarHelper : DependencyObject
    {
        /// <summary>
        /// Gets or sets a placeholder for us to store a selected day from our UI.
        /// </summary>
        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime), typeof(CalendarHelper), new PropertyMetadata(DateTime.Now));

        public Month ChosenMonth
        {
            get { return (Month)GetValue(ChooseMonthProperty); }
            set { SetValue(ChooseMonthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChooseMonth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChooseMonthProperty =
            DependencyProperty.Register(nameof(ChosenMonth), typeof(Month), typeof(CalendarHelper), new PropertyMetadata((Month)DateTime.Now.Month, OnChosenRangeChanged));

        public int ChosenYear
        {
            get { return (int)GetValue(ChooseYearProperty); }
            set { SetValue(ChooseYearProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChooseYear.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChooseYearProperty =
            DependencyProperty.Register(nameof(ChosenYear), typeof(int), typeof(CalendarHelper), new PropertyMetadata(DateTime.Now.Year, OnChosenRangeChanged));

        public int MonthStartOffset
        {
            get { return (int)GetValue(MonthStartOffsetProperty); }
            set { SetValue(MonthStartOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MonthStartOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MonthStartOffsetProperty =
            DependencyProperty.Register(nameof(MonthStartOffset), typeof(int), typeof(CalendarHelper), new PropertyMetadata(0));

        /// <summary>
        /// Gets an auto-populated list of days based on the ChosenMonth and ChosenYear.
        /// </summary>
        public ObservableCollection<CalendarDay> CalendarDays { get; private set; } = new ObservableCollection<CalendarDay>();

        private static void OnChosenRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Regenerate our list of available calendar days for the chosen month/year
            var calendar = d as CalendarHelper;

            calendar.CalendarDays.Clear();

            for (int day = 1; day <= DateTime.DaysInMonth(calendar.ChosenYear, (int)calendar.ChosenMonth); day++)
            {
                calendar.CalendarDays.Add(new CalendarDay(new DateTime(calendar.ChosenYear, (int)calendar.ChosenMonth, day)));
            }

            // Setup the offset for the top-left of the calendar.
            calendar.MonthStartOffset = (int)calendar.CalendarDays.First().DayOfWeek;
        }
    }

    #pragma warning disable SA1402 // File can only contain one class - keeping here to make easier to copy/update for .code file.
    public class CalendarDay
    {
        public int Day { get; private set; }

        public bool IsWeekend { get; private set; }

        public DateTime Date { get; private set; }

        public DayOfWeek DayOfWeek => Date.DayOfWeek;

        public CalendarDay(DateTime date)
        {
            Date = date;

            Day = date.Day;
            IsWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
    #pragma warning restore SA1402 // File can only contain one class - keeping here to make easier to copy/update for .code file.

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
