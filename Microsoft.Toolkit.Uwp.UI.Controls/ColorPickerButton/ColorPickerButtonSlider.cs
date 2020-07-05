// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A <see cref="Slider"/> implementation for use in the <see cref="ColorPickerButton"/>.
    /// </summary>
    /// <remarks>
    /// 
    /// ColorPickerSlider is currently the same as Slider except it fixes a critical bug.
    /// For details see:
    /// 
    ///  * https://github.com/microsoft/microsoft-ui-xaml/issues/477
    ///  * https://social.msdn.microsoft.com/Forums/sqlserver/en-US/0d3a2e64-d192-4250-b583-508a02bd75e1/uwp-bug-crash-layoutcycleexception-because-of-slider-under-certain-circumstances?forum=wpdevelop
    /// 
    /// An added benefit of being a separate, derived class is more logic can be added
    /// here in the future just like ColorPicker.
    /// 
    /// </remarks>
    public class ColorPickerButtonSlider : Slider
    {
        private Size oldSize;
        private Size measuredSize;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!Size.Equals(oldSize, availableSize))
            {
                measuredSize = base.MeasureOverride(availableSize);
                oldSize = availableSize;
            }

            return measuredSize;
        }
    }
}
