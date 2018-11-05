// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class CompositionEasingFunction : CompositionObject
    {
        protected private CompositionEasingFunction() { }
    }
}
