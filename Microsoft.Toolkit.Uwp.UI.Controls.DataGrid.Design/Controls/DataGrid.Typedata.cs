// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
#if VS_DESIGNER_PROCESS_ISOLATION
    internal static partial class ControlTypes
    {
        internal static readonly Type DataGrid = typeof(DataGrid);
        internal static readonly Type DataGridColumn = typeof(DataGridColumn);
        internal static readonly Type DataGridBoundColumn = typeof(DataGridBoundColumn);
        internal static readonly Type DataGridTextColumn = typeof(DataGridTextColumn);
        internal static readonly Type DataGridCheckBoxColumn = typeof(DataGridCheckBoxColumn);
        internal static readonly Type DataGridTemplateColumn = typeof(DataGridTemplateColumn);
    }
#endif
}