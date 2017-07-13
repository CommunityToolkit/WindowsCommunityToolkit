using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public interface IHamburgerMenuItem
    {
        string Label { get; set; }

        Type TargetPageType { get; set; }
    }
}
