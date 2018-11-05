// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Char
    {
        public Char(
              string characters,
              string fontFamily,
              string style,
              double fontSize,
              double width,
              IEnumerable<ShapeLayerContent> shapes)
        {
        }
    }
}
