using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
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
    }
}
