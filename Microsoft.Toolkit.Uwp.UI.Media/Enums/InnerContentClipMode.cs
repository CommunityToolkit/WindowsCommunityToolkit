using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// The method that shadows deriving from <see cref="AttachedCardShadowBase"/> use when clipping their inner content.
    /// </summary>
    public enum InnerContentClipMode
    {
        /// <summary>
        /// Use <see cref="Windows.UI.Composition.CompositionMaskBrush"/> to clip inner content.
        /// </summary>
        /// <remarks>
        /// This mode has better performance than <see cref="CompositionGeometricClip"/>.
        /// </remarks>
        CompositionMaskBrush,

        /// <summary>
        /// Use <see cref="Windows.UI.Composition.CompositionGeometricClip"/> to clip inner content.
        /// </summary>
        /// <remarks>
        /// Content clipped in this mode will have smoother corners than when using <see cref="CompositionMaskBrush"/>.
        /// </remarks>
        CompositionGeometricClip
    }
}
