using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class SpaceViewPanelItemArrangedArgs : EventArgs
    {
        public Point XYFromCenter { get; set; }
        public double DistanceFromCenter { get; set; }
        public int ItemIndex { get; set; }
        public UIElement Element { get; set; }
    }
}