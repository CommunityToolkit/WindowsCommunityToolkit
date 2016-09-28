using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// States of AnimationSet.
    /// </summary>
    public enum AnimationSetState
    {
        /// <summary>
        /// The animation has not been started
        /// </summary>
        NotStarted,

        /// <summary>
        /// The animation has been started and is in progress
        /// </summary>
        Running,

        /// <summary>
        /// The animation has been started and is stopped
        /// </summary>
        Stopped,

        /// <summary>
        /// The animation had completed
        /// </summary>
        Completed
    }
}
