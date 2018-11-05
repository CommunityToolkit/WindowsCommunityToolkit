// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    enum CompositionStrokeLineJoin
    {
        Miter,
        Bevel,
        Round,
        MiterOrBevel,
    }
}
