// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An opacity animation working on the composition or XAML layer.
    /// </summary>
    public sealed class OpacityAnimation : ImplicitAnimation<double?, double>
    {
        /// <inheritdoc/>
        protected override string ExplicitTarget => nameof(Visual.Opacity);

        /// <inheritdoc/>
        protected override (double? To, double? From) GetParsedValues()
        {
            return (To, From);
        }
    }
}
