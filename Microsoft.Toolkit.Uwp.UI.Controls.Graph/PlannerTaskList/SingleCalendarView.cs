using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class SingleCalendarView : CalendarView
    {
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(DateTimeOffset?), typeof(SingleCalendarView), new PropertyMetadata(null, DatePropertyChanged));

        private static void DatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SingleCalendarView calendarView)
            {
                calendarView._skipSelectedDatesChanged = true;
                calendarView.SelectedDates.Clear();
                if (e.NewValue != null)
                {
                    DateTimeOffset date = (DateTimeOffset)e.NewValue;
                    calendarView.SelectedDates.Add(date);
                    calendarView.SetDisplayDate(date);
                }
                else
                {
                    calendarView.SetDisplayDate(DateTimeOffset.Now);
                }

                calendarView._skipSelectedDatesChanged = false;
            }
        }

        private bool _skipSelectedDatesChanged = false;

        public SingleCalendarView()
        {
            SelectedDatesChanged += SingleCalendarView_SelectedDatesChanged;
        }

        private void SingleCalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (!_skipSelectedDatesChanged)
            {
                if (SelectedDates.Count == 0)
                {
                    Date = null;
                }
                else
                {
                    Date = SelectedDates.First();
                }
            }
        }

        public DateTimeOffset? Date
        {
            get { return (DateTimeOffset?)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
    }
}
