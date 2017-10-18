using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Animation that animates the <see cref="Visual"/> Translation property
    /// <seealso cref="ElementCompositionPreview.SetIsTranslationEnabled"/>
    /// </summary>
    public class TranslationAnimation : Vector3Animation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationAnimation"/> class.
        /// </summary>
        public TranslationAnimation()
        {
            Target = "Translation";
        }
    }
}
