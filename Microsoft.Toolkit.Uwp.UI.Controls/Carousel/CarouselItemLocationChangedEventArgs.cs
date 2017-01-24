using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class CarouselItemLocationChangedEventArgs : EventArgs
    {
        public int OldValue { get; set; }

        public int NewValue { get; set; }
    }
}