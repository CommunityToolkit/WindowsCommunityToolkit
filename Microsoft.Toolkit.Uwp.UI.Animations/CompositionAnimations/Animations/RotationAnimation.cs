using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Animation that animates the <see cref="Visual.RotationAngle"/> property
    /// </summary>
    public class RotationAnimation : ScalarAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RotationAnimation"/> class.
        /// </summary>
        public RotationAnimation()
        {
            Target = "RotationAngle";
        }
    }
}
