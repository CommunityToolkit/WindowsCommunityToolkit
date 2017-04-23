using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Arguments relating to a CTRL Key Shortcut being activated.
    /// </summary>
    public class ShortcutKeyRequestArgs
    {
        public ShortcutKeyRequestArgs(VirtualKey key, bool shiftKeyHeld)
        {
            Key = key;
            ShiftKeyHeld = shiftKeyHeld;
        }

        /// <summary>
        /// Gets key pressed with CTRL
        /// </summary>
        public VirtualKey Key { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Shift was held down too
        /// </summary>
        public bool ShiftKeyHeld { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Key been Handled
        /// </summary>
        public bool Handled { get; set; } = false;
    }
}