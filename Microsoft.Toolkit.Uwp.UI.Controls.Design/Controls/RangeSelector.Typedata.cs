// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
#if VS_DESIGNER_PROCESS_ISOLATION
    internal static partial class ControlTypes
    {
        internal static readonly Type RangeSelector = typeof(RangeSelector);
    }
#else
    internal static partial class ControlTypes
    {
        internal const string RangeSelector = RootNamespace + "." + nameof(RangeSelector);
    }

    internal static class RangeSelector
    {
        internal const string Maximum = nameof(Maximum);
        internal const string Minimum = nameof(Minimum);
        internal const string RangeMax = nameof(RangeMax);
        internal const string RangeMin = nameof(RangeMin);
    }
#endif
}