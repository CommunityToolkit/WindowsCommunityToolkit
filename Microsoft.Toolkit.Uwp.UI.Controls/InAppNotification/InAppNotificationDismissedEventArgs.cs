using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// EventArgs class for InAppNotification's dismissed event.
    /// </summary>
    public class InAppNotificationDismissedEventArgs : EventArgs
    {
        public DateTime OpenedTime { get; set; }

        public DateTime DismissedTime { get; set; }
    }
}
