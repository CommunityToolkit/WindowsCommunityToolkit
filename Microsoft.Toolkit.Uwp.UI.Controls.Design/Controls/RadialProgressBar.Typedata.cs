// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
#if VS_DESIGNER_PROCESS_ISOLATION
    internal static partial class ControlTypes
    {
        internal static readonly Type RadialProgressBar = typeof(RadialProgressBar);
    }
#else
    internal static partial class ControlTypes
    {
        internal const string RadialProgressBar = RootNamespace + "." + nameof(RadialProgressBar);
    }

    internal static class RadialProgressBar
    {
        internal const string Outline = nameof(Outline);
        internal const string Thickness = nameof(Thickness);
        internal const string Value = nameof(Value);
    }
#endif
}