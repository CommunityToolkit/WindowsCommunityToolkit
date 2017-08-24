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
using System.Reflection;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp.Common
{
    public class SolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return value;
            }

            var brush = (SolidColorBrush)value;

            return brush.Color.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // Check if what we're getting back is a named color so that we can keep it as its name if it is.
            var prop = typeof(Colors).GetTypeInfo().GetDeclaredProperty(value.ToString());

            if (prop != null)
            {
                return value.ToString();
            }

            return new SolidColorBrush(value.ToString().ToColor());
        }
    }
}
