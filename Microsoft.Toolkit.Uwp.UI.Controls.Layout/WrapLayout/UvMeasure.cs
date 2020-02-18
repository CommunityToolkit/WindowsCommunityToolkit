﻿using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    [System.Diagnostics.DebuggerDisplay("U = {U} V = {V}")]
    internal struct UvMeasure
    {
        internal static readonly UvMeasure Zero = default(UvMeasure);

        internal double U { get; set; }

        internal double V { get; set; }

        public UvMeasure(Orientation orientation, double width, double height)
        {
            if (orientation == Orientation.Horizontal)
            {
                U = width;
                V = height;
            }
            else
            {
                U = height;
                V = width;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is UvMeasure measure)
            {
                return (measure.U == U) && (measure.V == V);
            }

            return false;
        }
    }
}
