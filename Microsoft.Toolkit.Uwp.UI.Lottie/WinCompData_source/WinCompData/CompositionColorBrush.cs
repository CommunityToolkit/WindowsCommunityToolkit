// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CompositionColorBrush : CompositionBrush
    {
        internal CompositionColorBrush(Wui.Color color) { Color = color; }
        public Wui.Color Color { get; set; }

        public override CompositionObjectType Type => CompositionObjectType.CompositionColorBrush;
    }
}
