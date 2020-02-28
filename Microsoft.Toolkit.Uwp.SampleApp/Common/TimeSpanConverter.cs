// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.Common
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Duration dur)
            {
                return dur.TimeSpan.TotalMilliseconds;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is double milliseconds)
            {
                return DurationHelper.FromTimeSpan(TimeSpan.FromMilliseconds(milliseconds));
            }

            return DurationHelper.FromTimeSpan(TimeSpan.MinValue);
        }
    }
}
