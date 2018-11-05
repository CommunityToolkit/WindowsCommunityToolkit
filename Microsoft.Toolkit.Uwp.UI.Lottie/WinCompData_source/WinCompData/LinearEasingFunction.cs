// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class LinearEasingFunction : CompositionEasingFunction
    {
        internal LinearEasingFunction() { }

        public override CompositionObjectType Type => CompositionObjectType.LinearEasingFunction;
    }
}
