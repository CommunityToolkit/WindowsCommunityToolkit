// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class InsetClip : CompositionClip
    {
        internal InsetClip() { }

        // Default is 0.
        public float LeftInset { get; set; }
        // Default is 0.
        public float RightInset { get; set; }
        // Default is 0.
        public float BottomInset { get; set; }
        // Default is 0.
        public float TopInset { get; set; }

        public override CompositionObjectType Type => CompositionObjectType.InsetClip;
    }

}
