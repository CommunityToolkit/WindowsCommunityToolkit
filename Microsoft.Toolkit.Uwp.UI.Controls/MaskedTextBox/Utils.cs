using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class Utils
    {
        public static bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
        {
            return (value >= minValue) && (value <= maxValue);
        }
    }
}