// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// This class converts a double value into a Visibility enumeration.
    /// </summary>
    [Bindable]
    public class DoubleToVisibilityConverter : DoubleToObjectConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleToVisibilityConverter"/> class.
        /// </summary>
        public DoubleToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
            NullValue = Visibility.Collapsed;
        }
    }
}
