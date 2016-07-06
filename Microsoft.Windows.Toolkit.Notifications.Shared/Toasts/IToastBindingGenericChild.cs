using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Windows.Toolkit.Notifications
{
#if ANNIVERSARY_UPDATE
    /// <summary>
    /// Elements that can be direct children of <see cref="ToastBindingGeneric"/>, including (<see cref="AdaptiveText"/>, <see cref="AdaptiveImage"/>, and <see cref="AdaptiveGroup"/>).
    /// </summary>
#else
    /// <summary>
    /// Elements that can be direct children of <see cref="ToastBindingGeneric"/>, including (<see cref="AdaptiveText"/> and <see cref="AdaptiveImage"/>.
    /// </summary>
#endif
    public interface IToastBindingGenericChild
    {
        // Blank interface simply for compile-enforcing the child types in the list.
    }
}
