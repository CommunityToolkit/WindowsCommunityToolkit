// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Vector3Animation that animates the <see cref="Visual"/> Translation property
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

        /// <inheritdoc/>
        public override CompositionAnimation GetCompositionAnimation(Compositor compositor)
        {
            if (ApiInformationHelper.IsCreatorsUpdateOrAbove)
            {
                return base.GetCompositionAnimation(compositor);
            }
            else
            {
                return null;
            }
        }
    }
}
