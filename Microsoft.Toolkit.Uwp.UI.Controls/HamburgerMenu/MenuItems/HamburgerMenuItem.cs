using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public interface HamburgerMenuItem
    {
        string Label { get; set; }

        Type TargetPageType { get; set; }
    }
}
