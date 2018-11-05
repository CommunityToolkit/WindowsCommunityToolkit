// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Mgc
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CanvasDevice : IDisposable
    {
        public static CanvasDevice GetSharedDevice() => new CanvasDevice();

        public void Dispose()
        {
        }
    }
}
