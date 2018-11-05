// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CompositionPathGeometry : CompositionGeometry
    {
        internal CompositionPathGeometry() { }
        internal CompositionPathGeometry(CompositionPath path) { Path = path; }
        public CompositionPath Path { get; set; }

        public override CompositionObjectType Type => CompositionObjectType.CompositionPathGeometry;
    }
}
