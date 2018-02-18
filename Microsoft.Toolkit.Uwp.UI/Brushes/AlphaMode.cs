using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Brushes
{
    /// <summary>
    /// Specifies the way in which an alpha channel affects color channels.
    /// </summary>
    public enum AlphaMode
    {
        /// <summary>
        /// Provides better transparent effects without a white bloom.
        /// </summary>
        Premultiplied = 0,

        /// <summary>
        /// WPF default handling of alpha channel during transparent blending.
        /// </summary>
        Straight = 1,
    }
}
