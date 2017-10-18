using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Animation that animates the <see cref="Visual.Scale"/> property
    /// </summary>
    public class ScaleAnimation : Vector3Animation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleAnimation"/> class.
        /// </summary>
        public ScaleAnimation()
        {
            Target = "Scale";
        }
    }
}
