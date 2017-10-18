using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Animation that animates the <see cref="Visual.RotationAngleInDegrees"/> property
    /// </summary>
    public class RotationInDegreesAnimation : ScalarAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RotationInDegreesAnimation"/> class.
        /// </summary>
        public RotationInDegreesAnimation()
        {
            Target = "RotationAngleInDegrees";
        }
    }
}
